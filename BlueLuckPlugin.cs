using BepInEx;
using BepInEx.Unity.IL2CPP;
using BlueLuck.Core;

namespace BlueLuck;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class BlueLuckPlugin : BasePlugin
{
    public const string PluginGuid = "dev.blueluck.events";
    public const string PluginName = "BlueLuck Event";
    public const string PluginVersion = "0.1.0";

    BlueLuckCore? _core;

    public override void Load()
    {
        try
        {
            var configDirectory = Path.Combine(Paths.ConfigPath, "BlueLuck");
            _core = new BlueLuckCore(message => Log.LogInfo(message));
            _core.Initialize(configDirectory);
            Log.LogInfo($"{PluginName} {PluginVersion} loaded.");
        }
        catch (Exception ex)
        {
            Log.LogError($"BlueLuck failed to initialize: {ex}");
            throw;
        }
    }

    public override bool Unload()
    {
        try
        {
            _core?.Dispose();
            _core = null;
            return true;
        }
        catch (Exception ex)
        {
            Log.LogError($"BlueLuck failed to unload cleanly: {ex}");
            return false;
        }
    }
}
