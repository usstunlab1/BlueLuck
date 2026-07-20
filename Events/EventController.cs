using BlueLuck.Actions;

namespace BlueLuck.Events;

public sealed class EventController : IDisposable
{
    readonly IReadOnlyDictionary<string, EventDefinition> _definitions;
    readonly Dictionary<string, EventSession> _sessions =
        new(StringComparer.OrdinalIgnoreCase);
    readonly ActionExecutor _executor;
    readonly Action<string> _log;

    public EventController(
        IReadOnlyDictionary<string, EventDefinition> definitions,
        ActionExecutor executor,
        Action<string> log)
    {
        _definitions = definitions;
        _executor = executor;
        _log = log;
    }

    public EventSession Start(string eventId, ulong requestedBy = 0)
    {
        if (!_definitions.TryGetValue(eventId, out var definition))
            throw new KeyNotFoundException($"Unknown event '{eventId}'.");

        if (!definition.Metadata.Enabled)
            throw new InvalidOperationException(
                $"Event '{eventId}' is disabled.");

        if (_sessions.ContainsKey(eventId))
            throw new InvalidOperationException(
                $"Event '{eventId}' already has an active session.");

        var session = new EventSession(definition);
        _sessions.Add(eventId, session);

        ExecuteFlow(session, "start", requestedBy);
        _log(
            $"[BlueLuck] Started event '{eventId}' " +
            $"session={session.SessionId}.");

        return session;
    }

    public void ExecuteFlow(
        EventSession session,
        string flowName,
        ulong requestedBy = 0)
    {
        if (!session.Definition.Flows.TryGetValue(flowName, out var actions))
            return;

        var context = new ActionContext
        {
            EventId = session.Definition.Metadata.Id,
            SessionId = session.SessionId,
            State = session.State,
            RequestedBy = requestedBy
        };

        foreach (var request in actions)
        {
            var result = _executor.Execute(request, context);
            if (!result.Success)
                throw new InvalidOperationException(
                    $"Event '{session.Definition.Metadata.Id}' flow " +
                    $"'{flowName}' failed: {result.Message}");
        }
    }

    public bool End(string eventId, ulong requestedBy = 0)
    {
        if (!_sessions.Remove(eventId, out var session))
            return false;

        ExecuteFlow(session, "end", requestedBy);
        _log(
            $"[BlueLuck] Ended event '{eventId}' " +
            $"session={session.SessionId}.");

        return true;
    }

    public void Dispose()
    {
        foreach (var eventId in _sessions.Keys.ToArray())
        {
            try
            {
                End(eventId);
            }
            catch
            {
                // Shutdown is best effort.
            }
        }
    }
}
