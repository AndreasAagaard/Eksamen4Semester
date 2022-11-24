namespace mongo_service.Services;
using MongoDB.Driver;
using mongo_service.Models;

public class CustomerService
{
    private readonly ILogger<CustomerService> _logger;
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<MongoCustomer> customers;

    public CustomerService(ILogger<CustomerService> logger, VaultSharpService vault)
    {
        _logger = logger;

        var mongoKey = vault.GetMongoDBString("MongoDBString1").GetAwaiter().GetResult();
        var client = new MongoClient(mongoKey);
        database = client.GetDatabase("Eksamen4Semester");
        customers = database.GetCollection<MongoCustomer>("Customers");

        try 
        {
            SeedData();
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
        }    
    }
    public List<MongoCustomer> GetCustomers() => customers.Find(x => true).ToList();
    public MongoCustomer GetCustomer(string id)
    {
        MongoCustomer customer = new MongoCustomer();
        try 
        {
            customer = customers.Find(c => c._id == id).First();
            return customer; 
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
            return customer;
        }
    }

    public void CreateCustomer(MongoCustomer customer)
    {
        try 
        {
            customers.InsertOne(customer);
            _logger.LogInformation("Succesfully created new item");
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
        }
    }
    private void SeedData() 
    {
        CreateCustomer(new MongoCustomer("nicolai", "12345678", "nicolai@gmail.com"));
        CreateCustomer(new MongoCustomer("andreas", "34567812", "andreas@gmail.com"));
        CreateCustomer(new MongoCustomer("line", "56781234", "line@gmail.com"));
        CreateCustomer(new MongoCustomer("sissel", "78123456", "sissel@gmail.com"));
    }
}