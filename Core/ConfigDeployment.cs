using System.Reflection;

namespace BlueLuck.Core;

public static class ConfigDeployment
{
    const string ResourcePrefix = "BlueLuck.config.BlueLuck.";

    public static void DeployEmbeddedDefaults(string destinationRoot, Action<string> log)
    {
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var resourceName in assembly.GetManifestResourceNames())
        {
            if (!resourceName.StartsWith(ResourcePrefix, StringComparison.Ordinal) ||
                !resourceName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                continue;

            var relative = resourceName[ResourcePrefix.Length..]
                .Replace(".json", "§json", StringComparison.OrdinalIgnoreCase)
                .Replace('.', Path.DirectorySeparatorChar)
                .Replace("§json", ".json", StringComparison.OrdinalIgnoreCase);

            var destination = Path.Combine(destinationRoot, relative);
            if (File.Exists(destination))
                continue;

            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            using var source = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException($"Missing embedded resource '{resourceName}'.");
            using var target = File.Create(destination);
            source.CopyTo(target);
            log($"[BlueLuck] Created default config: {destination}");
        }
    }
}
