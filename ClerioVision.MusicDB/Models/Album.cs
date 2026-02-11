using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClerioVision.MusicDB.Models;

/// <summary>
/// Represents a music album in the database
/// </summary>
public class Album
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AlbumId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int ArtistId { get; set; }

    /// <summary>
    /// Path to album cover art file or null if not available
    /// </summary>
    [MaxLength(1000)]
    public string? CoverArtPath { get; set; }

    /// <summary>
    /// Album cover art stored as binary data (optional alternative to file path)
    /// </summary>
    public byte[]? CoverArtData { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(ArtistId))]
    public virtual Artist Artist { get; set; } = null!;
    
    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
