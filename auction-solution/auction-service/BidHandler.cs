using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using auction_service.Service;
using auction_service.Models;

public class BidHandler : BackgroundService
{
    private readonly ILogger<BidHandler> _logger;
    private readonly IAuctionService _service;
    private readonly string mqhostname;
    private readonly IConnection _connection;

    public BidHandler(ILogger<BidHandler> logger, IAuctionService service, IConfiguration conf)
    {
        _logger = logger;
        _service = service;

        mqhostname = conf["AuctionBrokerHost"];
        _logger.LogInformation($"Using host at {mqhostname} for message broker");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try 
            {
                await GetNewBid();
                //Hw();
                _logger.LogInformation("Connnection established at: {time}", DateTimeOffset.Now);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            await Task.Delay(3000, stoppingToken);
        }

        _logger.LogWarning("Worker stopped at: {time}", DateTimeOffset.Now);
    }

    public async Task GetNewBid() {
        var factory = new ConnectionFactory() { HostName = mqhostname, DispatchConsumersAsync = true };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "auction",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                _logger.LogWarning("Bid received");
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                BiddingItemDTO? bidding = JsonSerializer.Deserialize<BiddingItemDTO>(message);
                
                AuctionItemDTO? auction = await _service.GetAuction(bidding.AuctionId);
                if (auction != null)
                    _logger.LogInformation("Auction found");
                else
                    _logger.LogError("Bid not registered. Something wrong happened getting auction with id: " + bidding.AuctionId);
                
                OfferItemDTO? offer = new(bidding.OfferId, bidding.UserId, bidding.Offer, bidding.Timestamp);
                if (offer != null)
                {
                    _logger.LogInformation("Processing offer {id} from {auctionid} ", offer.OfferId, auction.AuctionId);
                    if(offer.Timestamp < auction.AuctionEnds && offer.Offer > auction.GetHighestOffer())
                    {
                        await _service.AddOfferToAuction(bidding.AuctionId, offer);
                    }
                    else
                    {
                        _logger.LogInformation($"New offer is not higher than current or out of date");
                    }
                }
                else 
                {
                    _logger.LogWarning($"Could not deserialize message with body: {message}");
                }

                
                Console.WriteLine(" [x] Bid processed");

                await Task.Yield();
            };
            channel.BasicConsume(queue: "auction",
                                 autoAck: true,
                                 consumer: consumer);

        }
    }

    // public void Hw() {
    //     var factory = new ConnectionFactory() { HostName = mqhostname };
    //     using(var connection = factory.CreateConnection())
    //     using(var channel = connection.CreateModel())
    //     {
    //         channel.QueueDeclare(queue: "hello",
    //                              durable: false,
    //                              exclusive: false,
    //                              autoDelete: false,
    //                              arguments: null);

    //         var consumer = new EventingBasicConsumer(channel);
    //         consumer.Received += (model, ea) =>
    //         {
    //             var body = ea.Body.ToArray();
    //             Console.WriteLine(" [x] after body");
    //             var message = Encoding.UTF8.GetString(body);
    //             Console.WriteLine(message);
    //         };
    //         channel.BasicConsume(queue: "hello",
    //                              autoAck: true,
    //                              consumer: consumer);
            
    //     }

    // }

    // protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    // {
    //     var channel = _connection.CreateModel();
    //     channel.QueueDeclare(queue: "auction",
    //                         durable: false,
    //                         exclusive: false,
    //                         autoDelete: false,
    //                         arguments: null);

    //     var consumer = new EventingBasicConsumer(channel);
    //     consumer.Received += (model, ea) =>
    //     {
    //         var auctionId = Guid.Parse(ea.RoutingKey);
    //         _logger.LogWarning("Bid received to auction: " + auctionId);
    //         AuctionItemDTO? auction = _service.GetAuction(auctionId).GetAwaiter().GetResult();
    //         if (auction == null)
    //         {
    //             _logger.LogWarning("Bid not registered. Something wrong happened getting auction with id: " + auctionId);
    //         }

    //         var body = ea.Body.ToArray();
    //         var message = Encoding.UTF8.GetString(body);
    //         OfferItemDTO? offer = JsonSerializer.Deserialize<OfferItemDTO>(message);
    //         if (offer != null)
    //         {
    //             _logger.LogInformation("Processing offer {id} from {auctionid} ", offer.OfferId, auction.AuctionId);
    //             if(offer.Timestamp < auction.AuctionEnds && offer.Offer > auction.GetHighestOffer().Offer)
    //             {
    //                 _service.AddOfferToAuction(auctionId, offer);
    //                 _logger.LogInformation("Bid accepted");
    //             }
    //             else
    //                 _logger.LogInformation($"New offer is not higher than current or out of date");
    //         } else 
    //         {
    //             _logger.LogWarning($"Could not deserialize message with body: {message}");
    //         }

    //     };

    //     channel.BasicConsume(queue: "auction",
    //                         autoAck: true,
    //                         consumer: consumer);

    //     while (!stoppingToken.IsCancellationRequested)
    //     {
    //         _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
    //         await Task.Delay(3000, stoppingToken);
    //         ExecuteAsync(stoppingToken);
    //     }
    // }
}