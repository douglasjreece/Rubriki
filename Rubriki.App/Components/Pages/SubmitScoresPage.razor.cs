using Rubriki.UseCases;

namespace Rubriki.App.Components.Pages;

public partial class SubmitScoresPage
{
    public class Model(SubmitScoresUseCase useCase)
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
                SuccessMessage = "Scores submitted successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error submitting scores: {ex.Message}";
            }
            finally
            {
                IsWorking = false;
            }
        }
    }
}
