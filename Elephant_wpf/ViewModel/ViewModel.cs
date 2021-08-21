using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ElephantLibrary;
using System.Linq;
using System.Text;


using System.Threading.Tasks;

namespace Elephant_wpf.ViewModel
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
    }
}
