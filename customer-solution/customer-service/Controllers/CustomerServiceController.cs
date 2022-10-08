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

    [HttpPost("createcustomer", Name = "CreateCustomer")]
    public string Post(Customer customer)
    {
        return _customerService.CreateCustomer(customer);
    }
}
