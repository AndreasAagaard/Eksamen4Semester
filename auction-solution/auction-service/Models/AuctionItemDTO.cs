namespace auction_service.Models;

public class AuctionItemDTO
{
    [BsonId]
    public Guid? AuctionId { get; set; }
    public ProductItemDTO Product {get; set;} 
    public List<OfferItemDTO> Offers {get; set;} 
}