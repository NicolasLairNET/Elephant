using Elephant.Model;

namespace Elephant_Services.Export;

public interface IExportService
{
    Task Export(List<Tag> tagList, string exportDestination);
}
