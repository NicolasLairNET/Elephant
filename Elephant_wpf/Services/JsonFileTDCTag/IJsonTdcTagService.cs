using Elephant.Model;

namespace Elephant.Services.JsonFileTDCTag;
public interface IJsonTdcTagService
{
    public List<TDCTag> TDCTags { get; set; }
    //public Task<List<TDCTag>> Import();
    public List<TDCTag> GetTagsToDataFile(string dataFilePath);
    public Task<List<TDCTag>> Search(string value);
    public Task GetTagsAsync(string filePath, IProgress<(string, List<TDCTag>)> p);
    public string[] GetPathList();
    public void WriteData(List<TDCTag> newValue);
}
