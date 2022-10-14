namespace mongo_service.Services;
using MongoDB.Driver;
using mongo_service.Models;

public class ItemService
{
    private readonly MongoCollection<MongoItem> _items = new();
    private readonly ILogger<ItemService> _logger;

    public ItemService(ILogger<ItemService> logger)
    {
        _logger = logger;

        try 
        {
            SeedData();
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
        }    
    }
    public MongoItem GetItem(string id)
    {
        MongoItem item = new MongoItem { };
        try 
        {
            item = _items.Find(item => item._id == id).First();
            return item; 
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
            return item;
        }
    }
    public void CreateItem(MongoItem item)
    {
        try 
        {
            _items.InsertOne(item);
            _logger.LogInformation("Succesfully created new item");
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
        }
    }

    private void SeedData() 
    {
        MongoItem item1 = new("Løve statue");
        MongoItem item2 = new("Guld tand");
        MongoItem item3 = new("Martins gamle trusser");
        MongoItem item4 = new("Coka Cola");

        CreateItem(item1);
        CreateItem(item2);
        CreateItem(item3);
        CreateItem(item4);
    }
}