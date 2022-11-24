namespace customer_service.Services;
using customer_service.Models;

public class CustomerService
{
    public static int Id = 1;
    public List<Customer> Customers = new();
    private readonly ILogger<CustomerService> _logger;
    public CustomerService(ILogger<CustomerService> logger)
    {
        _logger = logger;
        SeedData();
    }

    public List<Customer> GetCustomers()
    {
        return Customers;
    }

    public Customer? CreateCustomer(Customer customer)
    {
        try
        {
            customer.CustomerId = Id++;
            Customers.Add(customer);
            _logger.LogInformation("New customer with Id: " + customer.CustomerId +
                                        " created at timestamp " + DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError(ex);
            return null;
        }

        return customer;
    }

    private void LogError(Exception ex)
    {
        _logger.LogError(ex.Message);
    }

    public static void ResetCounter() 
    {
        Id = 1;
    }
    private void SeedData() 
    {
        CreateCustomer(new Customer("nicolai", "12345678", "nicolai@gmail.com"));
        CreateCustomer(new Customer("andreas", "34567812", "andreas@gmail.com"));
        CreateCustomer(new Customer("line", "56781234", "line@gmail.com"));
        CreateCustomer(new Customer("sissel", "78123456", "sissel@gmail.com"));
    }
}
