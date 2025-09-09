using Rubriki.Dto;
using Rubriki.Interfaces;

namespace Rubriki.UseCases;

public class SeedDatabaseUseCase(ISetupCommand setupCommand, IStorageQuery storageQuery, IStorageCommand storageCommand)
{
    private const string seedDataFileName = "seedData.json"; // TODO: put this in an options class

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
