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

    [HttpPost("createcustomer")]
    public MongoCustomer CreateCustomer(MongoCustomer customer)
    {
        try 
        {
            return _customerservice.CreateCustomer(customer);
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
            return new();
        }
    }

    [HttpPost("createitem")]
    public string CreateItem(MongoItem item)
    {
        try 
        {
            _itemservice.CreateItem(item);
            return "Created id: " + item._id;
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message);
            return "Something went wrong. Please try again";
        }
    }
}