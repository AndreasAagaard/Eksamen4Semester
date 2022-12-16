using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BiddingHandler.Models;

public class OfferItemDTO
{
    public OfferItemDTO(int offerId, Guid userId, double offer, DateTime timestamp)
    {
        OfferId = offerId;
        UserId = userId;
        Offer = offer;
        Timestamp = timestamp;
    }

    public Int32 OfferId { get; set; }
    public Guid UserId { get; set;}
    public double Offer {get; set;}
    public DateTime Timestamp {get; set;}
}