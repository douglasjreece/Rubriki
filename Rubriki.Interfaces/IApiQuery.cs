namespace Rubriki.Interfaces;

public interface IApiQuery
{
    Task<SeedData> GetSeedData();
}
