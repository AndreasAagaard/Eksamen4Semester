namespace item_service_test;

[TestFixture]
public class Tests
{
    
    private CatalogService _catalogService;

    [SetUp]
    public void Setup()
    {
        //Adding serviceprovier with ILogging<T> to itemservice-service-test.csproj
        var serviceProvier = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        //Getting ILoggerFactory
        var factory = serviceProvier.GetService<ILoggerFactory>();
        
        //Creating ILogger<CustomerService> from ILoggerFactory
        var logger = factory.CreateLogger<CatalogService>();

        //Creating ItemServiceService service with newly created ILogger
        // _catalogService = new CatalogService(logger);
    }

    // [Test]
    // public void InsertItem_ReturnTrue()
    // {
    //     ProductItemDTO item = new ProductItemDTO("TestItem");
    //     _catalogService.createItems(item);
    //     Assert.AreEqual(1, _itemService.Items.Count());
    // }

    // [Test]
    // public void ItemId_NowEqual()
    // {
    //     Item item1 = new Item("TestItem 1");
    //     Item item2 = new Item("TestItem 2");
    //     _itemService.createItems(item1);
    //     _itemService.createItems(item2);
    //     Assert.AreNotEqual(_itemService.Items[0].ItemId, _itemService.Items[1].ItemId);
    // }
}