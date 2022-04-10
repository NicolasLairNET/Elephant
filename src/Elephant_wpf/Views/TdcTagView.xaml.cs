using System.Windows.Controls;
using Elephant_wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Elephant_wpf.Views;

public partial class TdcTagView : UserControl
{
    public TdcTagView()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<TdcTagViewModel>();
    }
}