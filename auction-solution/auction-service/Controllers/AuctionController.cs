using Microsoft.AspNetCore.Mvc;
using auction_service.Service;
using auction_service.Models;

namespace auction_service.Controllers;

[ApiController]
[Route("[controller]")]
public class AuctionController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;
    private readonly auction_service _service;
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
            return NoContent();
        
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
