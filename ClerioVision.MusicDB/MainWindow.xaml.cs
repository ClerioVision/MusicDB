using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ClerioVision.MusicDB.Views;

namespace ClerioVision.MusicDB;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Set default page
        NavView.SelectedItem = NavView.MenuItems[0];
        ContentFrame.Navigate(typeof(LibraryPage));
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
        }
        else if (args.SelectedItemContainer != null)
        {
            var tag = args.SelectedItemContainer.Tag?.ToString();
            NavigateToPage(tag);
        }
    }

    private void NavigateToPage(string? pageTag)
    {
        switch (pageTag)
        {
            case "Library":
                ContentFrame.Navigate(typeof(LibraryPage));
                break;
            case "Search":
                ContentFrame.Navigate(typeof(SearchPage));
                break;
            case "Reports":
                ContentFrame.Navigate(typeof(ReportsPage));
                break;
        }
    }
}
