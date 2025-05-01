using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Rubriki.Website.Authentication;

public class SecretCodeAuthStateProvider(SecretCodeAuthStateProvider.Options options, CookieService cookieService) : AuthenticationStateProvider
{
    public class Options
    {
        public string AdminCode { get; set; } = string.Empty;
        public string JudgeCode { get; set; } = string.Empty;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var secretCode = await cookieService.GetValue("SecretCode");
        return MakeAuthenticationState(secretCode);
    }

    public async Task<bool> LoginAsync(string secretCode)
    {
        var state = MakeAuthenticationState(secretCode);
        if (state.User.Claims.Any())
        {
            // Save the secret code in a cookie
            await cookieService.SetValue("SecretCode", secretCode);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        await cookieService.SetValue("SecretCode", string.Empty);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public AuthenticationState MakeAuthenticationState(string secretCode)
    {
        if (secretCode == options.JudgeCode || secretCode == options.AdminCode)
        {
            // Create claims for the authenticated user
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "AuthenticatedUser"),
                    new Claim(ClaimTypes.Role, secretCode == options.AdminCode ? "Admin" : "Judge"),
                };

            var identity = new ClaimsIdentity(claims, "SecretCodeAuth");
            var authenticated = new ClaimsPrincipal(identity);

            return new(authenticated);
        }
        else
        {
            // Create an anonymous user
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new(anonymous);
        }
    }
}
