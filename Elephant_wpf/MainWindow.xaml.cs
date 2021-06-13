using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            var el = new ElephantJson();
            List<ElephantLibrary.Point> result = el.SearchDataJson(txtName.Text);
            DGridResult.ItemsSource = result;
        }
    }
}
