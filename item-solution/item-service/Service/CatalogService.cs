using item_service.Models;
using MongoDB.Driver;


namespace item_service.Service;

public interface ICatalogService
{
    Task<ProductItemDTO?> GetProduct(Guid productId);
    Task<Guid?> CreateProduct(ProductItemDTO item);
    Task DeleteProduct(Guid id);
}

public class CatalogService : ICatalogService
{
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
        //ProductItemDTO? product = null;
        var filter = Builders<ProductItemDTO>.Filter.Eq(x => x.ProductId, productId);
        
        ProductItemDTO? product = await _collection.Find(filter).SingleOrDefaultAsync();
        
        return product;    
    }

    public async Task<List<ProductItemDTO>?> GetAllProducts()
    {
        //ProductItemDTO? product = null;        
        List<ProductItemDTO>? products = await _collection.Find(x => true).ToListAsync();
        
        return products;    
    }

    public async Task<List<ProductItemDTO>?> GetProductsByCategory(int catId)
    {
        var filter = Builders<ProductItemDTO>.Filter.Eq(x => x.ProductCategory, (ProductCategory)catId);

        List<ProductItemDTO>? products = await _collection.Find(filter).ToListAsync();
        
        return products;    
    }

    public async Task<Guid?> CreateProduct(ProductItemDTO item)
    {
        item.ProductId = Guid.NewGuid();
        
        await _collection.InsertOneAsync(item);

        var result = item.ProductId;
        return result;
    }

    public async Task DeleteProduct(Guid id)
    {
        var deleteFilter = Builders<ProductItemDTO>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(deleteFilter);
    }

    private void LogError(Exception ex) 
    {
        _logger.LogError(ex.Message);
    }
}