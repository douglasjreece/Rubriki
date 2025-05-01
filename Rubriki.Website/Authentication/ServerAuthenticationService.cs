using Rubriki.Authentication;

namespace Rubriki.Website.Authentication;

public class ServerAuthenticationService(CookieService cookieService, ServerAuthenticationService.Options options) : ISecretCodeAuthenticationService
{
    public class Options
    {
        public string AdminCode { get; set; } = string.Empty;
        public string JudgeCode { get; set; } = string.Empty;
        public Guid AesEncryptionKey { get; set; } = Guid.Empty;
        public Guid AesEncryptionIV { get; set; } = Guid.Empty;
    }

    public async Task<string> GetAuthenticatedRole()
    {
        var token = await cookieService.GetValue(cookieName);
        if (string.IsNullOrEmpty(token))
        {
            return string.Empty;
        }
        var secretCode = Encrypter.DecryptStringFromBase64(token, options.AesEncryptionKey, options.AesEncryptionIV);
        return GetAuthenticatedRole(secretCode);
    }

    public async Task<AuthenticationResult> SignIn(string secretCode, bool persist)
    {
        var role = GetAuthenticatedRole(secretCode);

        if (!string.IsNullOrEmpty(role))
        {
            var token = Encrypter.EncryptStringToBase64(secretCode, options.AesEncryptionKey, options.AesEncryptionIV);
            if (persist)
            {
                await cookieService.SetValue(cookieName, token);
            }
            return new AuthenticationResult(role, token);
        }
        else
        {
            return AuthenticationResult.Empty;
        }
    }

    public async Task SignOut()
    {
        await cookieService.SetValue(cookieName, string.Empty);
    }

    private string cookieName = "token";

    private string GetAuthenticatedRole(string secretCode)
    {
        return secretCode switch
        {
            var code when code == options.AdminCode => RoleName.Admin,
            var code when code == options.JudgeCode => RoleName.Judge,
            _ => string.Empty,
        };
    }
}
