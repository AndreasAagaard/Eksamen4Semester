using System;
namespace auction_service.Models;

public class BiddingItemDTO
{
    public int OfferId { get; set; }
    public Guid AuctionId { get; set;}
    public Guid UserId { get; set;}
    public double Offer {get; set;}
    public DateTime Timestamp {get; set;}

}