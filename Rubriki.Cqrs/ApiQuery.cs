using Rubriki.Dto;

namespace Rubriki.Cqrs;

public abstract class ApiQuery : CqrsQuery
{
    public abstract Task<SeedData> GetSeedData();
}
