using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClerioVision.MusicDB.Models;

namespace ClerioVision.MusicDB.Services;

/// <summary>
/// Service for scanning folders and reading MP3 metadata
/// </summary>
public class Mp3ScannerService
{
    public event EventHandler<ScanProgressEventArgs>? ProgressChanged;
    public event EventHandler<string>? ErrorOccurred;

    /// <summary>
    /// Scans a folder recursively for MP3 files and extracts metadata
    /// </summary>
    public async Task<List<Mp3FileInfo>> ScanFolderAsync(string folderPath, CancellationToken cancellationToken = default)
    {
        var results = new List<Mp3FileInfo>();
        
        if (!Directory.Exists(folderPath))
        {
            ErrorOccurred?.Invoke(this, $"Folder does not exist: {folderPath}");
            return results;
        }

        try
        {
            var mp3Files = Directory.GetFiles(folderPath, "*.mp3", SearchOption.AllDirectories);
            var totalFiles = mp3Files.Length;
            var processedFiles = 0;

            ReportProgress(0, totalFiles, "Starting scan...");

            foreach (var filePath in mp3Files)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                try
                {
                    var fileInfo = await ExtractMetadataAsync(filePath);
                    if (fileInfo != null)
                    {
                        results.Add(fileInfo);
                    }
                }
                catch (Exception ex)
                {
                    ErrorOccurred?.Invoke(this, $"Error processing file {filePath}: {ex.Message}");
                }

                processedFiles++;
                ReportProgress(processedFiles, totalFiles, $"Processing: {Path.GetFileName(filePath)}");
            }

            ReportProgress(totalFiles, totalFiles, "Scan complete!");
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, $"Error scanning folder: {ex.Message}");
        }

        return results;
    }

    /// <summary>
    /// Extracts metadata from a single MP3 file using TagLibSharp
    /// </summary>
    private async Task<Mp3FileInfo?> ExtractMetadataAsync(string filePath)
    {
        return await Task.Run(() =>
        {
            try
            {
                using var file = TagLib.File.Create(filePath);
                
                var artist = file.Tag.FirstPerformer ?? file.Tag.FirstAlbumArtist ?? "Unknown Artist";
                var album = file.Tag.Album ?? "Unknown Album";
                var title = file.Tag.Title ?? Path.GetFileNameWithoutExtension(filePath);
                var trackNumber = (int?)file.Tag.Track;
                var durationSeconds = (int)file.Properties.Duration.TotalSeconds;

                // Extract album art if available
                byte[]? albumArt = null;
                if (file.Tag.Pictures.Length > 0)
                {
                    albumArt = file.Tag.Pictures[0].Data.Data;
                }

                return new Mp3FileInfo
                {
                    FilePath = filePath,
                    Title = title,
                    Artist = artist,
                    Album = album,
                    TrackNumber = trackNumber,
                    DurationSeconds = durationSeconds,
                    AlbumArt = albumArt
                };
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error reading tags from {filePath}: {ex.Message}");
                return null;
            }
        });
    }

    private void ReportProgress(int current, int total, string message)
    {
        var percentage = total > 0 ? (int)((double)current / total * 100) : 0;
        ProgressChanged?.Invoke(this, new ScanProgressEventArgs
        {
            CurrentFile = current,
            TotalFiles = total,
            PercentComplete = percentage,
            StatusMessage = message
        });
    }
}

/// <summary>
/// Information extracted from an MP3 file
/// </summary>
public class Mp3FileInfo
{
    public string FilePath { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public int? TrackNumber { get; set; }
    public int DurationSeconds { get; set; }
    public byte[]? AlbumArt { get; set; }
}

/// <summary>
/// Event args for scan progress updates
/// </summary>
public class ScanProgressEventArgs : EventArgs
{
    public int CurrentFile { get; set; }
    public int TotalFiles { get; set; }
    public int PercentComplete { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
}
