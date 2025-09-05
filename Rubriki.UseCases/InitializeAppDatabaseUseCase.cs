using Rubriki.Interfaces;

namespace Rubriki.UseCases;

public class InitializeAppDatabaseUseCase(IApiQuery query, ISetupCommand command)
{
    public async Task Invoke()
    {
        var seedData = await query.GetSeedData();
        await command.EnsureDatabaseIsCreated();
        await command.Clear();
        await command.Seed(seedData);
    }
}
