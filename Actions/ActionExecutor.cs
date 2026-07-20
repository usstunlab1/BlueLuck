using BlueLuck.Native;

namespace BlueLuck.Actions;

public delegate ActionResult BlueLuckActionHandler(
    ActionRequest request,
    ActionContext context,
    IGameBridge game);

public sealed class ActionExecutor
{
    readonly ActionCatalog _catalog;
    readonly Dictionary<string, BlueLuckActionHandler> _handlers =
        new(StringComparer.OrdinalIgnoreCase);
    readonly Action<string> _log;
    IGameBridge _game;

    public ActionExecutor(ActionCatalog catalog, IGameBridge game, Action<string> log)
    {
        _catalog = catalog;
        _game = game;
        _log = log;
    }

    public void SetGameBridge(IGameBridge game) =>
        _game = game ?? throw new ArgumentNullException(nameof(game));

    public void Register(string actionName, BlueLuckActionHandler handler)
    {
        var name = ActionCatalog.Normalize(actionName);
        if (!_catalog.IsKnown(name))
            throw new InvalidOperationException($"Cannot register unknown action '{name}'.");
        if (!_handlers.TryAdd(name, handler))
            throw new InvalidOperationException($"Handler already registered for '{name}'.");
    }

    public ActionResult Execute(ActionRequest request, ActionContext context)
    {
        var name = ActionCatalog.Normalize(request.Action);

        if (!_catalog.TryGet(name, out var definition))
            return ActionResult.Fail($"Unknown action '{name}'.");
        if (!definition.Enabled || definition.Risk == ActionRisk.Blocked)
            return ActionResult.Fail($"Action '{name}' is disabled or blocked.");
        if (definition.RequiresApproval && !request.Approved)
            return ActionResult.Fail($"Action '{name}' requires approval.");

        foreach (var parameter in definition.Required)
        {
            if (!request.Params.TryGetValue(parameter, out var value) ||
                string.IsNullOrWhiteSpace(value))
                return ActionResult.Fail(
                    $"Action '{name}' requires parameter '{parameter}'.");
        }

        if (!_handlers.TryGetValue(name, out var handler))
            return ActionResult.Fail($"Action '{name}' has no runtime handler yet.");

        try
        {
            var result = handler(request, context, _game);
            _log($"[BlueLuck] {name}: {(result.Success ? "ok" : "failed")} - {result.Message}");
            return result;
        }
        catch (Exception ex)
        {
            _log($"[BlueLuck] {name}: exception - {ex.Message}");
            return ActionResult.Fail($"Action '{name}' failed: {ex.Message}");
        }
    }
}
