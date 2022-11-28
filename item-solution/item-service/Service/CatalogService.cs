using item_service.Models;
using MongoDB.Driver;


namespace item_service.Service;

public interface ICatalogService
{
    Task<ProductItemDTO?> GetProduct(Guid productId);
    Task<Guid?> CreateProduct(ProductItemDTO item);
}

public class CatalogService : ICatalogService
{
    private int id = 1;
    public List<ProductItemDTO> Catalog = new List<ProductItemDTO>();

    private ILogger<CatalogService> _logger;
    private IMongoDatabase _database;
    private IMongoCollection<ProductItemDTO> _collection;
    public CatalogService(ILogger<CatalogService> logger, MongoDBContext dbcontext)
    {
        _logger = logger;
        _database = dbcontext.Database;        
        _collection = dbcontext.Collection;
    }

    public async Task<ProductItemDTO?> GetProduct(Guid productId)
    {
        ProductItemDTO? product = null;
        var filter = Builders<ProductItemDTO>.Filter.Eq(x => x.ProductId, productId);
        
        try
        {
            product = await _collection.Find(filter).SingleOrDefaultAsync();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return product;    
    }

    public async Task<Guid?> CreateProduct(ProductItemDTO item)
    {
        Guid? result = null;
        try {
            item.ProductId = Guid.NewGuid();
            await _collection.InsertOneAsync(item);
            result = item.ProductId;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return result;
    }

    private void LogError(Exception ex) 
    {
        _logger.LogError(ex.Message);
    }
}