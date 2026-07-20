namespace BlueLuck.Actions;

public sealed class ActionRequest
{
    public string Action { get; set; } = string.Empty;
    public Dictionary<string, string> Params { get; set; } =
        new(StringComparer.OrdinalIgnoreCase);
    public bool Approved { get; set; }
}

public sealed class ActionContext
{
    public string EventId { get; init; } = string.Empty;
    public string SessionId { get; init; } = string.Empty;
    public Dictionary<string, object> State { get; init; } = new(StringComparer.OrdinalIgnoreCase);
    public ulong RequestedBy { get; init; }
}
