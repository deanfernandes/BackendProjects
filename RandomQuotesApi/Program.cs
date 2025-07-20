using MongoDB.Driver;
using RandomQuotesApi.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IAnimeService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var mongoClient = new MongoClient(config.GetConnectionString("MongoDb"));
    var database = mongoClient.GetDatabase(config["MongoDbSettings:DatabaseName"]);

    return new AnimeService(database);
});

builder.Services.AddSingleton<SongService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("MongoDb");
    var databaseName = config["MongoDbSettings:DatabaseName"];

    var mongoClient = new MongoClient(connectionString);
    var database = mongoClient.GetDatabase(databaseName);

    return new SongService(database);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var animeService = scope.ServiceProvider.GetRequiredService<IAnimeService>();
    await animeService.SeedQuotesAsync();

    var songService = scope.ServiceProvider.GetRequiredService<SongService>();
    await songService.SeedQuotesAsync();
}

app.MapControllers();

app.Run();