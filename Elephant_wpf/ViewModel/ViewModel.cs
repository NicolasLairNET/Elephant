using System.Collections.ObjectModel;
using System.ComponentModel;
using Elephant.Model;

namespace Elephant.VM
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewModel()
        {
            TagsList = DataFile.Read();
        }

        public void Update()
        {
            TagsList = DataFile.Update();
        }

        public void Search(string tagName)
        {
            TagsList = DataFile.Search(tagName);
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
