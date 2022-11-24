using item_service.Models;
namespace item_service.Service;

public class ItemServiceService
{
    private int id = 1;
    public List<Item> Items = new List<Item>();

    private ILogger<ItemServiceService> _logger;

    public ItemServiceService(ILogger<ItemServiceService> logger)
    {
        _logger = logger;
        SeedData();
    }

    public List<Item> GetItem()
    {
        return Items;
    }

    public string createItems(Item item)
    {
        try
        {
            item.ItemId = id++;
            Items.Add(item);
            _logger.LogInformation("New item with Id: " + item.ItemId +
                                        " created at timestamp " + DateTime.Now);
            return $"{item.Name} added succesfully";
        }
        catch (Exception ex)
        {
            LogError(ex);
            return "Item not created";
        }
    }

    private void LogError(Exception ex) 
    {
        _logger.LogError(ex.Message);
    }

    private void SeedData() 
    {
        createItems(new Item("Pizza"));
        createItems(new Item("Mel"));
        createItems(new Item("Ikke ananas"));
        createItems(new Item("Tand"));
    }
}