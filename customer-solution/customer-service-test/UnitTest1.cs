namespace customer_service_test;

[TestFixture]
public class Tests
{

    private CustomerService _customerService;

    [SetUp]
    public void SetUp()
    {
        //Adding serviceprovier with ILogging<T> to customer-service-test.csproj
        var serviceProvier = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        //Getting ILoggerFactory
        var factory = serviceProvier.GetService<ILoggerFactory>();
        
        //Creating ILogger<CustomerService> from ILoggerFactory
        var logger = factory.CreateLogger<CustomerService>();

        //Creating CustomerService service with newly created ILogger
        _customerService = new CustomerService(logger);
    }

    [Test]
    public void InsertCustomer()
    {
        //Creating and passing customer object through API
        //CustomerService --- customerservice/createcustomer POST
        Customer customer = new Customer("Nicolai", "27118875", "jegerrockstar@something.com");
        _customerService.CreateCustomer(customer);

        //Testing if customer is added to customers list
        Assert.AreEqual(1, _customerService.Customers.Count());
    }

    [Test]
    public void NewCustomer_ReturnNewCustomerId()
    {
        //Creating and passing customer object through API
        //CustomerService --- customerservice/createcustomer POST
        Customer customer = new Customer("Nicolai", "27118875", "jegerrockstar@something.com");
        _customerService.CreateCustomer(customer);
        customer = _customerService.Customers.Last();

        //Testing if Id is added to customer object
        Assert.AreEqual(1, customer.CustomerId);
    }

    [Test]
    public void NewCustomer_ReturnNextIncrementedId()
    {
        //Creating and passing customer object through API
        //CustomerService --- customerservice/createcustomer POST
        Customer customer = new Customer("Nicolai", "27118875", "jegerrockstar@something.com");
        _customerService.CreateCustomer(customer);
        customer = _customerService.Customers.Last();

        //Testing if next Id is 1 higher than current last customer object
        Assert.AreEqual(_customerService.Id, customer.CustomerId + 1);
    }
}