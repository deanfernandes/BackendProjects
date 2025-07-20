using RandomQuotesApi.Models;

namespace RandomQuotesApi.Services
{
    public interface ISongService
    {
        public Task<List<SongQuote>> GetAllAsync();
        public Task<SongQuote?> GetRandomAsync(string? artist = null, string? album = null, string? song = null);
        public Task<List<string>> GetDistinctArtistsAsync();
        public Task<List<string>> GetDistinctAlbumsAsync();
        public Task<List<string>> GetDistinctSongsAsync();
        public Task SeedQuotesAsync();
    }
}