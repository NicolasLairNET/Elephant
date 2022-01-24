using Elephant.Model;

namespace Elephant.Services.ExportService;

public interface IExportService
{
    Task Export(List<TDCTag> tagList);
}
