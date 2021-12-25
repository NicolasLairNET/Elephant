using Elephant.Model;

namespace Elephant.Services;

public interface IJsonTdcTagService
{
    public bool Import(string fileDestination);
}
