using MongoDB.Driver;
using RandomQuotesApi.Models;

namespace RandomQuotesApi.Services;

public class SongService : ISongService
{
    private readonly IMongoCollection<SongQuote> _songQuotes;

    public SongService(IMongoDatabase database)
    {
        _songQuotes = database.GetCollection<SongQuote>("SongQuotes");
    }

    public async Task<List<SongQuote>> GetAllAsync()
    {
        return await _songQuotes.Find(_ => true).ToListAsync();
    }

    public async Task<SongQuote?> GetRandomAsync(string? artist = null, string? album = null, string? song = null)
    {
        var builder = Builders<SongQuote>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(artist))
            filter &= builder.Eq(q => q.Artist, artist);

        if (!string.IsNullOrEmpty(album))
            filter &= builder.Eq(q => q.Album, album);

        if (!string.IsNullOrEmpty(song))
            filter &= builder.Eq(q => q.Song, song);

        var count = await _songQuotes.CountDocumentsAsync(filter);
        if (count == 0)
            return null;

        var randomIndex = new Random().Next((int)count);

        var randomQuote = await _songQuotes.Find(filter)
            .Skip(randomIndex)
            .Limit(1)
            .FirstOrDefaultAsync();

        return randomQuote;
    }

    public async Task<List<string>> GetDistinctArtistsAsync()
    {
        return await _songQuotes.Distinct<string>("artist", Builders<SongQuote>.Filter.Empty).ToListAsync();
    }

    public async Task<List<string>> GetDistinctAlbumsAsync()
    {
        return await _songQuotes.Distinct<string>("album", Builders<SongQuote>.Filter.Empty).ToListAsync();
    }

    public async Task<List<string>> GetDistinctSongsAsync()
    {
        return await _songQuotes.Distinct<string>("song", Builders<SongQuote>.Filter.Empty).ToListAsync();
    }

    public async Task SeedQuotesAsync()
    {
        var count = await _songQuotes.CountDocumentsAsync(FilterDefinition<SongQuote>.Empty);
        if (count > 0)
        {
            Console.WriteLine("Already seeded song quotes.");
            return;
        }

        var seedQuotes = new List<SongQuote>
        {
            // 🎤 Eminem
            new SongQuote
            {
                Quote = "You only get one shot, do not miss your chance to blow.",
                Artist = "Eminem",
                Song = "Lose Yourself",
                Album = "8 Mile"
            },
            new SongQuote
            {
                Quote = "I am whatever you say I am. If I wasn't, then why would I say I am?",
                Artist = "Eminem",
                Song = "The Way I Am"
            },

            // 🎵 Kanye West
            new SongQuote
            {
                Quote = "We wasn't supposed to make it past 25, jokes on you we still alive.",
                Artist = "Kanye West",
                Song = "We Don’t Care",
                Album = "The College Dropout"
            },
            new SongQuote
            {
                Quote = "No one man should have all that power.",
                Artist = "Kanye West",
                Song = "Power"
            },

            // 🕊 Tupac
            new SongQuote
            {
                Quote = "Reality is wrong. Dreams are for real.",
                Artist = "Tupac",
                Song = "Keep Ya Head Up"
            },
            new SongQuote
            {
                Quote = "I'm not saying I'm gonna change the world, but I guarantee that I will spark the brain that will.",
                Artist = "Tupac",
                Song = "Changes",
                Album = "Greatest Hits"
            },

            // 🖤 Biggie Smalls
            new SongQuote
            {
                Quote = "It was all a dream, I used to read Word Up! magazine.",
                Artist = "Biggie Smalls",
                Song = "Juicy",
                Album = "Ready to Die"
            },
            new SongQuote
            {
                Quote = "Stay far from timid, only make moves when your heart's in it.",
                Artist = "Biggie Smalls",
                Song = "Sky's the Limit"
            },

            // 🔥 Drake
            new SongQuote
            {
                Quote = "Started from the bottom now we here.",
                Artist = "Drake",
                Song = "Started From the Bottom",
                Album = "Nothing Was the Same"
            },
            new SongQuote
            {
                Quote = "You only live once — that’s the motto, YOLO.",
                Artist = "Drake",
                Song = "The Motto"
            }
        };

        await _songQuotes.InsertManyAsync(seedQuotes);
    }
}