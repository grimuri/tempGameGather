using GameGather.Domain.Aggregates.Users;

namespace GameGather.Application.Persistance;

public interface IUserRepository
{
    User? GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    void AddUserAsync(User user, CancellationToken cancellationToken = default);
    bool AnyUserAsync(string email, CancellationToken cancellationToken = default);
}