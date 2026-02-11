using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClerioVision.MusicDB.Data;
using ClerioVision.MusicDB.Models;

namespace ClerioVision.MusicDB.Services;

/// <summary>
/// Service for generating reports
/// </summary>
public class ReportService
{
    private readonly MusicDbContext _context;

    public ReportService(MusicDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Generates a comprehensive album report sorted by artist and album
    /// </summary>
    public async Task<AlbumReport> GenerateAlbumReportAsync()
    {
        var albums = await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Tracks)
            .OrderBy(a => a.Artist.Name)
            .ThenBy(a => a.Title)
            .ToListAsync();

        var reportItems = albums.Select(album => new AlbumReportItem
        {
            AlbumId = album.AlbumId,
            AlbumTitle = album.Title,
            ArtistName = album.Artist.Name,
            CoverArtData = album.CoverArtData,
            Tracks = album.Tracks
                .OrderBy(t => t.TrackNumber ?? int.MaxValue)
                .Select(t => new ReportTrackItem
                {
                    TrackNumber = t.TrackNumber,
                    TrackTitle = t.Title,
                    DurationSeconds = t.DurationSeconds,
                    FormattedDuration = FormatDuration(t.DurationSeconds)
                })
                .ToList(),
            TotalDuration = album.Tracks.Sum(t => t.DurationSeconds),
            TrackCount = album.Tracks.Count
        }).ToList();

        return new AlbumReport
        {
            GeneratedDate = DateTime.Now,
            TotalAlbums = reportItems.Count,
            TotalTracks = reportItems.Sum(r => r.TrackCount),
            Items = reportItems
        };
    }

    /// <summary>
    /// Generates a report for a specific artist
    /// </summary>
    public async Task<ArtistReport> GenerateArtistReportAsync(string artistName)
    {
        var artist = await _context.Artists
            .Include(a => a.Albums)
                .ThenInclude(al => al.Tracks)
            .FirstOrDefaultAsync(a => a.Name == artistName);

        if (artist == null)
        {
            return new ArtistReport
            {
                ArtistName = artistName,
                ArtistFound = false
            };
        }

        var albums = artist.Albums
            .OrderBy(al => al.Title)
            .Select(album => new AlbumReportItem
            {
                AlbumId = album.AlbumId,
                AlbumTitle = album.Title,
                ArtistName = artist.Name,
                CoverArtData = album.CoverArtData,
                Tracks = album.Tracks
                    .OrderBy(t => t.TrackNumber ?? int.MaxValue)
                    .Select(t => new ReportTrackItem
                    {
                        TrackNumber = t.TrackNumber,
                        TrackTitle = t.Title,
                        DurationSeconds = t.DurationSeconds,
                        FormattedDuration = FormatDuration(t.DurationSeconds)
                    })
                    .ToList(),
                TotalDuration = album.Tracks.Sum(t => t.DurationSeconds),
                TrackCount = album.Tracks.Count
            })
            .ToList();

        return new ArtistReport
        {
            ArtistName = artist.Name,
            ArtistFound = true,
            GeneratedDate = DateTime.Now,
            Albums = albums,
            TotalAlbums = albums.Count,
            TotalTracks = albums.Sum(a => a.TrackCount)
        };
    }

    private static string FormatDuration(int seconds)
    {
        var timespan = TimeSpan.FromSeconds(seconds);
        return timespan.Hours > 0
            ? timespan.ToString(@"h\:mm\:ss")
            : timespan.ToString(@"m\:ss");
    }
}

/// <summary>
/// Complete album report
/// </summary>
public class AlbumReport
{
    public DateTime GeneratedDate { get; set; }
    public int TotalAlbums { get; set; }
    public int TotalTracks { get; set; }
    public List<AlbumReportItem> Items { get; set; } = new();
}

/// <summary>
/// Artist-specific report
/// </summary>
public class ArtistReport
{
    public string ArtistName { get; set; } = string.Empty;
    public bool ArtistFound { get; set; }
    public DateTime GeneratedDate { get; set; }
    public int TotalAlbums { get; set; }
    public int TotalTracks { get; set; }
    public List<AlbumReportItem> Albums { get; set; } = new();
}

/// <summary>
/// Single album in a report
/// </summary>
public class AlbumReportItem
{
    public int AlbumId { get; set; }
    public string AlbumTitle { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public byte[]? CoverArtData { get; set; }
    public int TrackCount { get; set; }
    public int TotalDuration { get; set; }
    public List<ReportTrackItem> Tracks { get; set; } = new();

    public string FormattedTotalDuration
    {
        get
        {
            var timespan = TimeSpan.FromSeconds(TotalDuration);
            return timespan.Hours > 0
                ? timespan.ToString(@"h\:mm\:ss")
                : timespan.ToString(@"m\:ss");
        }
    }
}

/// <summary>
/// Track item in a report
/// </summary>
public class ReportTrackItem
{
    public int? TrackNumber { get; set; }
    public string TrackTitle { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }
    public string FormattedDuration { get; set; } = string.Empty;
}
