using BlueLuck.Actions;
using BlueLuck.AI;
using BlueLuck.Events;
using BlueLuck.Native;

namespace BlueLuck.Core;

public sealed class BlueLuckCore : IDisposable
{
    readonly Action<string> _log;

    public ActionCatalog Actions { get; private set; } = null!;
    public ActionExecutor Executor { get; private set; } = null!;
    public EventController Events { get; private set; } = null!;
    public BlueLuckAssistant Assistant { get; private set; } = null!;
    public IGameBridge GameBridge { get; private set; } = NullGameBridge.Instance;

    public BlueLuckCore(Action<string> log)
    {
        _log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public void Initialize(string configDirectory)
    {
        if (string.IsNullOrWhiteSpace(configDirectory))
            throw new ArgumentException("Config directory is required.", nameof(configDirectory));
        Directory.CreateDirectory(configDirectory);

        ConfigDeployment.DeployEmbeddedDefaults(configDirectory, _log);

        Actions = ActionCatalog.Load(Path.Combine(configDirectory, "actions.json"));
        Executor = new ActionExecutor(Actions, GameBridge, _log);
        BuiltInActionHandlers.Register(Executor);

        var definitions = EventLoader.LoadDirectory(Path.Combine(configDirectory, "events"));
        EventValidator.ValidateAll(definitions, Actions);
        Events = new EventController(definitions, Executor, _log);

        var aiConfig = BlueLuckAiConfig.Load(Path.Combine(configDirectory, "ai.json"));
        Assistant = new BlueLuckAssistant(aiConfig, Actions);

        _log($"[BlueLuck] Loaded {Actions.Count}/{Actions.MaximumActions} actions and {definitions.Count} events.");
    }

    public void SetGameBridge(IGameBridge bridge)
    {
        GameBridge = bridge ?? throw new ArgumentNullException(nameof(bridge));
        Executor.SetGameBridge(GameBridge);
    }

    public void Dispose()
    {
        Events?.Dispose();
        Assistant?.Dispose();
    }
}
