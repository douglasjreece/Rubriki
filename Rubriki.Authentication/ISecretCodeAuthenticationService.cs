namespace Rubriki.Authentication;

public interface ISecretCodeAuthenticationService
{
    Task<string> GetAuthenticatedRole();
    Task<AuthenticationResult> SignIn(string secretCode, bool persist);
    Task SignOut();
}
