using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Rubriki.Website.Authentication;

public class SecretCodeAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    private ClaimsPrincipal? _authenticated = null;
    private readonly Options options;

    public class Options
    {
        public string AdminCode { get; set; } = string.Empty;
        public string JudgeCode { get; set; } = string.Empty;
    }

    public SecretCodeAuthStateProvider(Options options)
    {
        this.options = options;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_authenticated ?? _anonymous));
    }

    public async Task<bool> LoginAsync(string secretCode)
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
            _authenticated = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return true;
        }

        return false;
    }

    public Task LogoutAsync()
    {
        _authenticated = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }

    public bool IsAuthenticated()
    {
        return _authenticated != null;
    }
}
