using Rubriki.Authentication;

namespace Rubriki.SharedComponents;

public partial class SignInPanel
{
    public class Model(ISecretCodeAuthenticationService authenticationService)
    {
        public string SecretCode { get; set; } = "abcd";

        public async Task SignIn()
        {
            if (string.IsNullOrWhiteSpace(SecretCode))
            {
                return;
            }
            var result = await authenticationService.SignIn(SecretCode, persist:true);
            if (!result.IsAuthenticated)
            {
                SecretCode = string.Empty;
            }
        }

        public async Task SignOut()
        {
            await authenticationService.SignOut();
        }
    }
}
