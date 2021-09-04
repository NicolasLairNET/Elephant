using Elephant.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Elephant
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new TDCTagViewModel();
        }

        private void SearchTag_Click(object sender, RoutedEventArgs e)
        {
            TDCTagViewModel vm = (TDCTagViewModel)DataContext;
            vm.Search(txtName.Text);
        }

        private void BtnUpdateData_Click(object sender, RoutedEventArgs e)
        {
            TDCTagViewModel vm = (TDCTagViewModel)DataContext;
            vm.Update();
        }

        private void SearchTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TDCTagViewModel vm = (TDCTagViewModel)DataContext;
                vm.Search(txtName.Text);
            }
        }
    }
}
