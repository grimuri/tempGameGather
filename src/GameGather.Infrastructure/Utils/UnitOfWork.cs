using GameGather.Application.Persistance;
using GameGather.Domain.Common.Primitives;
using GameGather.Infrastructure.Persistance;
using GameGather.Infrastructure.Utils.Outbox;
using Newtonsoft.Json;

namespace GameGather.Infrastructure.Utils;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly GameGatherDbContext _dbContext;

    public UnitOfWork(GameGatherDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await ConvertDomainEventsToOutboxMessagesAsync();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private async Task ConvertDomainEventsToOutboxMessagesAsync()
    {
        var outboxMessages = _dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Any())
            .SelectMany(x =>
            {
                var domainEvents = x.DomainEvents.ToList();
                x.ClearDomainEvents();
                return domainEvents;
            })
            .Select(x => new OutboxMessage
            {
                OccuredOnUtc = DateTime.UtcNow,
                MessageType = x.GetType().Name,
                Content = JsonConvert.SerializeObject(x, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            })
            .ToList();

        await _dbContext.OutboxMessages.AddRangeAsync(outboxMessages);
    }
}