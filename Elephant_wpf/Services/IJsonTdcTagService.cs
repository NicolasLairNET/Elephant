using Elephant.Model;

namespace Elephant.Services;

public interface IJsonTdcTagService
{
    public List<TDCTag> TDCTags { get; set; }
    public Task<IEnumerable<TDCTag>> Import();
    public void InitializeJsonFile(string fileName);
    public IEnumerable<TDCTag> GetAllListTag();
    public Task<IEnumerable<TDCTag>> Search(string value);
}
