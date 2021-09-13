using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using Elephant.Model;
using Elephant.Services;

namespace Elephant.ViewModel
{
    class TDCTagViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public JsonFileTDCTagService JsonFileTDCTagService;
        public ExcelService ExcelService;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TDCTagViewModel()
        {
            JsonFileTDCTagService = new JsonFileTDCTagService();
            ExcelService = new ExcelService();
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
