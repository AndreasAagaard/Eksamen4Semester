namespace item_service.Models;

public class Item
{
    public Item(string? name)
    {
        CreatedAt = DateTime.Now;
        Name = name;
    }

    public int ItemId {get; set;}
    public DateTime CreatedAt { get; set; }
    public string? Name {get; set;}

}
