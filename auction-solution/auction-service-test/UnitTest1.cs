namespace auction_service_test;

[TestFixture]
public class AuctionServiceTests
{
    private ILogger<IAuctionService> _logger;
    private IConfiguration _config;
    private IMongoDatabase _database;
    private IMongoCollection<AuctionItemDTO> _collection;
    private IRetryService _retry;
    
    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<IAuctionService>>().Object;

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
    public async Task TestPostAuction_DTO_Valid()
    {
        // Arrange
        var product = NewProduct();
        var auction = NewAuction(product);
        var mockDbContext = new Mock<IMongoDBContext>();
        mockDbContext.SetupProperty(x => x.Collection);
        var mockCollection = new Mock<IMongoCollection<AuctionItemDTO>>(mockDbContext.Collection);

        var task = mockDbContext.Object.Collection.InsertOneAsync(auction);
        var mockRetry = new Mock<IRetryService>();
        mockRetry.Setup(svc => svc.VoidRetryFunction(task));

        var service = new AuctionService(_logger, mockDbContext.Object, mockRetry.Object, _config);
        
        
        // Act
        await service.CreateAuction(auction);

        // Assert
        Assert.IsInstanceOf(typeof(AuctionItemDTO), auction);
        Assert.AreEqual(auction.AuctionId, auction.AuctionId);
        Assert.AreEqual(auction.AuctionEnds, auction.AuctionEnds);
    }

    private ProductItemDTO NewProduct() {
        return new ProductItemDTO {
            ProductId = Guid.NewGuid(),
            ProductCategory = ProductCategory.Electronics,
            Title = "Hey",
            Description = "Description",
            ShowRoomId = 1,
            Valuation = 10,
            AuktionStart = DateTime.Now
        };
    }
    private AuctionItemDTO NewAuction(ProductItemDTO product) {
        int daystorun = 3;
        return new AuctionItemDTO{
            AuctionId = Guid.NewGuid(),
            Product = product,
            AuctionEnds = product.AuktionStart.AddDays(daystorun),
            DaysToRun = daystorun,
            Offers = new()
        };
    }
}
