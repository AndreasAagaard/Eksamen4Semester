namespace customer_service.Services;
using customer_service.Models;

public class CustomerService
{
    public int Id = 1;
    public List<Customer> Customers = new();
    private readonly ILogger<CustomerService> _logger;
    public CustomerService(ILogger<CustomerService> logger)
    {
        _logger = logger;
    }

    public List<Customer> GetCustomer()
    {
        return Customers;
    }

    public string CreateCustomer(Customer customer)
    {
        try
        {
            customer.CustomerId = Id++;
            Customers.Add(customer);
            _logger.LogInformation("New customer with Id: " + customer.CustomerId +
                                        " created at timestamp " + DateTime.Now);
            return "Customer create with Id: " + customer.CustomerId;
        }
        catch (Exception ex)
        {
            LogError(ex);
            return "Customer not created";
        }
    }

    private void LogError(Exception ex)
    {
        _logger.LogError(ex.Message);
    }
}
