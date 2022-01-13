using Elephant.ViewModel;
using Elephant_wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Elephant.Views
{
    /// <summary>
    /// Logique d'interaction pour ParameterView.xaml
    /// </summary>
    public partial class ParameterView
    {
        public ParameterView()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<ParameterViewModel>();
        }
    }
}
