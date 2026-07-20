namespace BlueLuck.AI;

public sealed class BlueLuckAiConfig
{
    public bool Enabled { get; set; }
    public string Mode { get; set; } = "plan_only";
    public string Provider { get; set; } = "none";
    public bool RequireStructuredJson { get; set; } = true;
    public bool AutoExecute { get; set; }
    public int MaximumActionsPerPlan { get; set; } = 8;

    public static BlueLuckAiConfig Load(string path)
    {
        if (!File.Exists(path))
            return new BlueLuckAiConfig();

        return JsonSerializer.Deserialize<BlueLuckAiConfig>(
                   File.ReadAllText(path),
                   JsonOptions.Create())
               ?? new BlueLuckAiConfig();
    }
}
