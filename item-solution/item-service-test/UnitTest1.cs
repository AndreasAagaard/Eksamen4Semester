namespace item_service_test;

public class Tests
{
    
    private ItemServiceService _service;

    [SetUp]
    public void Setup()
    {
        _service = new ItemServiceService();
    }

    [Test]
    public void InsertItem_ReturnTrue()
    {
        Item item = new Item("TestItem");
        _service.createItems(item);
        Assert.AreEqual(1, _service.Items.Count());
        
    }

    [Test]
    public void ItemId_NowEqual()
    {
        Item item1 = new Item("TestItem 1");
        Item item2 = new Item("TestItem 2");
        _service.createItems(item1);
        _service.createItems(item2);
        Assert.AreNotEqual(_service.Items[0].ItemId, _service.Items[1].ItemId);
    }
}