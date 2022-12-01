using auction_service.Models;
using MongoDB.Driver;


namespace auction_service.Service;

public interface IAuctionService
{
}

public class AuctionService : IAuctionService
{
    private int id = 1;
    public List<ProductItemDTO> Catalog = new List<ProductItemDTO>();
    private ILogger<AuctionService> _logger;
    private IMongoDatabase _database;
    private IMongoCollection<ProductItemDTO> _collection;
    public AuctionService(ILogger<AuctionService> logger, MongoDBContext dbcontext)
    {
        _logger = logger;
        _database = dbcontext.Database;        
        _collection = dbcontext.Collection;
    }

    public async Task<AuctionItemDTO> GetAuction(Guid auctionId)
    {
        //ProductItemDTO? product = null;
        var filter = Builders<AuctionItemDTO>.Filter.Eq(x => x.AuctionId, auctionId);
        
        AuctionItemDTO? auction = await _collection.Find(filter).SingleOrDefaultAsync();
        
        return product;    
    }

    public async Task<Guid?> CreateProduct(ProductItemDTO item)
    {
        item.ProductId = Guid.NewGuid();
        
        await _collection.InsertOneAsync(item);

        var result = item.ProductId;
        return result;
    }

    private void LogError(Exception ex) 
    {
        _logger.LogError(ex.Message);
    }
}