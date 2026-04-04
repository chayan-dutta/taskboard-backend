using Microsoft.EntityFrameworkCore;
using TaskBoard.AuthService.Infrastructure.Database;
using TaskBoard.AuthService.Infrastructure.Entities;

namespace TaskBoard.AuthService.Infrastructure.Repositories;

public class UserRepository(UserServiceDbContext dbContext) : IUserRepository
{
    private readonly UserServiceDbContext _context = dbContext;

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
