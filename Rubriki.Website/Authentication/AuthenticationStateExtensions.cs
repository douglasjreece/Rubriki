using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Rubriki.Website.Authentication;

public static class AuthenticationStateExtensions
{
    public static string GetPrimaryRole(this AuthenticationState state)
    {
        var user = state.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleClaim != null)
            {
                return roleClaim.Value;
            }
        }
        return string.Empty;
    }

    public static bool IsAuthenticated(this AuthenticationState state)
    {
        return state.User.Identity?.IsAuthenticated == true;
    }
}
