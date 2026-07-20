namespace BlueLuck.Events;

public static class EventLoader
{
    public static IReadOnlyDictionary<string, EventDefinition> LoadDirectory(
        string directory)
    {
        Directory.CreateDirectory(directory);
        var definitions = new Dictionary<string, EventDefinition>(
            StringComparer.OrdinalIgnoreCase);
        var options = JsonOptions.Create();

        foreach (var path in Directory.EnumerateFiles(
                     directory,
                     "*.json",
                     SearchOption.TopDirectoryOnly))
        {
            var definition = JsonSerializer.Deserialize<EventDefinition>(
                File.ReadAllText(path),
                options)
                ?? throw new InvalidDataException(
                    $"Event file '{path}' was empty.");

            var id = definition.Metadata.Id.Trim();
            if (string.IsNullOrWhiteSpace(id))
                throw new InvalidDataException(
                    $"Event file '{path}' has no metadata.id.");

            if (!definitions.TryAdd(id, definition))
                throw new InvalidDataException($"Duplicate event id '{id}'.");
        }

        return definitions;
    }
}
