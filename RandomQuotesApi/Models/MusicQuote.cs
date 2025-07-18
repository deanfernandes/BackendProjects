namespace RandomQuotesApi.Models;

public class MusicQuote : BaseQuote
{
    public string Song { get; set; }
    public string Album { get; set; }
    public string Artist { get; set; }
}