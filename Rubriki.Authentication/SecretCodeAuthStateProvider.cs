using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Rubriki.Authentication;

public class SecretCodeAuthStateProvider(ISecretCodeAuthenticationService authenticationService) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authState = await authenticationService.GetSignedInState();
        return MakeAuthenticationState(authState.Role);
    }

    private static AuthenticationState MakeAuthenticationState(string? role)
    {
        return !string.IsNullOrEmpty(role)
            ? new AuthenticationState(MakeClaimsPrinicpal(role))
            : new AuthenticationState(MakeAnonymousClaimsPrincipal());
    }

    private static ClaimsPrincipal MakeClaimsPrinicpal(string role)
    {
        List<Claim> claims =
            [
                new(ClaimTypes.Name, "AuthenticatedUser"),
                new(ClaimTypes.Role, role),
            ];
        ClaimsIdentity identity = new(claims, "SecretCodeAuth");
        return new(identity);
    }

    private static ClaimsPrincipal MakeAnonymousClaimsPrincipal()
    {
        ClaimsIdentity identity = new();
        return new(identity);
    }
}
