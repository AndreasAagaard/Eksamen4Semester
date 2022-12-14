using auction_service.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;


namespace auction_service.Service;

public interface IAuctionService
{
    Task<AuctionItemDTO?> GetAuction(Guid auctionId);
    Task<List<AuctionItemDTO>> GetAllAuctions();
    Task<Guid?> CreateAuction(AuctionItemDTO auction);
    Task AddOfferToAuction(Guid auctionId, OfferItemDTO offer);
}

public class AuctionService : IAuctionService
{
    private ILogger<IAuctionService> _logger;
    private IMongoDatabase _database;
    private IConfiguration _config;
    private IMongoCollection<AuctionItemDTO> _collection;
    private readonly IRetryService _retry;
    
    public AuctionService(ILogger<IAuctionService> logger, MongoDBContext dbcontext,
        IRetryService retry, IConfiguration config)
    {
        _logger = logger;
        _database = dbcontext.Database;        
        _collection = dbcontext.Collection;
        _retry = retry;
        _config = config;
    }

    public async Task<AuctionItemDTO?> GetAuction(Guid auctionId)
    {
        var filter = Builders<AuctionItemDTO>.Filter.Eq(x => x.AuctionId, auctionId);
        
        AuctionItemDTO? auction = await _retry.RetryFunction(
                _collection.Find(filter).SingleOrDefaultAsync());      

        if (auction == null)
            return null;
        return auction;    
    }

    public async Task<List<AuctionItemDTO>?> GetAllAuctions()
    {        
        List<AuctionItemDTO>? auctions = await _retry.RetryFunction(
                _collection.Find(x => true).ToListAsync());    

        if (auctions == null)
            return null;
        return auctions;    
    }

    public async Task<Guid?> CreateAuction(AuctionItemDTO auction)
    {
        auction.AuctionId = Guid.NewGuid();
        await _retry.VoidRetryFunction(
                _collection.InsertOneAsync(auction)
            );

        // HttpClient client = new HttpClient();
        // var host = _config["CatalogHostName"];
        // var response = await _retry.RetryFunction(
        //                 client.DeleteAsync($"http://{host}/catalog/{auction.Product.ProductId}"));

        // if (!response.IsSuccessStatusCode)
        // {
        //     return null;
        //     _logger.LogWarning("Could not delete product");
        // }

        return auction.AuctionId;
    }

    public async Task AddOfferToAuction(Guid auctionId, OfferItemDTO offer)
    {
        var filter = Builders<AuctionItemDTO>.Filter.Eq(x => x.AuctionId, auctionId);
        var update = Builders<AuctionItemDTO>.Update.AddToSet<OfferItemDTO>("Offers", offer);

        await _retry.RetryFunction(_collection.UpdateOneAsync(filter, update));
    }
}