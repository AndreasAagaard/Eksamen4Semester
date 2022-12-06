using auction_service.Models;
using MongoDB.Driver;


namespace auction_service.Service;

public interface IAuctionService
{
}

public class AuctionService : IAuctionService
{
    private int id = 1;
    private ILogger<AuctionService> _logger;
    private IMongoDatabase _database;
    private IMongoCollection<AuctionItemDTO> _collection;
    private readonly RetryService _retry;
    public AuctionService(ILogger<AuctionService> logger, MongoDBContext dbcontext,
        RetryService retry)
    {
        _logger = logger;
        _database = dbcontext.Database;        
        _collection = dbcontext.Collection;
        _retry = retry;
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

    public async Task<Guid?> CreateAuction(AuctionItemDTO auction)
    {
        auction.AuctionId = Guid.NewGuid();
        await _retry.VoidRetryFunction(_collection.InsertOneAsync(auction));
        
        var result = auction.AuctionId;
        return result;
    }

    public async Task AddOfferToAuction(Guid auctionId, OfferItemDTO offer)
    {
        var filter = Builders<AuctionItemDTO>.Filter.Eq(x => x.AuctionId, auctionId);
        var update = Builders<AuctionItemDTO>.Update.AddToSet<OfferItemDTO>("Offers", offer);

        await _retry.RetryFunction(_collection.UpdateOneAsync(filter, update));
    }
}