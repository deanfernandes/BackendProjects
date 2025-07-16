using MongoDB.Driver;
using ProductReviews.Models;

namespace ProductReviews.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString = "mongodb://localhost:27017", string databaseName = "ProductReviewsDB")
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    public IMongoCollection<Review> Reviews => _database.GetCollection<Review>("Reviews");
}
