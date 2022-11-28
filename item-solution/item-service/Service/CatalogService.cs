using item_service.Models;
namespace item_service.Service;

public class CatalogService
{
    private int id = 1;
    public List<ProductItemDTO> Catalog = new List<ProductItemDTO>();

    private ILogger<CatalogService> _logger;

    public CatalogService(ILogger<CatalogService> logger)
    {
        _logger = logger;
    }

    public ProductItemDTO GetProduct(Guid id)
    {
        return Catalog.Find(x => x.ProductId == id);
    }

    // public string CreateProduct(ProductItemDTO product)
    // {
        // try
        // {
        //     item.ItemId = id++;
        //     Items.Add(item);
        //     _logger.LogInformation("New item with Id: " + item.ItemId +
        //                                 " created at timestamp " + DateTime.Now);
        //     return $"{item.Name} added succesfully";
        // }
        // catch (Exception ex)
        // {
        //     LogError(ex);
        //     return "Item not created";
        // }
    // }

    private void LogError(Exception ex) 
    {
        _logger.LogError(ex.Message);
    }
}