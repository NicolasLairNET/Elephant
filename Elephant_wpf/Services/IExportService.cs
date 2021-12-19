using Elephant.Model;

namespace Elephant.Services;

public interface IExportService
{
    Task Export(List<TDCTag> tagList);
}
