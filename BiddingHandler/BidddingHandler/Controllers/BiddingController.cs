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
    private readonly string mqhostname;
    private static int NextId;

    public BiddingController(ILogger<BiddingController> logger, IConfiguration configuration)
    {
        _logger = logger;

        mqhostname = configuration["AuctionBrokerHost"];
        _logger.LogInformation($"Using host at {mqhostname} for message broker");
    }
    
    [HttpPost]
    public BiddingItemDTO? PostBid(BiddingItemDTO bid)
    {
        _logger.LogInformation($"Post bid is running");
        bid.OfferId = NextId++;
        bid.Timestamp = DateTime.Now;
        
        try {
            var factory = new ConnectionFactory() { HostName = mqhostname };
            using (var _connection = factory.CreateConnection())
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: "auction",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var body = JsonSerializer.SerializeToUtf8Bytes(bid);

                channel.BasicPublish(exchange: "",
                                    routingKey: "auction",
                                    basicProperties: null,
                                    body: body);
                
                _logger.LogInformation("Bid has succesfully been sent to queue");
            }
        } catch (Exception ex) {
            _logger.LogError(ex.Message);
            return null;
        }
        return bid;
    }

    public void ResetCounter() => NextId = 0; 
}