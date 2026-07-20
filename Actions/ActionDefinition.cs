namespace BlueLuck.Actions;

public sealed class ActionDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "general";
    public ActionRisk Risk { get; set; } = ActionRisk.Safe;
    public bool RequiresApproval { get; set; }
    public bool Enabled { get; set; } = true;
    public List<string> Required { get; set; } = new();
    public List<string> Optional { get; set; } = new();
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionRisk
{
    Safe,
    Controlled,
    Blocked
}
