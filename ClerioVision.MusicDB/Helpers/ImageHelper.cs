using System;
using System.IO;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

namespace ClerioVision.MusicDB.Helpers;

/// <summary>
/// Helper class for image-related operations
/// </summary>
public static class ImageHelper
{
    /// <summary>
    /// Converts a byte array to a BitmapImage for display in WinUI
    /// </summary>
    public static async Task<BitmapImage?> ByteArrayToBitmapImageAsync(byte[]? imageData)
    {
        if (imageData == null || imageData.Length == 0)
            return null;

        try
        {
            using var stream = new InMemoryRandomAccessStream();
            await stream.WriteAsync(imageData.AsBuffer());
            stream.Seek(0);

            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(stream);
            return bitmap;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Saves album art to a temporary file and returns the path
    /// </summary>
    public static async Task<string?> SaveAlbumArtToTempFileAsync(byte[]? imageData, int albumId)
    {
        if (imageData == null || imageData.Length == 0)
            return null;

        try
        {
            var tempPath = Path.Combine(Path.GetTempPath(), "MusicDB_AlbumArt");
            Directory.CreateDirectory(tempPath);

            var filePath = Path.Combine(tempPath, $"album_{albumId}.jpg");
            await File.WriteAllBytesAsync(filePath, imageData);
            return filePath;
        }
        catch
        {
            return null;
        }
    }
}
