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

public class AnimeControllerTests
{
    [Fact]
    public async Task GetQuoteAsync_ReturnsQuote()
    {
        var mockService = new Mock<IAnimeService>();
        AnimeQuote mockQuote = new AnimeQuote
        {
            Id = "1",
            Quote = "Pikachu, I choose you!",
            Anime = "Pokémon",
            Character = "Ash Ketchum"
        };
        mockService.Setup(s => s.GetRandomAsync(null, null)).ReturnsAsync(mockQuote);
        var animeController = new AnimeController(mockService.Object);

        ActionResult<AnimeQuoteDto> actionResult = await animeController.GetQuoteAsync(null, null);
        OkObjectResult okResult = (OkObjectResult)actionResult.Result;
        AnimeQuoteDto animeQuote = okResult.Value as AnimeQuoteDto;

        Assert.NotNull(animeQuote);
        Assert.Equal(mockQuote.Quote, animeQuote.Quote);
        Assert.Equal(mockQuote.Anime, animeQuote.Anime);
        Assert.Equal(mockQuote.Character, animeQuote!.Character);
        mockService.Verify(s => s.GetRandomAsync(null, null), Times.AtLeastOnce());
    }

    [Fact]
    public async Task GetQuoteAsync_NoQuoteFound_ReturnsNotFound()
    {
        var mockService = new Mock<IAnimeService>();
        mockService.Setup(s => s.GetRandomAsync(null, null)).ReturnsAsync((AnimeQuote?)null);
        var controller = new AnimeController(mockService.Object);

        ActionResult<AnimeQuoteDto> actionResult = await controller.GetQuoteAsync(null, null);
        NotFoundObjectResult notFoundResult = actionResult.Result as NotFoundObjectResult;

        Assert.Equal("No matching quotes found.", notFoundResult.Value);
        mockService.Verify(s => s.GetRandomAsync(null, null), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllQuotes()
    {
        var mockQuotes = new List<AnimeQuote>
        {
            new AnimeQuote { Id = "1", Quote = "Pikachu, I choose you!", Anime = "Pokémon", Character = "Ash Ketchum" },
            new AnimeQuote { Id = "2", Quote = "I'm gonna be King of the Pirates!", Character = "Luffy", Anime = "One Piece" },
            new AnimeQuote { Id = "3", Quote = "It's time to duel!", Character = "Yugi Muto", Anime = "Yu-Gi-Oh!" },
        };
        var mockService = new Mock<IAnimeService>();
        mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockQuotes);
        var controller = new AnimeController(mockService.Object);

        var result = await controller.GetAllAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var animeQuoteDtos = Assert.IsType<List<AnimeQuoteDto>>(okResult.Value);
        Assert.Equal(mockQuotes.Count, animeQuoteDtos.Count);
        for (int i = 0; i < mockQuotes.Count; i++)
        {
            Assert.Equal(mockQuotes[i].Quote, animeQuoteDtos[i].Quote);
            Assert.Equal(mockQuotes[i].Anime, animeQuoteDtos[i].Anime);
            Assert.Equal(mockQuotes[i].Character, animeQuoteDtos[i].Character);
        }
        Assert.False(typeof(AnimeQuoteDto).GetProperty("Id") != null, "AnimeQuoteDto should not have an 'Id' property.");
        mockService.Verify(s => s.GetAllAsync(), Times.AtLeastOnce());
    }

    [Fact]
    public async Task GetAnimesAsync_ReturnsOkResultWithAnimeList()
    {
        var mockService = new Mock<IAnimeService>();
        var mockAnimes = new List<string> { "Dragon Ball", "Dragon Ball Z", "Pokémon" };

        mockService.Setup(s => s.GetDistinctAnimesAsync())
                   .ReturnsAsync(mockAnimes);

        var controller = new AnimeController(mockService.Object);

        var result = await controller.GetAnimesAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<string>>(okResult.Value);
        Assert.Equal(mockAnimes.Count, returnedList.Count);
        Assert.Equal(mockAnimes, returnedList);
    }

    [Fact]
    public async Task GetCharactersAsync_ReturnsOkResultWithCharacterList()
    {
        var mockService = new Mock<IAnimeService>();
        var mockCharacters = new List<string> { "Ash Ketchum", "Goku", "Yugi Muto" };

        mockService.Setup(service => service.GetDistinctCharactersAsync())
                   .ReturnsAsync(mockCharacters);

        var controller = new AnimeController(mockService.Object);

        var result = await controller.GetCharactersAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<string>>(okResult.Value);

        Assert.Equal(mockCharacters.Count, returnedList.Count);
        Assert.Equal(mockCharacters, returnedList);
    }
}