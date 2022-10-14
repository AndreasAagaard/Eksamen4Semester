namespace mongo_service.Services;
using MongoDB.Driver;
using mongo_service.Models;

public class CustomerService
{
    private readonly IMongoCollection<MongoCustomer> _customers = new();
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ILogger<CustomerService> logger)
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
    public MongoCustomer GetCustomer(string id)
    {
        MongoCustomer customer = new MongoCustomer { };
        try 
        {
            customer = _customers.Find(customer => customer._id == id).First();
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
            _customers.InsertOne(customer);
            _logger.LogInformation("Succesfully created new customer");
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
        }
    }

    private void SeedData() 
    {
        MongoCustomer customer1 = new("Nicolai", "27091092", "jegerrockstar@cool.com");
        MongoCustomer customer2 = new("Nicolai", "27091092", "jegerrockstar@cool.com");
        MongoCustomer customer3 = new("Nicolai", "27091092", "jegerrockstar@cool.com");
        MongoCustomer customer4 = new("Nicolai", "27091092", "jegerrockstar@cool.com");

        CreateCustomer(customer1);
        CreateCustomer(customer2);
        CreateCustomer(customer3);
        CreateCustomer(customer4);
    }
}