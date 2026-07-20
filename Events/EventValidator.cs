using BlueLuck.Actions;

namespace BlueLuck.Events;

public static class EventValidator
{
    public static void ValidateAll(
        IReadOnlyDictionary<string, EventDefinition> definitions,
        ActionCatalog catalog)
    {
        foreach (var definition in definitions.Values)
            Validate(definition, catalog);
    }

    public static void Validate(
        EventDefinition definition,
        ActionCatalog catalog)
    {
        if (string.IsNullOrWhiteSpace(definition.Metadata.Id))
            throw new InvalidDataException("Event metadata.id is required.");

        if (definition.Rules.MinPlayers < 1)
            throw new InvalidDataException(
                $"Event '{definition.Metadata.Id}' minPlayers must be at least 1.");

        if (definition.Rules.MaxPlayers < definition.Rules.MinPlayers)
            throw new InvalidDataException(
                $"Event '{definition.Metadata.Id}' maxPlayers is below minPlayers.");

        foreach (var (flowName, actions) in definition.Flows)
        {
            foreach (var action in actions)
            {
                if (!catalog.IsKnown(action.Action))
                    throw new InvalidDataException(
                        $"Event '{definition.Metadata.Id}' flow '{flowName}' " +
                        $"uses unknown action '{action.Action}'.");
            }
        }

        foreach (var actionName in definition.Ai.AllowedActions)
        {
            if (!catalog.IsKnown(actionName))
                throw new InvalidDataException(
                    $"Event '{definition.Metadata.Id}' AI policy allows " +
                    $"unknown action '{actionName}'.");
        }
    }
}
