using Rubriki.UseCases;

namespace Rubriki.Website.Components.Pages
{
    public partial class AdminPage
    {
        public class Model(SeedDatabaseUseCase seedDataUseCase)
        {
            public string ErrorMessage { get; private set; } = string.Empty;
            public string SuccessMessage { get; private set; } = string.Empty;
            public bool IsWorking { get; private set; } = false;

            public async Task ResetData()
            {
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                IsWorking = true;
                try
                {
                    await seedDataUseCase.Invoke();
                    SuccessMessage = "Data reset successfully.";
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }
                finally
                {
                    IsWorking = false;
                }
            }
        }
    }
}
