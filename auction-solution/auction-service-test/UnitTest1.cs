namespace auction_service_test;

[TestFixture]
public class ServiceTests
{
    private ILogger<AuctionController>? _logger;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<AuctionController>>().Object;
    }

    [Test]
    public async Task GetAuctionOk()
    {
        // Arrange
        //Guid auctionId = Guid.Parse("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
        Guid auctionId = Guid.Parse("0b6f675e-7a1a-49d8-b535-352ceaa3fd6a");
        var mockService = new Mock<IAuctionService>();
        mockService.Setup(x => x.GetAuction(auctionId))
            .Returns(Task.FromResult<AuctionItemDTO?>(Auction()));

        var mockRetry = new Mock<IRetryService>();

        var controller = new AuctionController(_logger, mockService.Object, mockRetry.Object);

        // Act
        var actionResult = await controller.GetAuction(auctionId);

        // Assert
        Assert.IsInstanceOf(typeof(IActionResult), actionResult);
        Assert.IsInstanceOf(typeof(IActionResult), actionResult);

        // Assert.IsNotNull(contentResult);
        // Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
        // Assert.IsNotNull(contentResult.Content);
        // Assert.AreEqual(auctionId, contentResult.Content.Id);
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

    // [Test]
    // public void TestCustomerCreated_Failure()
    // {
    //     // Arrange
    //     Customer? customer = null;

    //     // Act        
    //     var service1 = new CustomerService(_logger);
    //     var result = service1.CreateCustomer(customer);

    //     // Assert
    //     Assert.IsNull(result);
    // }

    // private Customer NewCustomer()
    // {
    //     var customer = new Customer("Name", "Phone number", "Email");
    //     return customer;
    // }

//     private AuctionItemDTO Auction() {
//         return new AuctionItemDTO{ 
//             AuctionId = Guid.Parse("0b6f675e-7a1a-49d8-b535-352ceaa3fd6a"),
//             ProductId = Guid.Parse("0b6f675e-7a1a-49d8-b535-352ceaa3fd6a"),
//             AuctionEnds = DateTime.Now,
//             Offers = new(),
//         };
//     }
    
// }





    // //[HttpPost]
    // public async Task<IActionResult> CreateAuction(AuctionItemDTO dto)
    // {
    //     _logger.LogInformation($"Request for auction creation");
        
    //     Guid? result = await _retry.RetryFunction(
    //         _service.CreateAuction(dto)
    //         );
        
    //     if (result == null)
    //         return BadRequest();
        
    //     return Ok(new { result });
    // }