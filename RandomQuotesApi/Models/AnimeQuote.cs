using MongoDB.Bson.Serialization.Attributes;

namespace RandomQuotesApi.Models;

public class AnimeQuote : BaseQuote
{
    [BsonElement("anime")]
    public string Anime { get; set; }
    [BsonElement("character")]
    public string Character { get; set; }
}

public class AnimeQuoteDto
{
    public string Quote { get; set; } = null!;
    public string Anime { get; set; } = null!;
    public string Character { get; set; } = null!;
}

public static class AnimeQuoteExtensions
{
    public static AnimeQuoteDto ToDto(this AnimeQuote quote)
    {
        return new AnimeQuoteDto
        {
            Quote = quote.Quote,
            Anime = quote.Anime,
            Character = quote.Character
        };
    }
}