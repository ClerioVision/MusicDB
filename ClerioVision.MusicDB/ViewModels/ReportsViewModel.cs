using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClerioVision.MusicDB.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ClerioVision.MusicDB.ViewModels;

/// <summary>
/// View model for the Reports page
/// </summary>
public partial class ReportsViewModel : ObservableObject
{
    private readonly ReportService _reportService;

    [ObservableProperty]
    private bool _isGenerating = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private AlbumReport? _currentReport;

    [ObservableProperty]
    private string _reportTitle = string.Empty;

    [ObservableProperty]
    private string _reportSummary = string.Empty;

    public ObservableCollection<AlbumReportItem> ReportItems { get; } = new();

    public ReportsViewModel(ReportService reportService)
    {
        _reportService = reportService;
    }

    [RelayCommand]
    private async Task GenerateFullReportAsync()
    {
        IsGenerating = true;
        ErrorMessage = string.Empty;
        ReportItems.Clear();

        try
        {
            var report = await _reportService.GenerateAlbumReportAsync();
            CurrentReport = report;

            ReportTitle = "Complete Album Report";
            ReportSummary = $"Generated on {report.GeneratedDate:f}\n" +
                          $"Total Albums: {report.TotalAlbums}\n" +
                          $"Total Tracks: {report.TotalTracks}";

            foreach (var item in report.Items)
                ReportItems.Add(item);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error generating report: {ex.Message}";
        }
        finally
        {
            IsGenerating = false;
        }
    }

    [RelayCommand]
    private void ClearReport()
    {
        ReportItems.Clear();
        CurrentReport = null;
        ReportTitle = string.Empty;
        ReportSummary = string.Empty;
        ErrorMessage = string.Empty;
    }
}
