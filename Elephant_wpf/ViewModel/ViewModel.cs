using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
            TagsList = new DataFile().Tags;
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

        public void Update()
        {
            DataFile.UpdateData();
            TagsList = new DataFile().Tags;
        }

        public void Search(string tagName)
        {
            ObservableCollection<TDCTag> data = new DataFile().Tags;

            if (tagName != "")
            {
                TagsList = new ObservableCollection<TDCTag>(
                from TDCTag in data
                where TDCTag.Name == tagName
                select TDCTag);
                return;
            }

            TagsList = data;
        }
    }
}
