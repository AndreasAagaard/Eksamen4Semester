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
        var hostName = System.Net.Dns.GetHostName(); 
        var ips = System.Net.Dns.GetHostAddresses(hostName); 
        var _ipaddr = ips.First().MapToIPv4().ToString(); 
        _logger.LogInformation(1, $"Catalog responding from {_ipaddr}"); 
    }

    [HttpGet("GetProduct/{productId}")]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        _logger.LogInformation($"Request for product with guid: {productId}");
        
        ProductItemDTO? result = await _retry.RetryFunction(
            _service.GetProduct(productId)
            );

        if (result == null){
            _logger.LogInformation($"Result of the request was: {result}");
            return NoContent();
        }
        
        _logger.LogDebug($"Result of the request was: {result}");
        
        return Ok(new { result });
    }

    [HttpGet("GetProduct/")]
    public async Task<IActionResult> GetProduct()
    {
        _logger.LogInformation($"Request for all products");
        
        List<ProductItemDTO>? result = await _retry.RetryFunction(
            _service.GetAllProducts()
            );

        if (result == null){
            _logger.LogInformation($"Result of the request was: {result}");
            return NoContent();
        }
        
        _logger.LogDebug($"Result of the request was: {result}");
        
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
