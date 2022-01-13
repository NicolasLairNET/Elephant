using Elephant.ViewModel;
using Elephant_wpf;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Elephant;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<MainViewModel>();
    }
}
