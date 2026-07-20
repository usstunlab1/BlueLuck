namespace BlueLuck.Actions;

public readonly record struct ActionResult(bool Success, string Message)
{
    public static ActionResult Ok(string message = "Action completed.") => new(true, message);
    public static ActionResult Fail(string message) => new(false, message);
}
