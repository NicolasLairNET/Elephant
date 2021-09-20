using Elephant.ViewModel;
using System.Windows;


namespace Elephant
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new TDCTagViewModel();
        }
    }
}
