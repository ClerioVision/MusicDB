using Microsoft.UI.Xaml.Controls;
using ClerioVision.MusicDB.ViewModels;

namespace ClerioVision.MusicDB.Views;

public sealed partial class SearchPage : Page
{
    public SearchViewModel ViewModel { get; }

    public SearchPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<SearchViewModel>();
        DataContext = ViewModel;
    }
}
