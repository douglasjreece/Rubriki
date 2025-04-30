using Rubriki.UseCases;

namespace Rubriki.App.Components.Pages;

public partial class SetupPage
{
    public class Model(InitializeAppDatabaseUseCase useCase)
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
                await useCase.Invoke();
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
