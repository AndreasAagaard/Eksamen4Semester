namespace BiddingHandlerTest;

[TestFixture]
public class BiddingControllerTests
{
    private ILogger<BiddingController> _logger;
    private IConfiguration _config;
    
    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<BiddingController>>().Object;

        var myConfiguration = new Dictionary<string, string>
        {
            {"MongoConnectionString", "http://mongodb.admin"},
            {"AuctionBrokerHost", "http://testhost.local"},
            {"Database", "database"},
            {"Collection", "collection"},
            {"CatalogHostName", "servicename"},
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
    }

    [Test]
    public void TestMakeBid_NotConnected()
    {
        // Arrange
        var bid = NewBid();
        
        var controller = new BiddingController(_logger, _config);
        

        // Act
        var res = controller.PostBid(bid);
        
        // Assert
        Assert.IsNull(res);
    }
    private BiddingItemDTO NewBid() {
        return new BiddingItemDTO{
            AuctionId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Offer = 10
        };
    }
}