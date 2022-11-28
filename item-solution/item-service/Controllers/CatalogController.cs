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

    [HttpGet("GetProduct/{productId}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        _logger.LogInformation($"Request for product with guid: {productId}");
        
        ProductItemDTO? result = await _service.GetProduct(productId);

        return Ok(new { result });
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductItemDTO dto)
    {
        _logger.LogInformation($"Request for product creation");
        
        Guid? result = await _service.CreateProduct(dto);
        
        return Ok(new { result });
    }
}
