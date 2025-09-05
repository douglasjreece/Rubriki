

namespace Rubriki.Interfaces;

public interface ISetupCommand
{
    Task Clear();
    Task EnsureDatabaseIsCreated();
    Task Seed(SeedData seedData);
}
