using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ClerioVision.MusicDB.ViewModels;

/// <summary>
/// Main view model for the application
/// </summary>
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private object? _currentPage;

    [ObservableProperty]
    private string _title = "Music Library";

    [ObservableProperty]
    private int _selectedNavigationIndex = 0;

    public ObservableCollection<NavigationItem> NavigationItems { get; } = new()
    {
        new NavigationItem { Icon = "\uE8B7", Label = "Library", Tag = "Library" },
        new NavigationItem { Icon = "\uE721", Label = "Search", Tag = "Search" },
        new NavigationItem { Icon = "\uE8A5", Label = "Reports", Tag = "Reports" },
        new NavigationItem { Icon = "\uE713", Label = "Settings", Tag = "Settings" }
    };

    [RelayCommand]
    private void NavigateTo(string pageTag)
    {
        // Navigation logic will be handled in the code-behind
        // This command will be bound to navigation items
    }
}

/// <summary>
/// Navigation item for the sidebar
/// </summary>
public class NavigationItem
{
    public string Icon { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
}
