using auction_service.Models;
using MongoDB.Bson.Serialization;
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

        // foreach (var a in auctionsDTO){
        //     ProductItemDTO proItem = Catalog.Where(x => x.ProductId == a.ProductId).First();
        //     AuctionItem aucItem = new AuctionItem{ 
        //         AuctionId = a.AuctionId,
        //         Product = proItem,
        //         AuctionEnds = a.AuctionEnds,
        //         Offers = a.Offers
        //     };
        //     auctions.Add(aucItem);
        // }

        if (auctions == null)
            return null;
        return auctions;    
    }

    public async Task<Guid?> CreateAuction(AuctionItemDTO auction)
    {
        auction.AuctionId = Guid.NewGuid();
        await _retry.VoidRetryFunction(_collection.InsertOneAsync(auction));

         using HttpClient client = new() 
            { 
                BaseAddress = new Uri(_config["CatalogHostName"])
            }; 
        List<ProductItemDTO> Catalog = await client.GetFromJsonAsync<List<ProductItemDTO>>("catalog/getproduct"); 

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