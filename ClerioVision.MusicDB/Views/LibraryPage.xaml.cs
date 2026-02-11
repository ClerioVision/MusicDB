using Microsoft.UI.Xaml.Controls;
using ClerioVision.MusicDB.ViewModels;

namespace ClerioVision.MusicDB.Views;

public sealed partial class LibraryPage : Page
{
    public LibraryViewModel ViewModel { get; }

    public LibraryPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<LibraryViewModel>();
        DataContext = ViewModel;
    }
}
