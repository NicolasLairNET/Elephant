
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
            using OpenFileDialog openFileDialog = new();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "EB files (*.EB)|*.EB|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataFile.CreateDataFile(openFileDialog.FileNames);
            }
            System.Windows.MessageBox.Show("Import terminé");
        }
    }
}
