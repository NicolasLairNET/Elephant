using Elephant.Model;

namespace Elephant.Services.TagDataFileManagerService.TDCFiles;

public interface ITDCFile
{
    public List<TDCTag> GetTagsList();
}

