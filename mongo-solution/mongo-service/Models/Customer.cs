namespace mongo_service.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class MongoCustomer
{

    public MongoCustomer(string name, string mobile, string email)
    {
        Name = name;
        Mobile = mobile;
        Email = email;
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id;
    public string Name { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate = DateTime.Now;
}
