using BlueLuck.Actions;

namespace BlueLuck.Native;

public sealed class NullGameBridge : IGameBridge
{
    public static NullGameBridge Instance { get; } = new();

    public bool IsReady => false;

    NullGameBridge()
    {
    }

    public ActionResult Announce(string message) =>
        ActionResult.Fail("Native game bridge is not installed.");

    public ActionResult Notify(ulong steamId, string message) =>
        ActionResult.Fail("Native game bridge is not installed.");

    public ActionResult SpawnRequest(
        ActionContext context,
        IReadOnlyDictionary<string, string> parameters) =>
        ActionResult.Fail("Native spawn adapter is not installed.");

    public ActionResult EffectRequest(
        ActionContext context,
        IReadOnlyDictionary<string, string> parameters) =>
        ActionResult.Fail("Native effect adapter is not installed.");

    public ActionResult SetZoneRadius(ActionContext context, float radius) =>
        ActionResult.Fail("Native zone adapter is not installed.");

    public ActionResult SetPvp(ActionContext context, bool enabled) =>
        ActionResult.Fail("Native PvP adapter is not installed.");

    public ActionResult Heal(ActionContext context, string target) =>
        ActionResult.Fail("Native player-state adapter is not installed.");
}
