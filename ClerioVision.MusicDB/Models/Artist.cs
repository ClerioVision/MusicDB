using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClerioVision.MusicDB.Models;

/// <summary>
/// Represents a music artist in the database
/// </summary>
public class Artist
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ArtistId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
    
    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
