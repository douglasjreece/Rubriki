using Rubriki.Website.Authentication;

namespace Rubriki.Website.Components.Pages;

public partial class Home
{
    public class Model(SecretCodeAuthStateProvider provider)
    {
        public string SecretCode { get; set; } = "abcd";

        public async Task SignIn()
        {
            if (string.IsNullOrWhiteSpace(SecretCode))
            {
                return;
            }
            var success = await provider.LoginAsync(SecretCode);
            if (!success)
            {
                SecretCode = string.Empty;
            }
        }
    }
}
