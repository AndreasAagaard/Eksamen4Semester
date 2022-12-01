using System;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using BiddingHandler.Models;

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

                channel.BasicPublish(exchange: "",
                                    routingKey: bid.AuctionId.ToString(),
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
}