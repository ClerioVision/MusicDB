using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClerioVision.MusicDB.Models;

/// <summary>
/// Represents a music track in the database
/// </summary>
public class Track
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TrackId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int ArtistId { get; set; }

    [Required]
    public int AlbumId { get; set; }

    /// <summary>
    /// Track number within the album
    /// </summary>
    public int? TrackNumber { get; set; }

    /// <summary>
    /// Track duration in seconds
    /// </summary>
    public int DurationSeconds { get; set; }

    /// <summary>
    /// Full file path to the MP3 file
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string FilePath { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ArtistId))]
    public virtual Artist Artist { get; set; } = null!;
    
    [ForeignKey(nameof(AlbumId))]
    public virtual Album Album { get; set; } = null!;

    /// <summary>
    /// Formatted duration for display (MM:SS or HH:MM:SS)
    /// </summary>
    [NotMapped]
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
