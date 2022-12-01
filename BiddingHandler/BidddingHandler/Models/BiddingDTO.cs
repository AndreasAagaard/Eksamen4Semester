using System;
namespace BiddingHandler.Models;

public class BiddingItemDTO
{
    public Int32 OfferId { get; set; } = 0;
    public Guid? AuctionId { get; set;}
    public Guid? UserId { get; set;}
    public decimal Offer {get; set;}
    public DateTime Timestamp {get; set;}

}