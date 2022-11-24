namespace mongo_service.Services;
using MongoDB.Driver;
using mongo_service.Models;

public class ItemService
{
    private readonly ILogger<ItemService> _logger;
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<MongoItem> items;

    public ItemService(ILogger<ItemService> logger, VaultSharpService vault)
    {
        _logger = logger;

        var mongoKey = vault.GetMongoDBString("MongoDBString1").GetAwaiter().GetResult();
        var client = new MongoClient(mongoKey);
        database = client.GetDatabase("Eksamen4Semester");
        items = database.GetCollection<MongoItem>("Items");

        try 
        {
            SeedData();
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
        }    
    }
    public List<MongoItem> GetItems() => items.Find(x => true).ToList();

    public MongoItem GetItem(string id)
    {
        MongoItem item = new MongoItem();
        try 
        {
            item = items.Find(item => item._id == id).First();
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
            items.InsertOne(item);
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