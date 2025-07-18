using MongoDB.Bson.Serialization.Attributes;

namespace RandomQuotesApi.Models;

public class SongQuote : BaseQuote
{
    [BsonElement("song")]
    public string Song { get; set; }

    [BsonElement("album")]
    [BsonIgnoreIfNull]
    public string? Album { get; set; }

    [BsonElement("artist")]
    public string Artist { get; set; }
}

public class SongQuoteDto
{
    public string Quote { get; set; }
    public string Song { get; set; }

    public string? Album { get; set; }

    public string Artist { get; set; }
}

public static class SongQuoteExtensions
{
    public static SongQuoteDto ToDto(this SongQuote quote)
    {
        return new SongQuoteDto
        {
            Quote = quote.Quote,
            Song = quote.Song,
            Album = quote.Album,
            Artist = quote.Artist
        };
    }
}