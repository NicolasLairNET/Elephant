using Elephant.Messages;
using Elephant.Model;
using Elephant.Services;
using Elephant.Services.ConfigFileManagerService;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Elephant.ViewModel;

public class TdcTagViewModel : ObservableRecipient, IViewModel
{
    private readonly IJsonTdcTagService JsonService;
    private readonly IExportService ExportService;
    private readonly IConfigFileManagerService ConfigFileService;
    private IEnumerable<TDCTag> tagsList;
    private string tagToSearch;
    public IAsyncRelayCommand ImportCommand { get; }
    public IAsyncRelayCommand ExportCommand { get; }
    public IAsyncRelayCommand SearchCommand { get; }
    public IRelayCommand UpdateViewCommand { get;}

    public IEnumerable<TDCTag> TagsList
    {
        get => tagsList;
        set => SetProperty(ref tagsList, value);
    }

    public TdcTagViewModel(IExportService exportService,
        IJsonTdcTagService jsonService,
        IConfigFileManagerService configFileManagerService)
    {
        ImportCommand = new AsyncRelayCommand(Import);
        ExportCommand = new AsyncRelayCommand(Export);
        SearchCommand = new AsyncRelayCommand(Search);

        JsonService = jsonService;
        ExportService = exportService;
        ConfigFileService = configFileManagerService;
        tagsList = JsonService.TDCTags;
        tagToSearch = "";

        UpdateViewCommand = new RelayCommand(SendMessage);
        OnActivated();
    }

    public string TagToSearch
    {
        get => tagToSearch;
        set
        {
            tagToSearch = value;
            SearchCommand.Execute(null);
        }
    }

    private async Task Import()
    {
        TagsList = await JsonService.Import().ConfigureAwait(false);
    }

    private async Task Search()
    {
        TagsList = await JsonService.Search(TagToSearch).ConfigureAwait(false);
    }

    private async Task Export()
    {
        await ExportService.Export(TagsList.ToList()).ConfigureAwait(false);
    }

    public void SendMessage()
    {
        Messenger.Send(new ViewModelChangedMessage("ParameterViewModel"));
    }

    /// <summary>
    /// Activates the listening of the message which notifies the change of the data file.
    /// When the file is changed the list of tags is updated.
    /// </summary>
    protected override void OnActivated()
    {
        Messenger.Register<TdcTagViewModel, DataFileChangedMessage>(this, (r, m) =>
        {
            r.TagsList = JsonService.GetAllListTag(m.Value);
            // update tags list in the jsonTdcfileService
            JsonService.TDCTags = r.TagsList.ToList();
        });
    }
}