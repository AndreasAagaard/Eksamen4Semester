using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace auction_service.Models;

public class AuctionItemDTO
{
    [BsonId]
    public Guid? AuctionId { get; set; }
    public Guid? ProductId {get; set;} 
    public DateTime? AuctionEnds {get; set;} 
    public List<OfferItemDTO> Offers {get; set;} = new();


    public double GetHighestOffer() => (Offers.Any()) ? Offers.Last().Offer : 0;
}