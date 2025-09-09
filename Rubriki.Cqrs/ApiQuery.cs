namespace Rubriki.Cqrs;

public abstract class ApiQuery : IApiQuery
{
    public abstract Task<SeedData> GetSeedData();
}
