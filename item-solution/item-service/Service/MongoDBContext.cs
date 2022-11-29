using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using item_service.Models;

namespace item_service.Service;

/// <summary>
/// MongoDB database context class.
/// </summary>
public class MongoDBContext
{
    private ILogger<MongoDBContext> _logger;
    private IConfiguration _config;
    public IMongoDatabase Database { get; set; }
    public IMongoCollection<ProductItemDTO> Collection { get; set; }

    /// <summary>
    /// Create an instance of the context class.
    /// </summary>
    /// <param name="logger">Global logging facility.</param>
    /// <param name="config">System configuration instance.</param>
    public MongoDBContext(ILogger<MongoDBContext> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
        
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        //Add retry and circuit breaker
        //Add secret to vault
        var client = new MongoClient(_config["MongoConnectionString"]);
        Database = client.GetDatabase("AuctionHouse");
        Collection = Database.GetCollection<ProductItemDTO>("Catalog");

        logger.LogInformation($"Connected to database {Database}");
        logger.LogInformation($"Using collection {Collection}");
    }

}
