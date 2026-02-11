using Microsoft.UI.Xaml.Controls;
using ClerioVision.MusicDB.ViewModels;

namespace ClerioVision.MusicDB.Views;

public sealed partial class ReportsPage : Page
{
    public ReportsViewModel ViewModel { get; }

    public ReportsPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<ReportsViewModel>();
        DataContext = ViewModel;
    }
}
