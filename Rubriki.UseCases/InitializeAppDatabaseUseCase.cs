using Rubriki.Cqrs;

namespace Rubriki.UseCases;

public class InitializeAppDatabaseUseCase(ApiQuery query, SetupCommand command, AppUseCaseOptions options)
{
    public async Task Invoke()
    {
        var seedData = await query.GetSeedData();
        await command.EnsureDatabaseIsCreated();
        await command.Clear();
        await command.Seed(seedData);
    }
}
