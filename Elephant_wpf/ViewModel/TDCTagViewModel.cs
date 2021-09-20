using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using Elephant.Model;
using Elephant.Services;
using Elephant.ViewModel.Commands;

namespace Elephant.ViewModel
{
    class TDCTagViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public JsonFileTDCTagService JsonFileTDCTagService;
        public ExcelService ExcelService;
        public SearchCommand SearchCommand { get; private set; }
        public ImportCommand ImportCommand { get; private set; }
        public ExportCommand ExportCommand { get; private set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TDCTagViewModel()
        {
            JsonFileTDCTagService = new JsonFileTDCTagService();
            ExcelService = new ExcelService();
            ImportCommand = new ImportCommand(Update);
            SearchCommand = new SearchCommand(Search);
            ExportCommand = new ExportCommand(Export);
            TagsList = JsonFileTDCTagService.GetTDCTags();
        }

        public void Update()
        {
            TagsList = JsonFileTDCTagService.Update();
        }

        public void Search(string tagName)
        {
            TagsList = JsonFileTDCTagService.Search(tagName);
        }

        public void Export(DataGrid data)
        {
            ExcelService.Export(data);
        }

        private ObservableCollection<TDCTag> _tagsList;
        public ObservableCollection<TDCTag> TagsList
        {
            get => _tagsList;
            set
            {
                _tagsList = value;
                OnPropertyChanged(nameof(TagsList));
            }
        }
    }
}
