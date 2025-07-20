using RandomQuotesApi.Models;

namespace RandomQuotesApi.Services
{
    public interface IAnimeService
    {
        Task<List<AnimeQuote>> GetAllAsync();
        Task<AnimeQuote?> GetRandomAsync(string? anime = null, string? character = null);
        Task<List<string>> GetDistinctAnimesAsync();
        Task<List<string>> GetDistinctCharactersAsync();
        Task SeedQuotesAsync();
    }
}