namespace GameGather.Infrastructure.Utils.Outbox;

public sealed class OutboxMessage
{
    public int Id { get; set; }
    public string MessageType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime OccuredOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? ErrorMessage { get; set; }
}