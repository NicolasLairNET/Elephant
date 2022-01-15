using System.Windows.Input;

namespace Elephant.ViewModel
{
    public interface IViewModel
    {
        ICommand UpdateViewCommand { get ;}
        public void SendMessage();
    }
}
