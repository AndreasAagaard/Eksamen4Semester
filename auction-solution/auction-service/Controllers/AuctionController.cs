using Microsoft.AspNetCore.Mvc;
using auction_service.Service;
using auction_service.Models;

namespace auction_service.Controllers;

[ApiController]
[Route("[controller]")]
public class AuctionController : ControllerBase
{
    private readonly ILogger<AuctionController> _logger;
    private readonly IAuctionService _service;
    private readonly IRetryService _retry;

    public AuctionController(ILogger<AuctionController> logger, IAuctionService service, IRetryService retry)
    {
        _logger = logger;
        _service = service;
        _retry = retry;
        var hostName = System.Net.Dns.GetHostName(); 
        var ips = System.Net.Dns.GetHostAddresses(hostName); 
        var _ipaddr = ips.First().MapToIPv4().ToString(); 
        
        _logger.LogInformation(1, $"AuctionController responding from {_ipaddr}"); 
    }

    [HttpGet("GetAuction/{auctionId}")]
    public async Task<IActionResult> GetAuction(Guid auctionId)
    {
        _logger.LogInformation($"Request for product with guid: {auctionId}");
        
        AuctionItemDTO? result = await _retry.RetryFunction(
            _service.GetAuction(auctionId)
            );

        if (result == null)
            return NoContent();
        
        return Ok(result);
    }

    [HttpGet("GetAuction")]
    public async Task<IActionResult> GetAllProducts()
    {
        _logger.LogInformation($"Request for all auctions");
        
        List<AuctionItemDTO>? result = await _retry.RetryFunction(
            _service.GetAllAuctions()
            );

        if (result == null)
            return NoContent();
        
        return Ok(result);
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
        
        return Ok(result);
    }
}
