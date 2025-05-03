using Rubriki.Cqrs;
using Rubriki.Dto;

namespace Rubriki.UseCases;

public class SeedDatabaseUseCase(SetupCommand setupCommand, StorageQuery storageQuery, StorageCommand storageCommand)
{
    private const string seedDataFileName = "seedData.json";

    public async Task Invoke(SeedData seedData)
    {
        await setupCommand.EnsureDatabaseIsCreated();
        await setupCommand.Clear();
        await setupCommand.Seed(seedData);
        await storageCommand.Store(seedDataFileName, seedData);
    }

    public async Task<SeedData?> GetCurrentSeedData()
    {
        try
        {
            return await storageQuery.GetOrDefault<SeedData>(seedDataFileName);
        }
        catch
        {
            return null;
        }
    }
}
