using Xunit;
using FluentAssertions;
using Moq;
using RandomQuotesApi.Services;
using RandomQuotesApi.Controllers;
using RandomQuotesApi.Models;
using Xunit.Sdk;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RandomQuotesApi.Tests;

public class SongControllerTests
{
    [Fact]
    public async Task GetQuoteAsync_ReturnsQuote()
    {
        var mockService = new Mock<ISongService>();
        SongQuote mockQuote = new SongQuote
        {
            Id = "1",
            Quote = "Started from the bottom now we here.",
            Song = "Started From the Bottom",
            Album = "Nothing Was the Same",
            Artist = "Drake",
        };
        mockService.Setup(s => s.GetRandomAsync(null, null, null)).ReturnsAsync(mockQuote);
        var songController = new SongController(mockService.Object);

        ActionResult<SongQuoteDto> actionResult = await songController.GetQuoteAsync(null, null);

        OkObjectResult okResult = (OkObjectResult)actionResult.Result;
        SongQuoteDto songQuote = okResult.Value as SongQuoteDto;
        Assert.NotNull(songQuote);
        Assert.Equal(mockQuote.Quote, songQuote.Quote);
        Assert.Equal(mockQuote.Album, songQuote.Album);
        Assert.Equal(mockQuote.Artist, songQuote.Artist);
        Assert.Equal(mockQuote.Song, songQuote.Song);
        mockService.Verify(s => s.GetRandomAsync(null, null, null), Times.AtLeastOnce());
    }

    [Fact]
    public async Task GetArtistsAsync_ReturnsOkResultWithArtistList()
    {
        var mockService = new Mock<ISongService>();
        var mockArtists = new List<string> { "Artist1", "Artist2", "Artist3" };
        mockService.Setup(service => service.GetDistinctArtistsAsync())
                   .ReturnsAsync(mockArtists);
        var controller = new SongController(mockService.Object);

        var result = await controller.GetArtistsAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<string>>(okResult.Value);
        Assert.Equal(mockArtists.Count, returnedList.Count);
        Assert.Equal(mockArtists, returnedList);
    }

    [Fact]
    public async Task GetAlbumsAsync_ReturnsOkResultWithAlbumList()
    {
        var mockService = new Mock<ISongService>();
        var expectedAlbums = new List<string> { "Album1", "Album2", "Album3" };
        mockService.Setup(service => service.GetDistinctAlbumsAsync())
                   .ReturnsAsync(expectedAlbums);
        var controller = new SongController(mockService.Object);

        var result = await controller.GetAlbumsAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<string>>(okResult.Value);
        Assert.Equal(expectedAlbums.Count, returnedList.Count);
        Assert.Equal(expectedAlbums, returnedList);
    }

    [Fact]
    public async Task GetSongsAsync_ReturnsOkResultWithSongList()
    {
        var mockService = new Mock<ISongService>();
        var expectedSongs = new List<string> { "Song1", "Song2", "Song3" };
        mockService.Setup(service => service.GetDistinctSongsAsync())
                   .ReturnsAsync(expectedSongs);
        var controller = new SongController(mockService.Object);

        var result = await controller.GetSongsAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<string>>(okResult.Value);
        Assert.Equal(expectedSongs.Count, returnedList.Count);
        Assert.Equal(expectedSongs, returnedList);
    }
}