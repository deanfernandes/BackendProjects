using MongoDB.Bson.Serialization.Attributes;

namespace RandomQuotesApi.Models;

public abstract class BaseQuote
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("quoteText")]
    public string Quote { get; set; }
}