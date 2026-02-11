using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClerioVision.MusicDB.Data;
using ClerioVision.MusicDB.Models;

namespace ClerioVision.MusicDB.Services;

/// <summary>
/// Service for searching the music library
/// </summary>
public class SearchService
{
    private readonly MusicDbContext _context;

    public SearchService(MusicDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Searches for all albums by a particular artist
    /// </summary>
    public async Task<List<AlbumSearchResult>> SearchAlbumsByArtistAsync(string artistName)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Where(a => a.Artist.Name.Contains(artistName))
            .OrderBy(a => a.Title)
            .Select(a => new AlbumSearchResult
            {
                AlbumId = a.AlbumId,
                AlbumTitle = a.Title,
                ArtistName = a.Artist.Name,
                CoverArtData = a.CoverArtData,
                TrackCount = a.Tracks.Count
            })
            .ToListAsync();
    }

    /// <summary>
    /// Searches for all tracks by a particular artist
    /// </summary>
    public async Task<List<TrackSearchResult>> SearchTracksByArtistAsync(string artistName)
    {
        return await _context.Tracks
            .Include(t => t.Artist)
            .Include(t => t.Album)
            .Where(t => t.Artist.Name.Contains(artistName))
            .OrderBy(t => t.Album.Title)
            .ThenBy(t => t.TrackNumber)
            .Select(t => new TrackSearchResult
            {
                TrackId = t.TrackId,
                TrackTitle = t.Title,
                ArtistName = t.Artist.Name,
                AlbumTitle = t.Album.Title,
                TrackNumber = t.TrackNumber,
                DurationSeconds = t.DurationSeconds,
                FilePath = t.FilePath
            })
            .ToListAsync();
    }

    /// <summary>
    /// Searches for albums containing a track with the given title
    /// </summary>
    public async Task<List<AlbumSearchResult>> SearchAlbumsByTrackTitleAsync(string trackTitle)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Tracks)
            .Where(a => a.Tracks.Any(t => t.Title.Contains(trackTitle)))
            .OrderBy(a => a.Artist.Name)
            .ThenBy(a => a.Title)
            .Select(a => new AlbumSearchResult
            {
                AlbumId = a.AlbumId,
                AlbumTitle = a.Title,
                ArtistName = a.Artist.Name,
                CoverArtData = a.CoverArtData,
                TrackCount = a.Tracks.Count,
                MatchingTrackTitle = a.Tracks
                    .Where(t => t.Title.Contains(trackTitle))
                    .Select(t => t.Title)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }

    /// <summary>
    /// Searches for all tracks in a particular album
    /// </summary>
    public async Task<List<TrackSearchResult>> SearchTracksByAlbumAsync(string albumTitle)
    {
        return await _context.Tracks
            .Include(t => t.Artist)
            .Include(t => t.Album)
            .Where(t => t.Album.Title.Contains(albumTitle))
            .OrderBy(t => t.Album.Title)
            .ThenBy(t => t.TrackNumber ?? int.MaxValue)
            .Select(t => new TrackSearchResult
            {
                TrackId = t.TrackId,
                TrackTitle = t.Title,
                ArtistName = t.Artist.Name,
                AlbumTitle = t.Album.Title,
                TrackNumber = t.TrackNumber,
                DurationSeconds = t.DurationSeconds,
                FilePath = t.FilePath
            })
            .ToListAsync();
    }

    /// <summary>
    /// Gets complete album details including all tracks
    /// </summary>
    public async Task<AlbumDetails?> GetAlbumDetailsAsync(int albumId)
    {
        var album = await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Tracks)
            .FirstOrDefaultAsync(a => a.AlbumId == albumId);

        if (album == null)
            return null;

        return new AlbumDetails
        {
            AlbumId = album.AlbumId,
            AlbumTitle = album.Title,
            ArtistName = album.Artist.Name,
            CoverArtData = album.CoverArtData,
            Tracks = album.Tracks
                .OrderBy(t => t.TrackNumber ?? int.MaxValue)
                .Select(t => new TrackSearchResult
                {
                    TrackId = t.TrackId,
                    TrackTitle = t.Title,
                    ArtistName = t.Artist.Name,
                    AlbumTitle = t.Album.Title,
                    TrackNumber = t.TrackNumber,
                    DurationSeconds = t.DurationSeconds,
                    FilePath = t.FilePath
                })
                .ToList()
        };
    }
}

/// <summary>
/// Album search result
/// </summary>
public class AlbumSearchResult
{
    public int AlbumId { get; set; }
    public string AlbumTitle { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public byte[]? CoverArtData { get; set; }
    public int TrackCount { get; set; }
    public string? MatchingTrackTitle { get; set; }
}

/// <summary>
/// Track search result
/// </summary>
public class TrackSearchResult
{
    public int TrackId { get; set; }
    public string TrackTitle { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string AlbumTitle { get; set; } = string.Empty;
    public int? TrackNumber { get; set; }
    public int DurationSeconds { get; set; }
    public string FilePath { get; set; } = string.Empty;

    public string FormattedDuration
    {
        get
        {
            var timespan = TimeSpan.FromSeconds(DurationSeconds);
            return timespan.Hours > 0
                ? timespan.ToString(@"h\:mm\:ss")
                : timespan.ToString(@"m\:ss");
        }
    }
}

/// <summary>
/// Complete album details with tracks
/// </summary>
public class AlbumDetails
{
    public int AlbumId { get; set; }
    public string AlbumTitle { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public byte[]? CoverArtData { get; set; }
    public List<TrackSearchResult> Tracks { get; set; } = new();
}
