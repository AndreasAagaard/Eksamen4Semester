using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using auction_service.Models;

namespace auction_service.Service;

/// <summary>
/// MongoDB database context class.
/// </summary>
public class MongoDBContext
{
    private ILogger<MongoDBContext> _logger;
    private IConfiguration _config;
    public IMongoDatabase Database { get; set; }
    public IMongoCollection<AuctionItemDTO> Collection { get; set; }
    private MongoClient client { get; set; }


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
        _logger.LogInformation(_config["MongoConnectionString"]);
        RetryMongoConnection(_config["MongoConnectionString"]);

        logger.LogInformation($"Connected to database {Database}");
        logger.LogInformation($"Using collection {Collection}");


       
    async Task RetryMongoConnection(string? connectionString)
        {
            int currentRetry = 0;
            for (;;)
            {
                try
                {
                    // Call external service.
                    client = new MongoClient(connectionString);
                    Database = client.GetDatabase(_config["Database"]);
                    Collection = Database.GetCollection<AuctionItemDTO>(_config["Collection"]);

                    _logger.LogInformation("Connection established");

                    // Return or break.
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogTrace($"AddCustomer exception: {ex.Message}");

                    currentRetry++;
                    if (currentRetry >= 20 || !IsTransient(ex))
                    {
                        _logger.LogCritical(ex, ex.Message);
                        throw;
                    }
                    _logger.LogInformation("Trying again");
                }

                // Wait to retry the operation.
                // Consider calculating an exponential delay here and
                // using a strategy best suited for the operation and fault.
                await Task.Delay(3000);
            }
        }
        bool IsTransient(Exception ex)
        {
            _logger.LogInformation($"Checking if exception {ex.GetType().ToString()} is transient");
            return true;
        }
    }


}
