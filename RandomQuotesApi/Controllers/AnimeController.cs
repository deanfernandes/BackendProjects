using Microsoft.AspNetCore.Mvc;
using RandomQuotesApi.Models;
using RandomQuotesApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimeController : ControllerBase
{
    private readonly IAnimeService _animeService;

    public AnimeController(IAnimeService animeService)
    {
        _animeService = animeService;
    }

    // GET api/anime?anime=Dragon%20Ball&character=Goku
    [HttpGet]
    public async Task<ActionResult<AnimeQuoteDto>> GetQuoteAsync([FromQuery] string? anime = null, [FromQuery] string? character = null)
    {
        var randomQuote = await _animeService.GetRandomAsync(anime, character);

        if (randomQuote == null)
            return NotFound("No matching quotes found.");

        return Ok(randomQuote.ToDto());
    }

    // GET api/anime/all
    [HttpGet("all")]
    public async Task<ActionResult<List<AnimeQuoteDto>>> GetAllAsync()
    {
        var allQuotes = await _animeService.GetAllAsync();
        var dtoQuotes = allQuotes.Select(q => q.ToDto()).ToList();

        return Ok(dtoQuotes);
    }

    // GET api/anime/animes
    [HttpGet("animes")]
    public async Task<ActionResult<List<string>>> GetAnimesAsync()
    {
        var animes = await _animeService.GetDistinctAnimesAsync();
        return Ok(animes);
    }

    // GET api/anime/characters
    [HttpGet("characters")]
    public async Task<ActionResult<List<string>>> GetCharactersAsync()
    {
        var characters = await _animeService.GetDistinctCharactersAsync();
        return Ok(characters);
    }
}
