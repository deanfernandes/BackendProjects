using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductReviews.Models;

public enum UserRole
{
    Customer,
    Admin
}

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("username")]
    public string Username { get; set; } = null!;

    [BsonElement("email")]
    public string Email { get; set; } = null!;

    [BsonElement("password")]
    public string Password { get; set; } = null!;

    [BsonElement("role")]
    public UserRole Role { get; set; } = UserRole.Customer;
}
