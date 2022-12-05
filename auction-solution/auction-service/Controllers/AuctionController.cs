using Microsoft.AspNetCore.Mvc;
using auction_service.Service;
using auction_service.Models;

namespace auction_service.Controllers;

[ApiController]
[Route("[controller]")]
public class AuctionController : ControllerBase
{
    private readonly ILogger<AuctionController> _logger;
    private readonly auction_service _service;
    private readonly RetryService _retry;

    public CatalogController(ILogger<AuctionController> logger, AuctionController service,
        RetryService retry)
    {
        _logger = logger;
        _service = service;
        _retry = retry;
    }

    [HttpGet("GetAuction/{auctionId}")]
    public async Task<IActionResult> GetProduct(Guid auctionId)
    {
        _logger.LogInformation($"Request for product with guid: {auctionId}");
        
        AuctionItemDTO? result = await _retry.RetryFunction(
            _service.GetAuction(auctionId)
            );

        if (result == null)
            return NoContent();
        
        return Ok(new { result });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuction(AuctionItemDTO dto)
    {
        _logger.LogInformation($"Request for auction creation");
        
        Guid? result = await _retry.RetryFunction(
            _service.CreateAuction(dto)
            );
        
        if (result == null)
            return BadRequest();
        
        return Ok(new { result });
    }
}
