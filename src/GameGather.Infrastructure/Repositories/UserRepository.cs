using GameGather.Application.Persistance;
using GameGather.Domain.Aggregates.Users;
using GameGather.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameGather.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GameGatherDbContext _dbContext;

    public UserRepository(GameGatherDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(r => r.Email == email);

        return user;
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<bool> AnyUserAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.AnyAsync(r => r.Email == email);
    }
}