using BlueLuck.Actions;

namespace BlueLuck.Native;

public interface IGameBridge
{
    bool IsReady { get; }

    ActionResult Announce(string message);
    ActionResult Notify(ulong steamId, string message);
    ActionResult SpawnRequest(
        ActionContext context,
        IReadOnlyDictionary<string, string> parameters);
    ActionResult EffectRequest(
        ActionContext context,
        IReadOnlyDictionary<string, string> parameters);
    ActionResult SetZoneRadius(ActionContext context, float radius);
    ActionResult SetPvp(ActionContext context, bool enabled);
    ActionResult Heal(ActionContext context, string target);
}
