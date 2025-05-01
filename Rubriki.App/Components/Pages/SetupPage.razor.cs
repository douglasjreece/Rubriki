using Rubriki.Authentication;
using Rubriki.UseCases;

namespace Rubriki.App.Components.Pages;

public partial class SetupPage
{
    public class Model(ISecretCodeAuthenticationService authenticationService, InitializeAppDatabaseUseCase useCase)
    {
        public string? SuccessMessage { get; private set; }
        public string? ErrorMessage { get; private set; }
        public bool IsWorking { get; private set; } = false;

        public async Task Execute()
        {
            SuccessMessage = null;
            ErrorMessage = null;
            IsWorking = true;

            try
            {
                var authState = await authenticationService.GetSignedInState();
                if (string.IsNullOrEmpty(authState.Token))
                {
                    ErrorMessage = "Authentication token is missing.";
                    return;
                }

                await useCase.Invoke(authState.Token);
                SuccessMessage = "Database initialized successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error initializing database: {ex.Message}";
            }
            finally
            {
                IsWorking = false;
            }
        }
    }
}
