using Microsoft.AspNetCore.Mvc;
using customer_service.Models;
using customer_service.Services;

namespace customer_service.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerServiceController : ControllerBase
{
    private readonly ILogger<CustomerServiceController> _logger;
    private readonly CustomerService _customerService;

    public CustomerServiceController(ILogger<CustomerServiceController> logger, CustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    [HttpGet]
    public List<Customer> Get()
    {
        return _customerService.GetCustomers();
    }


    [HttpPost("createcustomer", Name = "CreateCustomer")]
    public Customer? PostCustomer(Customer customer)
    {
        try
        {
            var res = _customerService.CreateCustomer(customer);

            if (res == null)
            {
                return null;
            }

            return res;
        }
        catch (Exception ex)
        {
            LogError(ex);
            return null;
        }
    }


    private void LogError(Exception ex) 
    {
        _logger.LogError(ex.Message);
    }
}
