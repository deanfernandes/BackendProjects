using MongoDB.Driver;
using ProductReviews.Context;
using ProductReviews.Models;

namespace ProductReviews.Repositories;

public interface IReviewRepository
{
    Task<List<Review>> GetAllAsync();
    Task<Review?> GetByIdAsync(string id);
    Task<List<Review>> GetByProductIdAsync(string productId);
    Task CreateAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(string id);
}

public class ReviewRepository : IReviewRepository
{
    private readonly IMongoCollection<Review> _reviews;

    public ReviewRepository(MongoDbContext context)
    {
        _reviews = context.Reviews;
    }

    public async Task<List<Review>> GetAllAsync() =>
        await _reviews.Find(_ => true).ToListAsync();

    public async Task<Review?> GetByIdAsync(string id) =>
        await _reviews.Find(r => r.Id == id).FirstOrDefaultAsync();

    public async Task<List<Review>> GetByProductIdAsync(string productId) =>
        await _reviews.Find(r => r.ProductId == productId).ToListAsync();

    public async Task CreateAsync(Review review) =>
        await _reviews.InsertOneAsync(review);

    public async Task UpdateAsync(Review review) =>
        await _reviews.ReplaceOneAsync(r => r.Id == review.Id, review);

    public async Task DeleteAsync(string id) =>
        await _reviews.DeleteOneAsync(r => r.Id == id);
}
