namespace Rubriki.Authentication;

public interface ISecretCodeAuthenticationService
{
    Task<AuthenticationResult> GetSignedInState();
    Task<AuthenticationResult> GetStateForToken(string token);
    Task<AuthenticationResult> SignIn(string secretCode, bool persist);
    Task SignOut();
}
