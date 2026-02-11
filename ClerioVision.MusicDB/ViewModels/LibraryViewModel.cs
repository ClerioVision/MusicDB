using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClerioVision.MusicDB.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace ClerioVision.MusicDB.ViewModels;

/// <summary>
/// View model for the Library page
/// </summary>
public partial class LibraryViewModel : ObservableObject
{
    private readonly Mp3ScannerService _scannerService;
    private readonly DatabaseService _databaseService;
    private CancellationTokenSource? _cancellationTokenSource;

    [ObservableProperty]
    private string _selectedFolder = string.Empty;

    [ObservableProperty]
    private bool _isScanning = false;

    [ObservableProperty]
    private int _scanProgress = 0;

    [ObservableProperty]
    private string _scanStatus = "Ready to scan";

    [ObservableProperty]
    private int _totalArtists = 0;

    [ObservableProperty]
    private int _totalAlbums = 0;

    [ObservableProperty]
    private int _totalTracks = 0;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public LibraryViewModel(Mp3ScannerService scannerService, DatabaseService databaseService)
    {
        _scannerService = scannerService;
        _databaseService = databaseService;

        _scannerService.ProgressChanged += OnScanProgressChanged;
        _scannerService.ErrorOccurred += OnScanError;

        _ = LoadLibraryStatsAsync();
    }

    [RelayCommand]
    private async Task SelectFolderAsync()
    {
        var folderPicker = new FolderPicker();
        
        // Get the current window handle for the picker
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

        folderPicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
        folderPicker.FileTypeFilter.Add("*");

        var folder = await folderPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            SelectedFolder = folder.Path;
        }
    }

    [RelayCommand(CanExecute = nameof(CanStartScan))]
    private async Task StartScanAsync()
    {
        if (string.IsNullOrEmpty(SelectedFolder))
            return;

        IsScanning = true;
        ErrorMessage = string.Empty;
        _cancellationTokenSource = new CancellationTokenSource();

        try
        {
            // Scan folder for MP3 files
            var mp3Files = await _scannerService.ScanFolderAsync(SelectedFolder, _cancellationTokenSource.Token);

            // Import into database
            ScanStatus = "Importing to database...";
            var importedCount = await _databaseService.ImportMp3FilesAsync(mp3Files);

            ScanStatus = $"Scan complete! Imported {importedCount} tracks.";
            
            // Refresh stats
            await LoadLibraryStatsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error during scan: {ex.Message}";
            ScanStatus = "Scan failed";
        }
        finally
        {
            IsScanning = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private bool CanStartScan() => !IsScanning && !string.IsNullOrEmpty(SelectedFolder);

    [RelayCommand(CanExecute = nameof(CanCancelScan))]
    private void CancelScan()
    {
        _cancellationTokenSource?.Cancel();
        ScanStatus = "Cancelling scan...";
    }

    private bool CanCancelScan() => IsScanning;

    private async Task LoadLibraryStatsAsync()
    {
        try
        {
            var stats = await _databaseService.GetLibraryStatsAsync();
            TotalArtists = stats.TotalArtists;
            TotalAlbums = stats.TotalAlbums;
            TotalTracks = stats.TotalTracks;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading stats: {ex.Message}";
        }
    }

    private void OnScanProgressChanged(object? sender, ScanProgressEventArgs e)
    {
        ScanProgress = e.PercentComplete;
        ScanStatus = e.StatusMessage;
    }

    private void OnScanError(object? sender, string error)
    {
        // Collect errors but don't stop the scan
        ErrorMessage = error;
    }
}
