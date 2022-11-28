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
    private readonly RetryService _retry;

    public CatalogController(ILogger<CatalogController> logger, CatalogService service,
        RetryService retry)
    {
        _logger = logger;
        _service = service;
        _retry = retry;
    }

    [HttpGet("GetProduct/{productId}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        _logger.LogInformation($"Request for product with guid: {productId}");
        
        ProductItemDTO? result = await _retry.RetryFunction(
            _service.GetProduct(productId)
            );

        if (result == null)
            return StatusCode(204);
        
        return Ok(new { result });
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductItemDTO dto)
    {
        _logger.LogInformation($"Request for product creation");
        
        Guid? result = await _retry.RetryFunction(
            _service.CreateProduct(dto)
            );
        
        if (result == null)
            return BadRequest();
        
        return Ok(new { result });
    }
}
