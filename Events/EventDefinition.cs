using BlueLuck.Actions;

namespace BlueLuck.Events;

public sealed class EventDefinition
{
    public EventMetadata Metadata { get; set; } = new();
    public EventRules Rules { get; set; } = new();
    public Dictionary<string, List<ActionRequest>> Flows { get; set; } =
        new(StringComparer.OrdinalIgnoreCase);
    public EventAiPolicy Ai { get; set; } = new();
}

public sealed class EventMetadata
{
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Version { get; set; } = "1";
    public bool Enabled { get; set; } = true;
    public string Status { get; set; } = "template";
}

public sealed class EventRules
{
    public int MinPlayers { get; set; } = 1;
    public int MaxPlayers { get; set; } = 20;
    public int LivesPerPlayer { get; set; } = 1;
    public int DurationSeconds { get; set; } = 600;
    public float ZoneRadius { get; set; } = 60f;
}

public sealed class EventAiPolicy
{
    public bool Enabled { get; set; }
    public bool AllowConfigEdits { get; set; }
    public bool RequireApproval { get; set; } = true;
    public List<string> EditableFields { get; set; } = new();
    public List<string> AllowedActions { get; set; } = new();
    public string Prompt { get; set; } = string.Empty;
}
