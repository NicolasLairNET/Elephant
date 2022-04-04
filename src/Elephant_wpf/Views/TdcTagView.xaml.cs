using System.Windows.Controls;
using Elephant.ViewModel;
using Elephant_wpf;
using Microsoft.Extensions.DependencyInjection;

namespace Elephant.Views;

public partial class TdcTagView : UserControl
{
    public TdcTagView()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<TdcTagViewModel>();
    }
}