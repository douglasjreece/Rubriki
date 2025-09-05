
namespace Rubriki.Interfaces;

public interface IStorageCommand
{
    Task Remove(string fileName);
    Task Store<T>(string fileName, T content) where T : class;
}
