using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Forms;
using ElephantLibrary;
using Point = ElephantLibrary.Point;

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

        private void btnUpdateData_Click(object sender, RoutedEventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "EB files (*.EB)|*.EB|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.Multiselect = true;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CreateDataFile(openFileDialog.FileNames);
                }
                System.Windows.MessageBox.Show("Import terminé");
            }
        }

        static void CreateDataFile(string[] fileList)
        {
            List<Point> pointList = new List<Point>();
            string[] fileLines;

            foreach (string fileName in fileList)
            {
                fileLines = File.ReadAllLines(fileName);
                List<ElephantLibrary.Point> EBList = ReadEB(fileLines);
                pointList.AddRange(EBList);
            }
            string result = JsonSerializer.Serialize(pointList);
            File.WriteAllText(@"D:\Nico\Nico\DATA.json", result);
        }

        private static List<ElephantLibrary.Point> ReadEB(string[] lines)
        {
            List<ElephantLibrary.Point> pointList = new List<ElephantLibrary.Point>();
            string point = null;
            string parameter = null;
            string value = null;
            string origin = null;

            foreach (string l in lines)
            {
                string line = l.Trim();
                if (line.Substring(0, 2) == "&N")
                {
                    continue;
                }

                if (line.Substring(0, 1) == "{")
                {
                    point = line.Substring(15, line.IndexOf('(') - 15);
                    continue;
                }
                else if (line.Substring(0, 2) == "&T")
                {
                    value = line.Substring(3, line.Length - 3);
                    var p = new ElephantLibrary.Point()
                    {
                        Name = point,
                        Parameter = "PNTTYPE",
                        Value = value,
                        Origin = "EB"
                    };
                    pointList.Add(p);
                }
                else
                {
                    string[] element = line.Split("=");
                    var p = new ElephantLibrary.Point()
                    {
                        Name = point,
                        Parameter = element[0].Trim(),
                        Value = element[1].Replace("\"", "").Trim(),
                        Origin = "EB"
                    };
                    pointList.Add(p);
                }
            }

            return pointList;
        }
    }
}
