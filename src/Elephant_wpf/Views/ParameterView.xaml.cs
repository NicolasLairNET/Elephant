using Elephant_wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Elephant_wpf.Views;

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
