namespace BlueLuck.Actions;

public static class BuiltInActionHandlers
{
    public static void Register(ActionExecutor executor)
    {
        executor.Register("announce", (request, _, game) =>
            game.Announce(Required(request, "message")));

        executor.Register("notification", (request, context, game) =>
            game.Notify(context.RequestedBy, Required(request, "message")));

        executor.Register("timer.start", (request, context, _) =>
        {
            var id = Optional(request, "timerId", "main");
            var seconds = ParseInt(Required(request, "seconds"), 1, 86400);
            context.State[$"timer:{id}:endsUtc"] = DateTime.UtcNow.AddSeconds(seconds);
            return ActionResult.Ok($"Timer '{id}' started for {seconds} seconds.");
        });

        executor.Register("timer.stop", (request, context, _) =>
        {
            var id = Optional(request, "timerId", "main");
            context.State.Remove($"timer:{id}:endsUtc");
            return ActionResult.Ok($"Timer '{id}' stopped.");
        });

        executor.Register("score.add", (request, context, _) =>
        {
            var key = Optional(request, "key", "global");
            var amount = ParseInt(Required(request, "amount"), -1_000_000, 1_000_000);
            var stateKey = $"score:{key}";
            var current = context.State.TryGetValue(stateKey, out var value) &&
                          value is int score ? score : 0;
            context.State[stateKey] = current + amount;
            return ActionResult.Ok($"Score '{key}' is now {current + amount}.");
        });

        executor.Register("score.reset", (request, context, _) =>
        {
            var key = Optional(request, "key", "global");
            context.State[$"score:{key}"] = 0;
            return ActionResult.Ok($"Score '{key}' reset.");
        });

        executor.Register("phase.set", (request, context, _) =>
        {
            context.State["phase"] = Required(request, "phase");
            return ActionResult.Ok($"Phase changed to '{context.State["phase"]}'.");
        });

        executor.Register("counter.add", (request, context, _) =>
        {
            var key = Required(request, "key");
            var amount = ParseInt(Required(request, "amount"), -1_000_000, 1_000_000);
            var stateKey = $"counter:{key}";
            var current = context.State.TryGetValue(stateKey, out var value) &&
                          value is int count ? count : 0;
            context.State[stateKey] = current + amount;
            return ActionResult.Ok($"Counter '{key}' is now {current + amount}.");
        });

        executor.Register("condition.check", (request, context, _) =>
        {
            var key = Required(request, "key");
            var expected = Required(request, "equals");
            var actual = context.State.TryGetValue(key, out var value)
                ? value?.ToString() ?? ""
                : "";

            return string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase)
                ? ActionResult.Ok($"Condition '{key}' matched.")
                : ActionResult.Fail(
                    $"Condition '{key}' did not match. Actual='{actual}'.");
        });

        executor.Register("event.complete", (_, context, _) =>
        {
            context.State["status"] = "completed";
            return ActionResult.Ok("Event marked completed.");
        });

        executor.Register("event.cancel", (_, context, _) =>
        {
            context.State["status"] = "cancelled";
            return ActionResult.Ok("Event marked cancelled.");
        });

        executor.Register("spawn.request", (request, context, game) =>
            game.SpawnRequest(context, request.Params));

        executor.Register("effect.request", (request, context, game) =>
            game.EffectRequest(context, request.Params));

        executor.Register("zone.radius.set", (request, context, game) =>
            game.SetZoneRadius(
                context,
                float.Parse(
                    Required(request, "radius"),
                    System.Globalization.CultureInfo.InvariantCulture)));

        executor.Register("pvp.set", (request, context, game) =>
            game.SetPvp(context, bool.Parse(Required(request, "enabled"))));

        executor.Register("heal.request", (request, context, game) =>
            game.Heal(context, Optional(request, "target", "session")));
    }

    static string Required(ActionRequest request, string key) =>
        request.Params.TryGetValue(key, out var value) &&
        !string.IsNullOrWhiteSpace(value)
            ? value.Trim()
            : throw new InvalidOperationException($"Missing parameter '{key}'.");

    static string Optional(ActionRequest request, string key, string fallback) =>
        request.Params.TryGetValue(key, out var value) &&
        !string.IsNullOrWhiteSpace(value)
            ? value.Trim()
            : fallback;

    static int ParseInt(string value, int minimum, int maximum) =>
        Math.Clamp(
            int.Parse(value, System.Globalization.CultureInfo.InvariantCulture),
            minimum,
            maximum);
}
