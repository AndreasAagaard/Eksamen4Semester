using System;
namespace auction_service.Models;

public class OfferItemDTO
{
    [BsonId]
    public Guid? OfferId { get; set; }

    public Guid? UserId { get; set;}

    public double Offer {get; set;}

    public DateTime Timestamp {get; set;}

}