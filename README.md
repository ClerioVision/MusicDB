# Music Library - WinUI 3 Desktop Application

A comprehensive music library management application for Windows that scans MP3 files, extracts metadata, stores information in PostgreSQL, and provides powerful search and reporting capabilities.

![Music Library](https://img.shields.io/badge/Platform-Windows-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![WinUI](https://img.shields.io/badge/WinUI-3-green)

## Features

### 🎵 Music Library Management
- **Recursive Folder Scanning**: Scan entire music folders and all subfolders for MP3 files
- **Automatic Metadata Extraction**: Reads ID3 tags including title, artist, album, track number, duration, and album art
- **PostgreSQL Database**: Normalized database schema with separate tables for Artists, Albums, and Tracks
- **Progress Tracking**: Real-time progress indicators during scanning operations

### 🔍 Advanced Search Capabilities
- **Search Albums by Artist**: Find all albums from a specific artist
- **Search Tracks by Artist**: View all tracks across all albums by an artist
- **Search Albums by Track Title**: Locate albums containing a specific track
- **Search Tracks by Album**: See complete track listings for any album
- **Auto-complete Suggestions**: Smart suggestions for artists and albums

### 📊 Comprehensive Reports
- **Full Album Report**: Generate detailed reports sorted by artist and album
- **Album Art Display**: Visual presentation with album cover thumbnails
- **Track Listings**: Complete track information with durations
- **Summary Statistics**: View totals for artists, albums, and tracks

### ⚙️ Configuration & Settings
- **Database Configuration**: Easy PostgreSQL connection string management
- **Connection Testing**: Verify database connectivity before operations
- **Database Initialization**: Automatic schema creation and migrations
- **Modern UI**: WinUI 3 Fluent Design with Mica/Acrylic materials

## Prerequisites

### Required Software
- **Windows 10** version 1809 (build 17763) or later, or **Windows 11**
- **.NET 8.0 SDK** or later - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022** version 17.8 or later with:
  - .NET Desktop Development workload
  - Windows App SDK (included with Windows App SDK workload)
- **PostgreSQL** 12 or later - [Download](https://www.postgresql.org/download/)

### Optional Tools
- **pgAdmin 4** - For database management
- **Visual Studio Code** - Alternative IDE

## Getting Started

### 1. PostgreSQL Setup

#### Install PostgreSQL
1. Download and install PostgreSQL from https://www.postgresql.org/download/
2. During installation, remember the password you set for the `postgres` user
3. Default port is usually 5432

#### Create Database
```sql
-- Connect to PostgreSQL using psql or pgAdmin
CREATE DATABASE musiclibrary;

-- Optionally, create a dedicated user
CREATE USER musicuser WITH PASSWORD 'yourpassword';
GRANT ALL PRIVILEGES ON DATABASE musiclibrary TO musicuser;
```

### 2. Clone and Build

```bash
# Clone the repository
git clone https://github.com/ClerioVision/MusicDB.git
cd MusicDB

# Open the solution in Visual Studio
# or use command line:
dotnet restore
dotnet build
```

### 3. Configure Database Connection

Edit `appsettings.json` in the project root:

```json
{
  "ConnectionStrings": {
    "MusicDatabase": "Host=localhost;Port=5432;Database=musiclibrary;Username=postgres;Password=yourpassword"
  }
}
```

**Connection String Parameters:**
- `Host`: Database server address (usually `localhost`)
- `Port`: PostgreSQL port (default: `5432`)
- `Database`: Database name you created
- `Username`: PostgreSQL username
- `Password`: Your PostgreSQL password

### 4. Initialize Database

Two options:

**Option A: Using the Application**
1. Launch the application
2. Navigate to Settings page
3. Verify your connection string
4. Click "Test Connection"
5. Click "Initialize Database"

**Option B: Using Command Line**
```bash
# Navigate to project directory
cd ClerioVision.MusicDB

# Run migrations
dotnet ef database update
```

### 5. Run the Application

**From Visual Studio:**
- Press F5 or click the "Start" button
- Ensure the platform is set to x64, x86, or ARM64 (not AnyCPU)

**From Command Line:**
```bash
dotnet run --project ClerioVision.MusicDB
```

## Usage Guide

### Scanning Your Music Library

1. **Navigate to Library Page**
   - Click "Library" in the navigation menu

2. **Select Folder**
   - Click "Select Folder" button
   - Browse to your music folder (e.g., `C:\Users\YourName\Music`)
   - The app will scan all subdirectories

3. **Start Scan**
   - Click "Start Scan" button
   - Monitor progress with the progress bar
   - View statistics update in real-time

4. **Handle Errors**
   - Files with missing or corrupted tags are skipped
   - Error messages display at the bottom
   - Scan continues even if some files fail

### Searching Your Library

1. **Navigate to Search Page**
   - Click "Search" in the navigation menu

2. **Select Search Type**
   - Choose from dropdown:
     - Albums by Artist
     - Tracks by Artist
     - Albums by Track Title
     - Tracks by Album

3. **Enter Search Query**
   - Type artist name, album title, or track title
   - Partial matches are supported
   - Use autocomplete suggestions

4. **View Results**
   - Results display in cards or lists
   - Click on items for more details

### Generating Reports

1. **Navigate to Reports Page**
   - Click "Reports" in the navigation menu

2. **Generate Report**
   - Click "Generate Full Report"
   - Wait for report generation

3. **View Report**
   - Scroll through albums sorted by artist
   - Each album shows:
     - Album art
     - Album and artist name
     - Complete track listing
     - Total duration

4. **Export (Future Feature)**
   - PDF export coming soon
   - HTML export coming soon

## Architecture

### Technology Stack
- **Frontend**: WinUI 3 (Windows App SDK 1.6)
- **Backend**: .NET 8.0
- **Database**: PostgreSQL with Entity Framework Core 8.0
- **MP3 Metadata**: TagLibSharp 2.3.0
- **MVVM Framework**: CommunityToolkit.Mvvm 8.3.2

### Project Structure
```
ClerioVision.MusicDB/
├── Data/                        # Database context and migrations
│   ├── MusicDbContext.cs
│   └── Migrations/
├── Models/                      # Entity models
│   ├── Artist.cs
│   ├── Album.cs
│   └── Track.cs
├── Services/                    # Business logic
│   ├── Mp3ScannerService.cs
│   ├── DatabaseService.cs
│   ├── SearchService.cs
│   └── ReportService.cs
├── ViewModels/                  # MVVM view models
│   ├── MainViewModel.cs
│   ├── LibraryViewModel.cs
│   ├── SearchViewModel.cs
│   ├── ReportsViewModel.cs
│   └── SettingsViewModel.cs
├── Views/                       # XAML pages
│   ├── LibraryPage.xaml
│   ├── SearchPage.xaml
│   ├── ReportsPage.xaml
│   └── SettingsPage.xaml
├── Helpers/                     # Utility classes
│   ├── Converters.cs
│   └── ImageHelper.cs
└── Assets/                      # Images and resources
```

### Database Schema

**Artists Table:**
- ArtistId (PK, Auto-increment)
- Name (string, indexed)
- CreatedAt, UpdatedAt (timestamps)

**Albums Table:**
- AlbumId (PK, Auto-increment)
- Title (string, indexed)
- ArtistId (FK to Artists)
- CoverArtPath (string, optional)
- CoverArtData (bytea, optional)
- CreatedAt, UpdatedAt (timestamps)

**Tracks Table:**
- TrackId (PK, Auto-increment)
- Title (string, indexed)
- ArtistId (FK to Artists)
- AlbumId (FK to Albums)
- TrackNumber (int, optional)
- DurationSeconds (int)
- FilePath (string, unique, indexed)
- CreatedAt, UpdatedAt (timestamps)

**Relationships:**
- One Artist → Many Albums
- One Artist → Many Tracks
- One Album → Many Tracks
- Cascade delete enabled

## Troubleshooting

### Database Connection Issues

**Problem: Cannot connect to database**
```
Solution:
1. Verify PostgreSQL is running
   - Windows: Check Services for "postgresql-x64-XX"
2. Test connection with pgAdmin or psql
3. Verify connection string parameters
4. Check firewall settings
5. Ensure database exists
```

**Problem: Database initialization fails**
```
Solution:
1. Check PostgreSQL user permissions
2. Ensure user can create tables
3. Run: GRANT ALL PRIVILEGES ON DATABASE musiclibrary TO postgres;
4. Try manual migration: dotnet ef database update
```

### Scanning Issues

**Problem: No files found**
```
Solution:
1. Verify folder contains .mp3 files
2. Check file permissions
3. Ensure files are not in use by other programs
```

**Problem: Some files skipped**
```
Solution:
1. Check file integrity (try playing in media player)
2. Verify files have ID3 tags
3. Look at error messages for specific issues
4. Some files may have corrupted metadata
```

### Build Issues

**Problem: Missing Windows App SDK**
```
Solution:
1. Install latest Windows App SDK
2. Update Visual Studio to 17.8 or later
3. Install "Windows App Development" workload
```

**Problem: NuGet restore fails**
```
Solution:
1. Clear NuGet cache: dotnet nuget locals all --clear
2. Restore packages: dotnet restore
3. Check internet connection
```

## Performance Tips

1. **Large Libraries**: For libraries with 10,000+ tracks:
   - Scan in batches by folder
   - Be patient during initial scan (can take several minutes)
   - Database operations are batched automatically

2. **Database Performance**:
   - Indexes are created automatically
   - Consider increasing PostgreSQL shared_buffers
   - Regular VACUUM operations recommended

3. **Album Art**:
   - Album art is stored in database as binary data
   - Large collections may increase database size significantly
   - Consider storing as file paths instead if space is limited

## Development

### Adding Database Migrations

```bash
# Add a new migration
dotnet ef migrations add MigrationName --project ClerioVision.MusicDB

# Update database
dotnet ef database update --project ClerioVision.MusicDB

# Remove last migration (if not applied)
dotnet ef migrations remove --project ClerioVision.MusicDB
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "TestName"
```

## Contributing

Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License.

## Acknowledgments

- **TagLibSharp** - For MP3 metadata extraction
- **Entity Framework Core** - For database operations
- **Community Toolkit** - For MVVM helpers
- **WinUI 3** - For modern Windows UI

## Support

For issues and questions:
- Create an issue on GitHub
- Check existing issues for solutions
- Review troubleshooting section above

## Roadmap

### Upcoming Features
- [ ] Export reports to PDF
- [ ] Export reports to HTML
- [ ] Playlist management
- [ ] Music player integration
- [ ] Duplicate track detection
- [ ] Batch tag editing
- [ ] Cloud backup support
- [ ] Statistics and analytics dashboard

## Screenshots

_Screenshots will be added after UI implementation and testing_

---

**Made with ❤️ by ClerioVision**
