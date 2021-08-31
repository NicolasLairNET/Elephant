using System.Windows;
using System.Windows.Input;
using Elephant.VM;

namespace Elephant
{
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

        private void SearchTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel vm = (ViewModel)DataContext;
                vm.Search(txtName.Text);
            }
        }
    }
}
