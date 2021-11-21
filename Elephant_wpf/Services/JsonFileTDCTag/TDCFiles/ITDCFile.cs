using Elephant.Model;

namespace Elephant.Services.JsonFileTDCTag.TDCFiles;

public interface ITDCFile
{
    public List<TDCTag> GetTagsList();
}

