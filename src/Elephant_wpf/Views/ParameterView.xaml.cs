using Elephant.ViewModel;
using Elephant_wpf;
using Microsoft.Extensions.DependencyInjection;

namespace Elephant.Views;

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
