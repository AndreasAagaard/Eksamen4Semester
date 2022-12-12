using auction_service.Models;
using MongoDB.Driver;


namespace auction_service.Service;

public interface IAuctionService
{
    Task<AuctionItemDTO?> GetAuction(Guid auctionId);
    Task<Guid?> CreateAuction(AuctionItemDTO auction);
    Task AddOfferToAuction(Guid auctionId, OfferItemDTO offer);
}

public class AuctionService : IAuctionService
{
    private ILogger<IAuctionService> _logger;
    private IMongoDatabase _database;
    private IMongoCollection<AuctionItemDTO> _collection;
    private readonly IRetryService _retry;
    public AuctionService(ILogger<IAuctionService> logger, MongoDBContext dbcontext,
        IRetryService retry)
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

    public async Task<List<AuctionItemDTO>?> GetAllAuctions()
    {        
        List<AuctionItemDTO>? auction = await _retry.RetryFunction(
                _collection.Find(x => true).ToListAsync());        

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