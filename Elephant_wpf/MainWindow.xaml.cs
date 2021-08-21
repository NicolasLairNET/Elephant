
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using ElephantLibrary;
using Elephant_wpf.ViewModel;

namespace Elephant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataFile Data;

        public MainWindow()
        {
            DataContext = new ViewModel();
            //Data = new DataFile();
            //DGridResult.ItemsSource = Data.Tags;
        }

        private void SearchTag_Click(object sender, RoutedEventArgs e)
        {
            DGridResult.ItemsSource = Data.Search(txtName.Text);
        }

        private void BtnUpdateData_Click(object sender, RoutedEventArgs e)
        {
            Data.UpdateData();
        }
    }
}
