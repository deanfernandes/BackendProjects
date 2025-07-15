using InventoryOrderSystem.Data.Context;
using InventoryOrderSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryOrderSystem.Data.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users.FindAsync(id);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _context.Users.ToListAsync();

    public async Task AddAsync(User user)
    {
        var (hash, salt) = PasswordHelper.HashPassword(user.PasswordHash);
        user.PasswordHash = hash;
        user.Salt = salt;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null) return null;

        string hashToCheck = PasswordHelper.HashWithSalt(password, user.Salt);

        if (hashToCheck == user.PasswordHash)
            return user;

        return null;
    }
}
