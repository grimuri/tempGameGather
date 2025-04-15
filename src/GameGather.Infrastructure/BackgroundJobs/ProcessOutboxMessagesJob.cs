using GameGather.Domain.Common.Primitives;
using GameGather.Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace GameGather.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly GameGatherDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(
        GameGatherDbContext gameGatherDbContext, 
        IPublisher publisher)
    {
        _dbContext = gameGatherDbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext
            .OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var domainEvent = JsonConvert
                    .DeserializeObject<IDomainEvent>(
                        message.Content, 
                        new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });

                if (domainEvent is null)
                {
                    message.ErrorMessage = "Failed to deserialize domain event";
                }

                await _publisher.Publish(domainEvent, context.CancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;
                message.ErrorMessage = null;
            }
            catch (JsonException e)
            {
                message.ErrorMessage = $"Deserialization error: {e.Message}";
            }
            catch (Exception e)
            {
                message.ErrorMessage = $"Processing error: {e.Message}";
            }
            
        }
        
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}