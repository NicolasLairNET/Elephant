using System.Windows.Controls;
using Elephant.ViewModel;

namespace Elephant.Views;

public partial class TdcTagView : UserControl
{
    public TdcTagView()
    {
        InitializeComponent();
        DataContext = new TdcTagViewModel();
    }
}

