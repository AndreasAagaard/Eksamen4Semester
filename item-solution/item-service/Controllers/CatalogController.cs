using Microsoft.AspNetCore.Mvc;
using item_service.Service;
using item_service.Models;

namespace item_service.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{


    private readonly ILogger<CatalogController> _logger;
    private readonly CatalogService _service;

    public CatalogController(ILogger<CatalogController> logger, CatalogService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("GetProduct/{string:id}")]
    public IActionResult GetProduct(Guid id)
    {
        return Ok(_service.GetProduct(id));
    }

    // [HttpPost]
    // public IActionResult CreateProduct(ProductItemDTO product)
    // {
    //     return Ok(_service.CreateProduct(product));
    // }
}
