namespace BlueLuck.Events;

public sealed class EventSession
{
    public string SessionId { get; } = Guid.NewGuid().ToString("N");
    public EventDefinition Definition { get; }
    public DateTime CreatedUtc { get; } = DateTime.UtcNow;
    public HashSet<ulong> Players { get; } = new();

    public Dictionary<string, object> State { get; } =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["status"] = "created",
            ["phase"] = "created"
        };

    public EventSession(EventDefinition definition)
    {
        Definition = definition;
    }
}
