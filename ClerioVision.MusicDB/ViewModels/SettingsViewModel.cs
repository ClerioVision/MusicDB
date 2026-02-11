using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClerioVision.MusicDB.Services;
using System;
using System.Threading.Tasks;

namespace ClerioVision.MusicDB.ViewModels;

/// <summary>
/// View model for the Settings page
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private string _connectionString = string.Empty;

    [ObservableProperty]
    private bool _isTestingConnection = false;

    [ObservableProperty]
    private string _connectionStatus = "Not tested";

    [ObservableProperty]
    private bool _connectionSuccessful = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public SettingsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadConnectionString();
    }

    private void LoadConnectionString()
    {
        // In a real app, this would load from configuration
        ConnectionString = "Host=localhost;Port=5432;Database=musiclibrary;Username=postgres;Password=yourpassword";
    }

    [RelayCommand]
    private async Task TestConnectionAsync()
    {
        IsTestingConnection = true;
        ErrorMessage = string.Empty;

        try
        {
            var canConnect = await _databaseService.TestConnectionAsync();
            ConnectionSuccessful = canConnect;
            ConnectionStatus = canConnect ? "Connection successful!" : "Connection failed";

            if (!canConnect)
            {
                ErrorMessage = "Could not connect to the database. Please check your connection string.";
            }
        }
        catch (Exception ex)
        {
            ConnectionSuccessful = false;
            ConnectionStatus = "Connection failed";
            ErrorMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsTestingConnection = false;
        }
    }

    [RelayCommand]
    private async Task InitializeDatabaseAsync()
    {
        IsTestingConnection = true;
        ErrorMessage = string.Empty;

        try
        {
            await _databaseService.InitializeDatabaseAsync();
            ConnectionStatus = "Database initialized successfully!";
            ConnectionSuccessful = true;
        }
        catch (Exception ex)
        {
            ConnectionStatus = "Database initialization failed";
            ErrorMessage = $"Error: {ex.Message}";
            ConnectionSuccessful = false;
        }
        finally
        {
            IsTestingConnection = false;
        }
    }

    [RelayCommand]
    private void SaveSettings()
    {
        // In a real app, this would save to configuration file
        ConnectionStatus = "Settings saved (not implemented in demo)";
    }
}
