using Microsoft.AspNetCore.Mvc;
using RandomQuotesApi.Models;
using System.Collections.Generic;
using RandomQuotesApi.Services;
using System.Threading.Tasks;

namespace RandomQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusicController : ControllerBase
{
    private readonly SongService _songService;

    public MusicController(SongService songService)
    {
        _songService = songService;
    }

    [HttpGet]
    public async Task<ActionResult<SongQuote>> GetQuote([FromQuery] string? artist = null, [FromQuery] string? album = null, [FromQuery] string? song = null)
    {
        var randomQuote = await _songService.GetRandomAsync(artist, album, song);

        if (randomQuote == null)
            return NotFound("No matching quotes found.");

        return Ok(randomQuote.ToDto());
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<SongQuoteDto>>> GetAll()
    {
        var allQuotes = await _songService.GetAllAsync();
        var dtoQuotes = allQuotes.Select(q => q.ToDto()).ToList();

        return Ok(dtoQuotes);
    }

    [HttpGet("artists")]
    public async Task<ActionResult<List<string>>> GetArtists()
    {
        var artists = await _songService.GetDistinctArtistsAsync();
        return Ok(artists);
    }

    [HttpGet("albums")]
    public async Task<ActionResult<List<string>>> GetAlbums()
    {
        var albums = await _songService.GetDistinctAlbumsAsync();
        return Ok(albums);
    }

    [HttpGet("songs")]
    public async Task<ActionResult<List<string>>> GetSongs()
    {
        var songs = await _songService.GetDistinctSongsAsync();
        return Ok(songs);
    }
}