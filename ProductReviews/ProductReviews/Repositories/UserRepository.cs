using MongoDB.Driver;
using ProductReviews.Models;
using ProductReviews.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductReviews.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(MongoDbContext context)
    {
        _users = context.Users;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _users.Find(_ => true).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
    }

    public async Task DeleteAsync(string id)
    {
        await _users.DeleteOneAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
    {
        return await _users.Find(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
    }
}
