using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;

namespace customer_service_test;

[TestFixture]
public class ServiceTests
{
    private ILogger<CustomerService>? _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CustomerService>>().Object;
    }

    [Test]
    public void TestCustomerInserted()
    {
        // Arrange
        Customer customer = NewCustomer();

        // Act
        var service = new CustomerService(_logger);
        service.CreateCustomer(customer);

        // Assert
        Assert.AreEqual(1, service.Customers.Count());
    }

    [Test]
    public void TestCustomerIncrementedId()
    {
        // Arrange
        Customer customer = NewCustomer();
        CustomerService.ResetCounter();

        // Act
        var service1 = new CustomerService(_logger);
        service1.CreateCustomer(customer);
        var customer1Id = service1.Customers.Last().CustomerId;

        var service2 = new CustomerService(_logger);
        service2.CreateCustomer(customer);
        var customer2Id = service2.Customers.Last().CustomerId;

        // Assert
        Assert.AreEqual(1, customer1Id);
        Assert.AreEqual(2, customer2Id);
    }

    // [Test]
    // public async Task TestBookingEndpoint_failure_posting()
    // {
    //     // Arrange
    //     var bookingDTO = CreateBooking(new DateTime(2023,11,22, 14, 22, 32));
    //     var mockRepo = new Mock<IBookingService>();
    //     mockRepo.Setup(svc => svc.AddBooking(bookingDTO)).Returns(Task.FromException(new Exception()));
    //     var controller = new BookingController(_logger, _configuration, mockRepo.Object);

    //     // Act        
    //     var result = await controller.PostBooking(bookingDTO);

    //     // Assert
    //     Assert.IsNull(result);
    // }  

    [Test]
    public void TestCustomerCreated_Failure()
    {
        // Arrange
        Customer? customer = null;

        // Act        
        var service1 = new CustomerService(_logger);
        var result = service1.CreateCustomer(customer);

        // Assert
        Assert.IsNull(result);
    }

    private Customer NewCustomer()
    {
        var customer = new Customer("Name", "Phone number", "Email");
        return customer;
    }
    
}
