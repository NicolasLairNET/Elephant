using Elephant.Model;

namespace Elephant.Services.Export;

public interface IExportService
{
    Task Export(List<Tag> tagList);
}
