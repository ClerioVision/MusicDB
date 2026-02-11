using Microsoft.EntityFrameworkCore;
using ClerioVision.MusicDB.Models;

namespace ClerioVision.MusicDB.Data;

/// <summary>
/// Entity Framework Core database context for the Music Library
/// </summary>
public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options)
        : base(options)
    {
    }

    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Album> Albums { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Artist entity
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("Artists");
            entity.HasKey(a => a.ArtistId);
            entity.HasIndex(a => a.Name);
            
            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(a => a.CreatedAt)
                .HasDefaultValueSql("NOW()");
            
            entity.Property(a => a.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // Configure relationships
            entity.HasMany(a => a.Albums)
                .WithOne(al => al.Artist)
                .HasForeignKey(al => al.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(a => a.Tracks)
                .WithOne(t => t.Artist)
                .HasForeignKey(t => t.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Album entity
        modelBuilder.Entity<Album>(entity =>
        {
            entity.ToTable("Albums");
            entity.HasKey(al => al.AlbumId);
            entity.HasIndex(al => al.Title);
            entity.HasIndex(al => al.ArtistId);
            
            entity.Property(al => al.Title)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(al => al.CoverArtPath)
                .HasMaxLength(1000);
            
            entity.Property(al => al.CreatedAt)
                .HasDefaultValueSql("NOW()");
            
            entity.Property(al => al.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            // Configure relationships
            entity.HasMany(al => al.Tracks)
                .WithOne(t => t.Album)
                .HasForeignKey(t => t.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Track entity
        modelBuilder.Entity<Track>(entity =>
        {
            entity.ToTable("Tracks");
            entity.HasKey(t => t.TrackId);
            entity.HasIndex(t => t.Title);
            entity.HasIndex(t => t.ArtistId);
            entity.HasIndex(t => t.AlbumId);
            entity.HasIndex(t => t.FilePath).IsUnique();
            
            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(t => t.FilePath)
                .IsRequired()
                .HasMaxLength(1000);
            
            entity.Property(t => t.CreatedAt)
                .HasDefaultValueSql("NOW()");
            
            entity.Property(t => t.UpdatedAt)
                .HasDefaultValueSql("NOW()");
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is Artist artist)
                artist.UpdatedAt = DateTime.UtcNow;
            else if (entry.Entity is Album album)
                album.UpdatedAt = DateTime.UtcNow;
            else if (entry.Entity is Track track)
                track.UpdatedAt = DateTime.UtcNow;
        }
    }
}
