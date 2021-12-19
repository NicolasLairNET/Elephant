using Elephant.Model;

namespace Elephant.Services;

internal interface IJsonTdcTagService
{
    public ObservableCollection<TDCTag> GetTDCTags();
    public ObservableCollection<TDCTag> Import();
    public ObservableCollection<TDCTag> Search(string value);
}
