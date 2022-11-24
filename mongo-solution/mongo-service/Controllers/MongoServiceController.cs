using Microsoft.AspNetCore.Mvc;
using mongo_service.Services;
using mongo_service.Models;

namespace mongo_service.Controllers;

[ApiController]
[Route("[controller]")]
public class MongoServiceController : ControllerBase
{
    private readonly CustomerService _customerservice;
    private readonly ItemService _itemservice;
    private readonly ILogger<MongoServiceController> _logger;

    public MongoServiceController(ILogger<MongoServiceController> logger, ItemService itemservice, CustomerService customerservice)
    {
        _itemservice = itemservice;
        _customerservice = customerservice;
        _logger = logger;
    }

    [HttpGet("getitems")]
    public List<MongoItem> GetItems() => _itemservice.GetItems();

    [HttpGet("getcustomers")]
    public List<MongoCustomer> GetCustomers() => _customerservice.GetCustomers();
    [HttpPost("createcustomer")]
    public void CreateCustomer(MongoCustomer customer)
    {
        try 
        {
            _customerservice.CreateCustomer(customer);
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
        }
    }

    [HttpPost("createitem")]
    public void CreateItem(MongoItem item)
    {
        try 
        {
            _itemservice.CreateItem(item);
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
        }
    }
}