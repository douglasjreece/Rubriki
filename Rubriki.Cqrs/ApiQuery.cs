namespace Rubriki.Cqrs;

public abstract class ApiQuery : CqrsQuery, IApiQuery
{
    public abstract Task<SeedData> GetSeedData();
}
