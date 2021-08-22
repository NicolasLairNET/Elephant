using System.Windows;
using Elephant.VM;

namespace Elephant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new ViewModel();
        }

        private void SearchTag_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)DataContext;
            vm.Search(txtName.Text);
        }

        private void BtnUpdateData_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)DataContext;
            vm.Update();
        }
    }
}
