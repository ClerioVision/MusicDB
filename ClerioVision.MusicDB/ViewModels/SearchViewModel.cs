using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClerioVision.MusicDB.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ClerioVision.MusicDB.ViewModels;

/// <summary>
/// View model for the Search page
/// </summary>
public partial class SearchViewModel : ObservableObject
{
    private readonly SearchService _searchService;
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private int _selectedSearchType = 0; // 0: Albums by Artist, 1: Tracks by Artist, 2: Albums by Track, 3: Tracks by Album

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private bool _isSearching = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private int _resultCount = 0;

    public ObservableCollection<AlbumSearchResult> AlbumResults { get; } = new();
    public ObservableCollection<TrackSearchResult> TrackResults { get; } = new();
    public ObservableCollection<string> ArtistSuggestions { get; } = new();
    public ObservableCollection<string> AlbumSuggestions { get; } = new();

    public SearchViewModel(SearchService searchService, DatabaseService databaseService)
    {
        _searchService = searchService;
        _databaseService = databaseService;

        _ = LoadSuggestionsAsync();
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
            return;

        IsSearching = true;
        ErrorMessage = string.Empty;
        AlbumResults.Clear();
        TrackResults.Clear();

        try
        {
            switch (SelectedSearchType)
            {
                case 0: // Albums by Artist
                    var albums = await _searchService.SearchAlbumsByArtistAsync(SearchQuery);
                    foreach (var album in albums)
                        AlbumResults.Add(album);
                    ResultCount = albums.Count;
                    break;

                case 1: // Tracks by Artist
                    var tracksByArtist = await _searchService.SearchTracksByArtistAsync(SearchQuery);
                    foreach (var track in tracksByArtist)
                        TrackResults.Add(track);
                    ResultCount = tracksByArtist.Count;
                    break;

                case 2: // Albums by Track Title
                    var albumsByTrack = await _searchService.SearchAlbumsByTrackTitleAsync(SearchQuery);
                    foreach (var album in albumsByTrack)
                        AlbumResults.Add(album);
                    ResultCount = albumsByTrack.Count;
                    break;

                case 3: // Tracks by Album
                    var tracksByAlbum = await _searchService.SearchTracksByAlbumAsync(SearchQuery);
                    foreach (var track in tracksByAlbum)
                        TrackResults.Add(track);
                    ResultCount = tracksByAlbum.Count;
                    break;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Search error: {ex.Message}";
        }
        finally
        {
            IsSearching = false;
        }
    }

    [RelayCommand]
    private void ClearSearch()
    {
        SearchQuery = string.Empty;
        AlbumResults.Clear();
        TrackResults.Clear();
        ResultCount = 0;
        ErrorMessage = string.Empty;
    }

    private async Task LoadSuggestionsAsync()
    {
        try
        {
            var artists = await _databaseService.GetArtistNamesAsync();
            var albums = await _databaseService.GetAlbumTitlesAsync();

            ArtistSuggestions.Clear();
            AlbumSuggestions.Clear();

            foreach (var artist in artists)
                ArtistSuggestions.Add(artist);

            foreach (var album in albums)
                AlbumSuggestions.Add(album);
        }
        catch
        {
            // Silently fail - suggestions are optional
        }
    }

    partial void OnSelectedSearchTypeChanged(int value)
    {
        // Clear results when search type changes
        ClearSearch();
    }
}
