using BlueLuck.Actions;
using BlueLuck.Events;

namespace BlueLuck.AI;

public sealed class BlueLuckAssistant : IDisposable
{
    readonly BlueLuckAiConfig _config;
    readonly ActionCatalog _catalog;

    public BlueLuckAssistant(
        BlueLuckAiConfig config,
        ActionCatalog catalog)
    {
        _config = config;
        _catalog = catalog;
    }

    public AiPlanResult ValidatePlan(
        EventDefinition definition,
        IEnumerable<ActionRequest> requests)
    {
        if (!_config.Enabled)
            return AiPlanResult.Rejected("AI assistance is disabled.");

        var actions = requests
            .Take(Math.Clamp(_config.MaximumActionsPerPlan, 1, 20))
            .ToList();

        var allowed = new HashSet<string>(
            definition.Ai.AllowedActions.Select(ActionCatalog.Normalize),
            StringComparer.OrdinalIgnoreCase);

        foreach (var request in actions)
        {
            var name = ActionCatalog.Normalize(request.Action);

            if (!_catalog.TryGet(name, out var catalogEntry))
                return AiPlanResult.Rejected($"Unknown action '{name}'.");

            if (!allowed.Contains(name))
                return AiPlanResult.Rejected(
                    $"Action '{name}' is not allowed by this event.");

            if (catalogEntry.RequiresApproval && !request.Approved)
                return AiPlanResult.Rejected(
                    $"Action '{name}' requires approval.");
        }

        return AiPlanResult.Accepted(actions);
    }

    public void Dispose()
    {
    }
}

public sealed class AiPlanResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public IReadOnlyList<ActionRequest> Actions { get; init; } =
        Array.Empty<ActionRequest>();

    public static AiPlanResult Accepted(
        IReadOnlyList<ActionRequest> actions) =>
        new()
        {
            Success = true,
            Message = "Plan validated.",
            Actions = actions
        };

    public static AiPlanResult Rejected(string message) =>
        new()
        {
            Success = false,
            Message = message
        };
}
