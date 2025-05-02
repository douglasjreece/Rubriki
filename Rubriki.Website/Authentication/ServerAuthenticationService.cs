using Rubriki.Authentication;
using Rubriki.Website.CookieCqrs;

namespace Rubriki.Website.Authentication;

public class ServerAuthenticationService(CookieQuery cookieQuery, CookieCommand cookieCommand, ServerAuthenticationService.Options options) : ISecretCodeAuthenticationService
{
    public class Options
    {
        public string AdminCode { get; set; } = string.Empty;
        public string JudgeCode { get; set; } = string.Empty;
        public Guid AesEncryptionKey { get; set; } = Guid.Empty;
        public Guid AesEncryptionIV { get; set; } = Guid.Empty;
    }

    public async Task<AuthenticationResult> GetSignedInState()
    {
        var token = await cookieQuery.GetValue(cookieName);
        return await GetStateForToken(token);
    }

    public async Task<AuthenticationResult> GetStateForToken(string token)
    {
        string GetRole()
        {
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
            }
            try
            {
                var secretCode = Encrypter.DecryptStringFromBase64(token, options.AesEncryptionKey, options.AesEncryptionIV);
                return GetAuthenticatedRole(secretCode);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        var role = GetRole();
        return await Task.FromResult(new AuthenticationResult(role, token));
    }

    public async Task<AuthenticationResult> SignIn(string secretCode, bool persist)
    {
        var role = GetAuthenticatedRole(secretCode);

        if (!string.IsNullOrEmpty(role))
        {
            var token = Encrypter.EncryptStringToBase64(secretCode, options.AesEncryptionKey, options.AesEncryptionIV);
            if (persist)
            {
                await cookieCommand.SetValue(cookieName, token);
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
        await cookieCommand.SetValue(cookieName, string.Empty);
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
