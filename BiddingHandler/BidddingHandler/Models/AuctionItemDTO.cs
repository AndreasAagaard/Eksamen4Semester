using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BiddingHandler.Models;

// public class AuctionItemDTO
// {
//     [BsonId]
//     public Guid? AuctionId { get; set; }
//     public Guid? ProductId {get; set;} 
//     public DateTime? AuctionEnds {get; set;} 
//     public List<OfferItemDTO> Offers {get; set;} = new();


//     public double GetHighestOffer() => (Offers.Any()) ? Offers.Last().Offer : 0;
// }

public class AuctionItemDTO
{
    [BsonId]
    public Guid? AuctionId { get; set; }
    public ProductItemDTO Product { get; set; }
    public DateTime? AuctionEnds { get; set; } 
    public int DaysToRun { get; set; }  
    public List<BiddingItemDTO> Offers {get; set; } = new();


    public double GetHighestOffer() => (Offers.Any()) ? Offers.Last().Offer : 0;

    

}
