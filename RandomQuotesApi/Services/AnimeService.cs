using MongoDB.Driver;
using RandomQuotesApi.Models;

namespace RandomQuotesApi.Services;

public class AnimeService
{
    private readonly IMongoCollection<AnimeQuote> _animeQuotes;

    public AnimeService(IMongoDatabase database)
    {
        _animeQuotes = database.GetCollection<AnimeQuote>("AnimeQuotes");
    }

    public async Task<List<AnimeQuote>> GetAllAsync()
    {
        return await _animeQuotes.Find(_ => true).ToListAsync();
    }

    public async Task<AnimeQuote?> GetRandomAsync(string? anime = null, string? character = null)
    {
        var builder = Builders<AnimeQuote>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(anime))
            filter &= builder.Eq(q => q.Anime, anime);

        if (!string.IsNullOrEmpty(character))
            filter &= builder.Eq(q => q.Character, character);

        var count = await _animeQuotes.CountDocumentsAsync(filter);
        if (count == 0)
            return null;

        var randomIndex = new Random().Next((int)count);

        var randomQuote = await _animeQuotes.Find(filter)
            .Skip(randomIndex)
            .Limit(1)
            .FirstOrDefaultAsync();

        return randomQuote;
    }

    public async Task<List<string>> GetDistinctAnimesAsync()
    {
        return await _animeQuotes.Distinct<string>("anime", Builders<AnimeQuote>.Filter.Empty).ToListAsync();
    }

    public async Task<List<string>> GetDistinctCharactersAsync()
    {
        return await _animeQuotes.Distinct<string>("character", Builders<AnimeQuote>.Filter.Empty).ToListAsync();
    }

    public async Task SeedQuotesAsync()
    {
        var count = await _animeQuotes.CountDocumentsAsync(FilterDefinition<AnimeQuote>.Empty);
        if (count > 0)
        {
            Console.WriteLine("Already seeded anime quotes.");
            return;
        }     

        var seedQuotes = new List<AnimeQuote>
        {
            // 🔴 Dragon Ball
            new AnimeQuote
            {
                Quote = "Power comes in response to a need, not a desire.",
                Character = "Goku",
                Anime = "Dragon Ball"
            },
            new AnimeQuote
            {
                Quote = "I’m not a hero. I’m a high-functioning lunatic.",
                Character = "Vegeta",
                Anime = "Dragon Ball"
            },

            // 🔴 Dragon Ball Z
            new AnimeQuote
            {
                Quote = "I am the hope of the universe.",
                Character = "Goku",
                Anime = "Dragon Ball Z"
            },
            new AnimeQuote
            {
                Quote = "You may have invaded my mind and my body, but there's one thing a Saiyan always keeps… his pride!",
                Character = "Vegeta",
                Anime = "Dragon Ball Z"
            },
            new AnimeQuote
            {
                Quote = "Even a low-class warrior can surpass an elite, with enough hard work.",
                Character = "Goku",
                Anime = "Dragon Ball Z"
            },

            // 🟡 Pokémon
            new AnimeQuote
            {
                Quote = "I wanna be the very best, like no one ever was.",
                Character = "Ash Ketchum",
                Anime = "Pokémon"
            },
            new AnimeQuote
            {
                Quote = "Pikachu, I choose you!",
                Character = "Ash Ketchum",
                Anime = "Pokémon"
            },
            new AnimeQuote
            {
                Quote = "We do have a lot in common. The same Earth, the same air, the same sky.",
                Character = "Mewtwo",
                Anime = "Pokémon: The First Movie"
            },

            // 🔷 Yu-Gi-Oh!
            new AnimeQuote
            {
                Quote = "It's time to duel!",
                Character = "Yugi Muto",
                Anime = "Yu-Gi-Oh!"
            },
            new AnimeQuote
            {
                Quote = "I will not lose! Not to Kaiba, not to anyone!",
                Character = "Yugi Muto",
                Anime = "Yu-Gi-Oh!"
            },
            new AnimeQuote
            {
                Quote = "You activated my trap card!",
                Character = "Seto Kaiba",
                Anime = "Yu-Gi-Oh!"
            }
        };

        await _animeQuotes.InsertManyAsync(seedQuotes);
    }
}