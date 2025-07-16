using ProductReviews.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductReviews.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string id);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(string id);
    Task<User?> GetByEmailAndPasswordAsync(string email, string password);
}
