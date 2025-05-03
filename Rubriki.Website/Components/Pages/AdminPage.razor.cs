using Rubriki.Cqrs;
using Rubriki.Dto;
using Rubriki.UseCases;
using System.Text.Json;

namespace Rubriki.Website.Components.Pages
{
    public partial class AdminPage
    {
        public class Model(SeedDatabaseUseCase seedDataUseCase, ScoreCommand scoreCommand)
        {
            public string SeedDataJson { get; set; } = string.Empty;

            public string ErrorMessage { get; private set; } = string.Empty;
            public string SuccessMessage { get; private set; } = string.Empty;
            public bool IsWorking { get; private set; } = false;

            public async Task Load()
            {
                var currentSeedData = await seedDataUseCase.GetCurrentSeedData();
                if (currentSeedData != null)
                {
                    SeedDataJson = JsonSerializer.Serialize(currentSeedData, new JsonSerializerOptions { WriteIndented = true });
                }
                else
                {
                    SeedDataJson = string.Empty;
                }
            }

            public async Task ClearScores()
            {
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;
                IsWorking = true;
                try
                {
                    await scoreCommand.ClearScores();
                    SuccessMessage = "Scores cleared successfully.";
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

            public async Task InitializeDatabase()
            {
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                IsWorking = true;
                try
                {
                    var seedData = JsonSerializer.Deserialize<SeedData>(SeedDataJson) ?? throw new InvalidOperationException("Seed Data is invalid.");
                    await seedDataUseCase.Invoke(seedData);
                    SuccessMessage = "Database initialized successfully.";
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
