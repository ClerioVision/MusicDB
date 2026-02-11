using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClerioVision.MusicDB.Data;
using ClerioVision.MusicDB.Models;

namespace ClerioVision.MusicDB.Services;

/// <summary>
/// Service for database operations
/// </summary>
public class DatabaseService
{
    private readonly MusicDbContext _context;

    public DatabaseService(MusicDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Ensures the database is created and migrations are applied
    /// </summary>
    public async Task InitializeDatabaseAsync()
    {
        await _context.Database.MigrateAsync();
    }

    /// <summary>
    /// Tests the database connection
    /// </summary>
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            return await _context.Database.CanConnectAsync();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Imports MP3 file information into the database
    /// </summary>
    public async Task<int> ImportMp3FilesAsync(List<Mp3FileInfo> files)
    {
        var importedCount = 0;

        // Group files by artist and album for batch processing
        var grouped = files.GroupBy(f => new { f.Artist, f.Album });

        foreach (var group in grouped)
        {
            try
            {
                // Get or create artist
                var artist = await GetOrCreateArtistAsync(group.Key.Artist);

                // Get or create album
                var album = await GetOrCreateAlbumAsync(group.Key.Album, artist.ArtistId, group.First().AlbumArt);

                // Add tracks
                foreach (var fileInfo in group)
                {
                    // Check if track already exists by file path
                    var existingTrack = await _context.Tracks
                        .FirstOrDefaultAsync(t => t.FilePath == fileInfo.FilePath);

                    if (existingTrack == null)
                    {
                        var track = new Track
                        {
                            Title = fileInfo.Title,
                            ArtistId = artist.ArtistId,
                            AlbumId = album.AlbumId,
                            TrackNumber = fileInfo.TrackNumber,
                            DurationSeconds = fileInfo.DurationSeconds,
                            FilePath = fileInfo.FilePath
                        };

                        _context.Tracks.Add(track);
                        importedCount++;
                    }
                    else
                    {
                        // Update existing track
                        existingTrack.Title = fileInfo.Title;
                        existingTrack.ArtistId = artist.ArtistId;
                        existingTrack.AlbumId = album.AlbumId;
                        existingTrack.TrackNumber = fileInfo.TrackNumber;
                        existingTrack.DurationSeconds = fileInfo.DurationSeconds;
                        existingTrack.UpdatedAt = DateTime.UtcNow;
                    }
                }

                // Save changes in batches
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error but continue with other files
                Console.WriteLine($"Error importing files for {group.Key.Artist} - {group.Key.Album}: {ex.Message}");
            }
        }

        return importedCount;
    }

    private async Task<Artist> GetOrCreateArtistAsync(string artistName)
    {
        var artist = await _context.Artists
            .FirstOrDefaultAsync(a => a.Name == artistName);

        if (artist == null)
        {
            artist = new Artist { Name = artistName };
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
        }

        return artist;
    }

    private async Task<Album> GetOrCreateAlbumAsync(string albumTitle, int artistId, byte[]? coverArt)
    {
        var album = await _context.Albums
            .FirstOrDefaultAsync(a => a.Title == albumTitle && a.ArtistId == artistId);

        if (album == null)
        {
            album = new Album 
            { 
                Title = albumTitle, 
                ArtistId = artistId,
                CoverArtData = coverArt
            };
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
        }
        else if (album.CoverArtData == null && coverArt != null)
        {
            // Update album art if it wasn't set before
            album.CoverArtData = coverArt;
            await _context.SaveChangesAsync();
        }

        return album;
    }

    /// <summary>
    /// Gets summary statistics for the library
    /// </summary>
    public async Task<LibraryStats> GetLibraryStatsAsync()
    {
        return new LibraryStats
        {
            TotalArtists = await _context.Artists.CountAsync(),
            TotalAlbums = await _context.Albums.CountAsync(),
            TotalTracks = await _context.Tracks.CountAsync()
        };
    }

    /// <summary>
    /// Gets all artist names for autocomplete
    /// </summary>
    public async Task<List<string>> GetArtistNamesAsync()
    {
        return await _context.Artists
            .OrderBy(a => a.Name)
            .Select(a => a.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Gets all album titles for autocomplete
    /// </summary>
    public async Task<List<string>> GetAlbumTitlesAsync()
    {
        return await _context.Albums
            .OrderBy(a => a.Title)
            .Select(a => a.Title)
            .Distinct()
            .ToListAsync();
    }
}

/// <summary>
/// Library statistics
/// </summary>
public class LibraryStats
{
    public int TotalArtists { get; set; }
    public int TotalAlbums { get; set; }
    public int TotalTracks { get; set; }
}
