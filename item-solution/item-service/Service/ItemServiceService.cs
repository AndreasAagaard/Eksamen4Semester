using item_service.Models;
namespace item_service.Service;

public class ItemServiceService
{
    private int id = 1;
    public List<Item> Items = new List<Item>();

    // private ILogger<ItemServiceService> _logger;

    // ItemServiceService(ILogger<ItemServiceService> logger)
    // {
    //     _logger = logger;
    // }

    public string createItems(Item item)
    {
        try
        {
            item.ItemId = id++;
            Items.Add(item);
            // _logger.LogInformation("{item.Name} added succesfully");
            return $"{item.Name} added succesfully";
        }
        catch (Exception e)
        {
            return $"{e}";
        }
    }
}