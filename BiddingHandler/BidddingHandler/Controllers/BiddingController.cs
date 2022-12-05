using System;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using BiddingHandler.Models;
using RabbitMQ.Client.Events;

namespace BiddingHandler.Controllers;

/// <summary>
/// This controller exposes a microservices HTTP interface for receiving
/// taxa booking messages. Data is relayed to a common message-broker for
/// further processing.
/// </summary>
[ApiController]
[Route("[controller]")]
public class BiddingController : ControllerBase
{
    private readonly ILogger<BiddingController> _logger;
    private IConnection _connection;
    private readonly string mqhostname;
    private Int32 NextId { get; set; }

    /// <summary>
    /// Inject a logger service into the controller on creation.
    /// </summary>
    /// <param name="logger">The logger service.</param>
    public BiddingController(ILogger<BiddingController> logger, IConfiguration configuration)
    {
        _logger = logger;

        mqhostname = configuration["AuctionBrokerHost"];

        _logger.LogInformation($"Using host at {mqhostname} for message broker");
    }

    /// <summary>
    /// Endpoint for recieving bids.
    /// </summary>
    /// <param name="bid">A bidding object</param>
    /// <returns>On success - the bid object with bid id and received date.</returns>
    [HttpPost]
    public async Task<BiddingItemDTO?> Post(BiddingItemDTO bid)
    {
        _logger.LogInformation($"Post bid is running");
        bid.OfferId = NextId++;
        bid.Timestamp = DateTime.Now;

        try {
            var factory = new ConnectionFactory() { HostName = mqhostname };
            _connection = factory.CreateConnection();

            using(var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: "auction",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var body = JsonSerializer.SerializeToUtf8Bytes(bid);

                var Exchange = bid.AuctionId.ToString();
                channel.BasicPublish(exchange: Exchange,
                                    routingKey: "auction",
                                    basicProperties: null,
                                    body: body);
            }
        } catch (Exception ex) {
            _logger.LogError(ex, "Something went wrong with the bid");
            return null;
        }

        _logger.LogInformation("Bid has succesfully been sent to queue");
        return bid;

    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: "taxabooking",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            object dto = JsonSerializer.Deserialize<object>(message);
            if (dto != null)
            {
                dto.BookingID = _nextID++;
                _logger.LogInformation("Processing booking {id} from {customer} ", dto.BookingID, dto.CustomerName);

                _repository.Put(dto);

            }
            else
            {
                _logger.LogWarning($"Could not deserialize message with body: {message}");
            }

        };

        channel.BasicConsume(queue: "taxabooking",
                            autoAck: true,
                            consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}