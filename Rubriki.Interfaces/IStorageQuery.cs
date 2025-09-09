namespace Rubriki.Interfaces;

public interface IStorageQuery
{
    Task<T?> GetOrDefault<T>(string fileName) where T : class;
}
