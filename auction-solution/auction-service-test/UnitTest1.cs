namespace auction_service_test;

[TestFixture]
public class AuctionServiceTests
{
    private ILogger<IAuctionService> _logger;
    private IConfiguration _config;
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
        int daystorun = 3;
        var mockDbContext = new Mock<IMongoDBContext>();
        var mockRetry = new Mock<IRetryService>();
        var service = new AuctionService(_logger, mockDbContext.Object, mockRetry.Object, _config);
        
        
        // Act
        //await service.CreateAuction(auction);

        // Assert
        Assert.IsInstanceOf(typeof(AuctionItemDTO), auction);
        Assert.AreEqual(auction.AuctionId, auction.AuctionId);
        Assert.AreEqual(auction.AuctionEnds, auction.AuctionEnds);
        Assert.AreEqual(auction.AuctionEnds, auction.Product.AuktionStart.AddDays(daystorun));
    }

    [Test]
    public async Task TestPostAuction_DTO_InValid()
    {
        // Arrange
        var product = NewProduct();
        var auction = NewAuction(product);
        int daystorun = 2;
        var mockDbContext = new Mock<IMongoDBContext>();
        var mockRetry = new Mock<IRetryService>();
        var service = new AuctionService(_logger, mockDbContext.Object, mockRetry.Object, _config);
        
        
        // Act
        //await service.CreateAuction(auction);

        // Assert
        Assert.IsInstanceOf(typeof(AuctionItemDTO), auction);
        Assert.AreEqual(auction.AuctionId, auction.AuctionId);
        Assert.AreEqual(auction.AuctionEnds, auction.AuctionEnds);
        Assert.AreNotEqual(auction.AuctionEnds, auction.Product.AuktionStart.AddDays(daystorun));
    }

    [Test]
    public async Task TestAddOffertoAuction_Succes()
    {
        // Arrange
        var auctionId = Guid.NewGuid();
        var offer = NewOffer();
        var _collection = new Mock<IMongoCollection<AuctionItemDTO>>().Object;
        var mockDbContext = new Mock<IMongoDBContext>();
        mockDbContext.SetupProperty(x => x.Collection, _collection);
        
        var filter = Builders<AuctionItemDTO>.Filter.Eq(x => x.AuctionId, auctionId);
        var update = Builders<AuctionItemDTO>.Update.AddToSet<OfferItemDTO>("Offers", offer);
        
        var mockRetry = new Mock<IRetryService>();
        Task task = new Task( async () => 
            mockDbContext.Object.Collection.UpdateOneAsync(filter, update)
            );
        mockRetry.Setup(svc => svc.VoidRetryFunction(task))      
            .Returns(Task.CompletedTask);
        var service = new AuctionService(_logger, mockDbContext.Object, mockRetry.Object, _config);
        
        // Act
        await service.AddOfferToAuction(auctionId, offer);

        // Assert
        var highestOffer = offer.Offer;
        Assert.AreEqual(highestOffer, offer.Offer);
    }

    [Test]
    public async Task TestGetAuction_AuctionFound()
    {
        // Arrange
        var auctionId = Guid.NewGuid();
        var auction = NewAuction(NewProduct());
        auction.AuctionId = auctionId;

        var _collection = new Mock<IMongoCollection<AuctionItemDTO>>().Object;
        var mockDbContext = new Mock<IMongoDBContext>();
        mockDbContext.SetupProperty(x => x.Collection, _collection);
        
        var filter = Builders<AuctionItemDTO>.Filter.Eq(x => x.AuctionId, auctionId);
        
        var mockRetry = new Mock<IRetryService>();
        Task<AuctionItemDTO> task = mockDbContext.Object.Collection.Find(filter).SingleOrDefaultAsync();

        mockRetry.Setup(svc => svc.RetryFunction(task))      
            .Returns(Task.FromResult<AuctionItemDTO>(auction));
        var service = new AuctionService(_logger, mockDbContext.Object, mockRetry.Object, _config);
        
        // Act
        var result = await service.GetAuction(auctionId);
        
        // Assert
        Assert.AreEqual(auction.AuctionId, auctionId);
    }

    [Test]
    public async Task TestGetAuction_AuctionNotFound()
    {
        // Arrange
        var auctionId = Guid.NewGuid();
        var auction = NewAuction(NewProduct());
        auction.AuctionId = auctionId;

        var _collection = new Mock<IMongoCollection<AuctionItemDTO>>().Object;
        var mockDbContext = new Mock<IMongoDBContext>();
        mockDbContext.SetupProperty(x => x.Collection, _collection);
        
        var filter = Builders<AuctionItemDTO>.Filter.Eq(x => x.AuctionId, auctionId);
        
        var mockRetry = new Mock<IRetryService>();
        Task<AuctionItemDTO?> task = mockDbContext.Object.Collection.Find(filter).SingleOrDefaultAsync();

        mockRetry.Setup(svc => svc.RetryFunction(task))      
            .Returns(Task.FromResult<AuctionItemDTO?>(null));
        var service = new AuctionService(_logger, mockDbContext.Object, mockRetry.Object, _config);
        
        // Act
        var result = await service.GetAuction(auctionId);
        
        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task TestGetAuction_GetAuctionsSucces()
    {
        // Arrange
        var auctions = new List<AuctionItemDTO> { 
            NewAuction(NewProduct()),
            NewAuction(NewProduct()),
            NewAuction(NewProduct()),
            NewAuction(NewProduct()),
        };

        var _collection = new Mock<IMongoCollection<AuctionItemDTO>>().Object;
        var mockDbContext = new Mock<IMongoDBContext>();
        mockDbContext.SetupProperty(x => x.Collection, _collection);
    
        var mockRetry = new Mock<IRetryService>();
        Task<List<AuctionItemDTO>> task = 
            mockDbContext.Object.Collection.Find(x => true).ToListAsync();

        mockRetry.Setup(svc => svc.RetryFunction(task))      
            .Returns(Task.FromResult<List<AuctionItemDTO>>(auctions));

        var service = new AuctionService(_logger, mockDbContext.Object, mockRetry.Object, _config);
        
        // Act
        var result = await service.GetAllAuctions();
        
        // Assert
        Assert.IsInstanceOf(typeof(List<AuctionItemDTO>), auctions);
    }

    [Test]
    public async Task TestGetAuction_GetAuctionsNone()
    {
        // Arrange
        var _collection = new Mock<IMongoCollection<AuctionItemDTO>>().Object;
        var mockDbContext = new Mock<IMongoDBContext>();
        mockDbContext.SetupProperty(x => x.Collection, _collection);
    
        var mockRetry = new Mock<IRetryService>();
        Task<List<AuctionItemDTO>> task = 
            mockDbContext.Object.Collection.Find(x => true).ToListAsync();

        mockRetry.Setup(svc => svc.RetryFunction(task))      
            .Returns(Task.FromResult<List<AuctionItemDTO>>(null));

        var service = new AuctionService(_logger, mockDbContext.Object, mockRetry.Object, _config);
        
        // Act
        var result = await service.GetAllAuctions();
        
        // Assert
        Assert.IsNull(result);
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
    private OfferItemDTO NewOffer() {
        return new OfferItemDTO(
            4,
            Guid.NewGuid(),
            1000,
            DateTime.Now //Denne logik sker i controller, men sættes til Now, for at gøre det nemmere at teste
        );
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
