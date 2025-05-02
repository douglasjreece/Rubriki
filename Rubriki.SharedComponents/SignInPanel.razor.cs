using Rubriki.Authentication;

namespace Rubriki.SharedComponents;

public partial class SignInPanel
{
    public class Model(ISecretCodeAuthenticationService authenticationService)
    {
        public string SecretCode { get; set; } = "abcd";

        public string ErrorMessage { get; private set; } = string.Empty;

        public async Task<bool> SignIn()
        {
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(SecretCode))
                {
                    return false;
                }
                var result = await authenticationService.SignIn(SecretCode, persist: true);
                if (!result.IsAuthenticated)
                {
                    SecretCode = string.Empty;
                }
                return result.IsAuthenticated;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public async Task SignOut()
        {
            await authenticationService.SignOut();
        }
    }
}
