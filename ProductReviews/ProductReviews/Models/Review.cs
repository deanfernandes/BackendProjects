using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductReviews.Models;

public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;

    [BsonElement("productId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; } = null!;

    [BsonElement("rating")]
    public int Rating { get; set; } // out of 5

    [BsonElement("comment")]
    public string Comment { get; set; } = null!;
}
