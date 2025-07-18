namespace RandomQuotesApi.Models;

public abstract class BaseQuote
{
    public int Id { get; set; }
    public string Quote { get; set; }
}