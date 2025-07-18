using Microsoft.AspNetCore.Mvc;
using RandomQuotesApi.Models;
using System.Collections.Generic;

namespace RandomQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicController : ControllerBase
{
    private static List<MusicQuote> _musicQuotes = new List<MusicQuote>
    {
        new MusicQuote
        {
            Id = 1,
            Quote = "And in the end, the love you take is equal to the love you make.",
            Song = "The End",
            Album = "Abbey Road",
            Artist = "The Beatles"
        },
        new MusicQuote
        {
            Id = 2,
            Quote = "We don't need no education.",
            Song = "Another Brick in the Wall, Pt. 2",
            Album = "The Wall",
            Artist = "Pink Floyd"
        },
        new MusicQuote
        {
            Id = 3,
            Quote = "Cause the players gonna play, play, play, play, play.",
            Song = "Shake It Off",
            Album = "1989",
            Artist = "Taylor Swift"
        },
        new MusicQuote
        {
            Id = 4,
            Quote = "Is this the real life? Is this just fantasy?",
            Song = "Bohemian Rhapsody",
            Album = "A Night at the Opera",
            Artist = "Queen"
        },
        new MusicQuote
        {
            Id = 5,
            Quote = "Lose yourself in the music, the moment, you own it.",
            Song = "Lose Yourself",
            Album = "8 Mile: Music from and Inspired by the Motion Picture",
            Artist = "Eminem"
        },
        new MusicQuote
        {
            Id = 6,
            Quote = "Hello from the other side.",
            Song = "Hello",
            Album = "25",
            Artist = "Adele"
        }
    };

    [HttpGet]
    public ActionResult<MusicQuote> GetQuote([FromQuery] string? artist = null, [FromQuery] string? album = null, [FromQuery] string? song = null)
    {
        var filteredQuotes = _musicQuotes.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(artist))
        {
            filteredQuotes = filteredQuotes.Where(q => q.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase));
        }
        if (!string.IsNullOrWhiteSpace(album))
        {
            filteredQuotes = filteredQuotes.Where(q => q.Album.Equals(album, StringComparison.OrdinalIgnoreCase));
        }
        if (!string.IsNullOrWhiteSpace(song))
        {
            filteredQuotes = filteredQuotes.Where(q => q.Song.Equals(song, StringComparison.OrdinalIgnoreCase));
        }
        var quotesList = filteredQuotes.ToList();

        if (!quotesList.Any())
        {
            return NotFound("No matching quotes found.");
        }

        var random = new Random();
        var randomQuote = quotesList[random.Next(quotesList.Count)];

        return Ok(randomQuote);
    }

    [HttpGet("all")]
    public ActionResult<List<MusicQuote>> GetAll()
    {
        return Ok(_musicQuotes);
    }

    [HttpGet("artists")]
    public ActionResult<List<string>> GetArtists()
    {
        var distinctArtists = _musicQuotes
            .Select(q => q.Artist)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(name => name)
            .ToList();

        return Ok(distinctArtists);
    }

    [HttpGet("albums")]
    public ActionResult<List<string>> GetAlbums()
    {
        var distinctAlbums = _musicQuotes
            .Select(q => q.Album)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(name => name)
            .ToList();

        return Ok(distinctAlbums);
    }

    [HttpGet("songs")]
    public ActionResult<List<string>> GetSongs()
    {
        var distinctSongs = _musicQuotes
            .Select(q => q.Song)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(name => name)
            .ToList();

        return Ok(distinctSongs);
    }
}