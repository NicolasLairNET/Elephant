using Elephant.Model;
namespace Elephant.Services;
public interface IJsonTdcTagService
{
    public List<TDCTag> TDCTags { get; set; }
    public Task<IEnumerable<TDCTag>> Import();
    public IEnumerable<TDCTag> GetAllListTag(string dataFilePath);
    public Task<IEnumerable<TDCTag>> Search(string value);
}
