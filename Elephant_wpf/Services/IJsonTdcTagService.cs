using Elephant.Model;

namespace Elephant.Services;

internal interface IJsonTdcTagService
{
    public List<TDCTag> GetTDCTags();
    public List<TDCTag> Import();
    public List<TDCTag> Search(string value);
}
