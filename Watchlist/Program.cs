using System.Collections.Generic;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore;
//using NSwag.AspNetCore;

class Anime
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool Watched { get; set; } = false;

    public DateTime CreatedAt { get; set; }

    public Anime()
    {
        CreatedAt = DateTime.Now;
    }

    public Anime(int id, string title, bool watched = false)
    {
        Id = id;
        Title = title;
        Watched = watched;
        CreatedAt = DateTime.Now;
    }
}

class CreateAnimeDto
{
    public string Title { get; set; }

    public CreateAnimeDto()
    {

    }

    public CreateAnimeDto(Anime anime)
    {
        Title = anime.Title;
    }
}

class AnimeDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool Watched { get; set; } = false;

    public AnimeDto(Anime anime)
    {
        Id = anime.Id;
        Title = anime.Title;
        Watched = anime.Watched;
    }
}

class UpdateAnimeDto
{
    public string Title { get; set; }
    public bool Watched { get; set; } = false;
}

class Program
{
    static List<Anime> animes = new List<Anime>
    {
        new Anime(1, "dragon ball"),
        new Anime { Id = 2, Title = "pokemon", },
        new Anime(3, "yugioh"),
        new Anime(4, "naruto"),
    };

    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        //builder.Services.AddOpenApiDocument();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            /*
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            */
            //app.UseOpenApi();
            //app.UseSwaggerUi();
        }

        var animesMapGroup = app.MapGroup("/animes");
        animesMapGroup.MapGet("/", GetAllAnimes);
        animesMapGroup.MapGet("/{id}", GetAnime)
        .Produces<AnimeDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        animesMapGroup.MapGet("/watched", GetAllWatchedAnimes);
        animesMapGroup.MapPost("/", CreateAnime);
        animesMapGroup.MapPut("/{id}", UpdateAnime);
        animesMapGroup.MapDelete("/{id}", DeleteAnime);

        app.Run();
    }

    static IResult GetAllAnimes()
    {
        return TypedResults.Ok<IEnumerable<AnimeDto>>(animes.Select(a => new AnimeDto(a)));
    }

    static IResult GetAnime(int id)
    {
        var anime = animes.Find(a => a.Id == id);
        if (anime is null)
            return TypedResults.NotFound();

        return TypedResults.Ok<AnimeDto>(new AnimeDto(anime));
    }

    static IResult GetAllWatchedAnimes()
    {
        return TypedResults.Ok<IEnumerable<AnimeDto>>(animes.Where(a => a.Watched).Select(a => new AnimeDto(a)));
    }

    static IResult CreateAnime(CreateAnimeDto createAnimeDto)
    {
        var anime = new Anime(animes.Count + 1, createAnimeDto.Title);
        animes.Add(anime);
        return TypedResults.Created<AnimeDto>($"/animes/{anime.Id}", new AnimeDto(anime));
    }

    static IResult UpdateAnime(int id, UpdateAnimeDto updateAnimeDto)
    {
        var a = animes.Find(a => a.Id == id);
        if (a is null)
            return TypedResults.NotFound();

        a.Title = updateAnimeDto.Title;
        a.Watched = updateAnimeDto.Watched;

        return TypedResults.NoContent();
    }

    static IResult DeleteAnime(int id)
    {
        var anime = animes.Find(a => a.Id == id);
        if (anime is null)
            return TypedResults.NotFound();

        animes.Remove(anime);
        return TypedResults.NoContent();
    }
}