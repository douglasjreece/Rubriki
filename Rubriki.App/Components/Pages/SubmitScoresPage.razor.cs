using Rubriki.Interfaces;
using Rubriki.UseCases;

namespace Rubriki.App.Components.Pages;

public partial class SubmitScoresPage
{
    public class Model(IScoreQuery query, SubmitScoresUseCase useCase)
    {
        public int? ScoresToSubmit { get; private set; }

        public string? SuccessMessage { get; private set; }
        public string? ErrorMessage { get; private set; }
        public bool IsWorking { get; private set; } = false;

        public async Task Load()
        {
            try
            {
                ScoresToSubmit = await query.GetScoredContestantCount();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading scores: {ex.Message}";
            }
        }

        public async Task Execute()
        {
            SuccessMessage = null;
            ErrorMessage = null;
            IsWorking = true;

            try
            {
                await useCase.Invoke();
                SuccessMessage = "Scores submitted successfully.";
                ScoresToSubmit = 0;
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
