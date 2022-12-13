namespace item_service.Models;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public enum ProductCategory
{
    None = 0,
    Electronics = 1,
    Cars = 2,
    Hobby = 3,
    Furniture = 4,
    Jewlery = 5,
    SportAndLeisure = 6,
    Textiles = 7,
    Watches = 8,
    Wine = 9
}
public class ProductItemDTO
{
    [BsonId]
    public Guid? ProductId { get; set; }
    public ProductCategory ProductCategory { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int ShowRoomId { get; set; }
    public double Valuation { get; set; }
    // Status bool
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime AuktionStart { get; set; }
    public List<Uri> Images { get; set; } = new ();
}
