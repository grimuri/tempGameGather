using GameGather.Application.Persistance;
using GameGather.Domain.Aggregates.Users;
using GameGather.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace GameGather.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GameGatherDbContext _dbContext;

    public UserRepository(GameGatherDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    

    public User GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = _dbContext.Users.FirstOrDefault(r => r.Email == email);

        return user;
    }

    public void AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Add(user);
    }

    public bool AnyUserAsync(string email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users.Any(r => r.Email == email);
    }
}