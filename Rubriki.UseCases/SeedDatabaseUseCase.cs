using Rubriki.Cqrs;
using Rubriki.Dto;

namespace Rubriki.UseCases;

public class SeedDatabaseUseCase(SetupCommand setupCommand)
{
    public async Task Invoke(SeedData seedData)
    {
        await setupCommand.EnsureDatabaseIsCreated();
        await setupCommand.Clear();
        await setupCommand.Seed(seedData);
    }
}
