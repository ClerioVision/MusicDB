using Microsoft.UI.Xaml.Controls;
using ClerioVision.MusicDB.ViewModels;

namespace ClerioVision.MusicDB.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; }

    public SettingsPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<SettingsViewModel>();
        DataContext = ViewModel;
    }
}
