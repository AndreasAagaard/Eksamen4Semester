namespace mongo_service.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class MongoItem
{
    public MongoItem(string? name)
    {
        Name = name;
    }
    public MongoItem(){}


    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id {get; set;}
    public string? Name {get; set;}
    public DateTime CreatedAt = DateTime.Now;

}
