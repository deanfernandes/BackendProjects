using Microsoft.AspNetCore.Mvc;
using RandomQuotesApi.Models;
using System.Collections.Generic;

namespace RandomQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimeController : ControllerBase
{
    private static List<AnimeQuote> _animeQuotes = new List<AnimeQuote>
    {
        // 🔶 Dragon Ball
        new AnimeQuote
        {
            Id = 1,
            Quote = "I am the hope of the universe.",
            Character = "Goku",
            Anime = "Dragon Ball Z"
        },
        new AnimeQuote
        {
            Id = 2,
            Quote = "Power comes in response to a need, not a desire.",
            Character = "Goku",
            Anime = "Dragon Ball Z"
        },
        new AnimeQuote
        {
            Id = 3,
            Quote = "Even a low-class warrior can surpass an elite, with enough hard work.",
            Character = "Goku",
            Anime = "Dragon Ball Z"
        },

        // 🟡 Pokémon
        new AnimeQuote
        {
            Id = 4,
            Quote = "I wanna be the very best, like no one ever was.",
            Character = "Ash Ketchum",
            Anime = "Pokémon"
        },
        new AnimeQuote
        {
            Id = 5,
            Quote = "We do have a lot in common. The same Earth, the same air, the same sky.",
            Character = "Mewtwo",
            Anime = "Pokémon: The First Movie"
        },
        new AnimeQuote
        {
            Id = 6,
            Quote = "Pikachu, I choose you!",
            Character = "Ash Ketchum",
            Anime = "Pokémon"
        },

        // 🔷 Yu-Gi-Oh!
        new AnimeQuote
        {
            Id = 7,
            Quote = "It's time to duel!",
            Character = "Yugi Muto",
            Anime = "Yu-Gi-Oh!"
        },
        new AnimeQuote
        {
            Id = 8,
            Quote = "I will not lose! Not to Kaiba, not to anyone!",
            Character = "Yugi Muto",
            Anime = "Yu-Gi-Oh!"
        },
    };

    [HttpGet]
    public ActionResult<AnimeQuote> GetQuote()
    {
        var random = new Random();
        AnimeQuote randomQuote = _animeQuotes[random.Next(_animeQuotes.Count)];
        return Ok(randomQuote);
    }

    [HttpGet("all")]
    public ActionResult<List<AnimeQuote>> GetAll()
    {
        return Ok(_animeQuotes);
    }
}
