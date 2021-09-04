using System.Collections.ObjectModel;
using System.ComponentModel;
using Elephant.Model;
using Elephant.Services;

namespace Elephant.ViewModel
{
    class TDCTagViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public JsonFileTDCTagService JsonFileTDCTagService;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TDCTagViewModel()
        {
            JsonFileTDCTagService = new JsonFileTDCTagService();
            TagsList = JsonFileTDCTagService.GetProducts();
        }

        public void Update()
        {
            TagsList = JsonFileTDCTagService.Update();
        }

        public void Search(string tagName)
        {
            TagsList = JsonFileTDCTagService.Search(tagName);
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
