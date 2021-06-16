
using System.Windows;
using System.Windows.Forms;
using ElephantLibrary;

namespace Elephant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
        }

        private void SearchTag_Click(object sender, RoutedEventArgs e)
        {
            DGridResult.ItemsSource = DataFile.Search(txtName.Text);
        }

        private void BtnUpdateData_Click(object sender, RoutedEventArgs e)
        {
            DataFile.UpdateData();
        }
    }
}
