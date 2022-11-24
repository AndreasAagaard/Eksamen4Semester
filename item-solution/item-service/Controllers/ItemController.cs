using Microsoft.AspNetCore.Mvc;
using item_service.Service;
using item_service.Models;

namespace item_service.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{


    private readonly ILogger<ItemController> _logger;
    private readonly ItemServiceService _service;

    public ItemController(ILogger<ItemController> logger, ItemServiceService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public List<Item> Get()
    {
        return _service.GetItem();
    }

    [HttpPost("createItem", Name = "GetItem")]
    public string Post(Item item)
    {
        return _service.createItems(item);
    }
}
