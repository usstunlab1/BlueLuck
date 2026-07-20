namespace BlueLuck.Actions;

public sealed class ActionCatalog
{
    public const int HardMaximumActions = 300;

    readonly Dictionary<string, ActionDefinition> _definitions;

    public int MaximumActions { get; }
    public int Count => _definitions.Count;
    public IReadOnlyCollection<ActionDefinition> Entries => _definitions.Values;

    ActionCatalog(int maximumActions, IEnumerable<ActionDefinition> definitions)
    {
        MaximumActions = Math.Clamp(maximumActions, 1, HardMaximumActions);
        _definitions = new Dictionary<string, ActionDefinition>(StringComparer.OrdinalIgnoreCase);

        foreach (var definition in definitions)
        {
            var name = Normalize(definition.Name);
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidDataException("Action names cannot be empty.");
            if (_definitions.ContainsKey(name))
                throw new InvalidDataException($"Duplicate action '{name}'.");
            if (_definitions.Count >= MaximumActions)
                throw new InvalidDataException($"Action catalog exceeds its limit of {MaximumActions}.");

            definition.Name = name;
            _definitions.Add(name, definition);
        }
    }

    public static ActionCatalog Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("BlueLuck action catalog was not found.", path);

        var document = JsonSerializer.Deserialize<ActionCatalogFile>(
            File.ReadAllText(path), JsonOptions.Create())
            ?? throw new InvalidDataException("Action catalog JSON was empty.");

        return new ActionCatalog(document.MaxActions, document.Actions ?? new());
    }

    public bool TryGet(string actionName, out ActionDefinition definition) =>
        _definitions.TryGetValue(Normalize(actionName), out definition!);

    public bool IsKnown(string actionName) => TryGet(actionName, out _);

    public static string Normalize(string? actionName) =>
        string.IsNullOrWhiteSpace(actionName)
            ? string.Empty
            : actionName.Trim().ToLowerInvariant();

    sealed class ActionCatalogFile
    {
        public int MaxActions { get; set; } = HardMaximumActions;
        public List<ActionDefinition>? Actions { get; set; }
    }
}
