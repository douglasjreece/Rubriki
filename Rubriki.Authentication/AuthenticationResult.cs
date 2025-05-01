namespace Rubriki.Authentication;

public record AuthenticationResult(string? Role, string? Token)
{
    public bool IsAuthenticated => !string.IsNullOrEmpty(Role);

    public static readonly AuthenticationResult Empty = new(null, null);
}
