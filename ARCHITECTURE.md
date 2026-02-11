# Architecture Documentation - Music Library

## Overview

Music Library is a desktop application built with WinUI 3 that manages MP3 music collections. It follows MVVM (Model-View-ViewModel) architecture pattern with clean separation of concerns.

## Technology Stack

### Frontend
- **WinUI 3** (Windows App SDK 1.6) - Modern Windows UI framework
- **XAML** - Declarative UI markup
- **Fluent Design** - Microsoft's design system

### Backend
- **.NET 8.0** - Cross-platform framework
- **C# 12** - Programming language
- **Async/Await** - Asynchronous programming model

### Database
- **PostgreSQL 12+** - Relational database
- **Entity Framework Core 8.0** - ORM
- **Npgsql** - PostgreSQL provider

### Libraries
- **TagLibSharp 2.3.0** - MP3 metadata extraction
- **CommunityToolkit.Mvvm 8.3.2** - MVVM helpers and source generators

## Architecture Patterns

### MVVM Pattern

```
┌─────────────┐       ┌──────────────┐       ┌─────────────┐
│    View     │◄─────►│  ViewModel   │◄─────►│   Model     │
│   (XAML)    │       │  (C# Class)  │       │ (Entities)  │
└─────────────┘       └──────────────┘       └─────────────┘
      │                       │                      │
      │                       │                      │
      │                       ▼                      ▼
      │               ┌──────────────┐       ┌─────────────┐
      └──────────────►│  Commands    │       │  Services   │
                      │   & Events   │       │  (Business) │
                      └──────────────┘       └─────────────┘
```

### Dependency Injection

Services are registered in `App.xaml.cs` and injected via constructor:

```csharp
services.AddDbContext<MusicDbContext>(options =>
    options.UseNpgsql(connectionString));

services.AddSingleton<Mp3ScannerService>();
services.AddScoped<DatabaseService>();
services.AddTransient<LibraryViewModel>();
```

### Repository Pattern

`DatabaseService` acts as a repository, abstracting database operations:
- `GetOrCreateArtistAsync()` - Artist management
- `GetOrCreateAlbumAsync()` - Album management
- `ImportMp3FilesAsync()` - Batch operations

## Project Structure

```
ClerioVision.MusicDB/
│
├── Views/                    # XAML UI Pages
│   ├── LibraryPage.xaml     # Music scanning interface
│   ├── SearchPage.xaml      # Search interface
│   ├── ReportsPage.xaml     # Report generation
│   └── SettingsPage.xaml    # Configuration
│
├── ViewModels/               # Business logic & state
│   ├── LibraryViewModel.cs  # Scan management
│   ├── SearchViewModel.cs   # Search logic
│   ├── ReportsViewModel.cs  # Report generation
│   └── SettingsViewModel.cs # Settings management
│
├── Models/                   # Entity models
│   ├── Artist.cs            # Artist entity
│   ├── Album.cs             # Album entity
│   └── Track.cs             # Track entity
│
├── Services/                 # Business services
│   ├── Mp3ScannerService.cs # File scanning
│   ├── DatabaseService.cs   # Data access
│   ├── SearchService.cs     # Search operations
│   └── ReportService.cs     # Report generation
│
├── Data/                     # Database context
│   ├── MusicDbContext.cs    # EF Core context
│   └── Migrations/          # Database migrations
│
├── Helpers/                  # Utility classes
│   ├── Converters.cs        # Value converters
│   └── ImageHelper.cs       # Image utilities
│
└── Assets/                   # Images & resources
```

## Data Flow

### Scanning Flow

```
User → LibraryPage → LibraryViewModel → Mp3ScannerService
                            │
                            ├─→ Read MP3 files
                            ├─→ Extract metadata (TagLib)
                            └─→ DatabaseService → PostgreSQL
```

### Search Flow

```
User → SearchPage → SearchViewModel → SearchService
                                         │
                                         ├─→ Query database
                                         └─→ Return results
```

### Report Flow

```
User → ReportsPage → ReportsViewModel → ReportService
                                           │
                                           ├─→ Query database
                                           ├─→ Format data
                                           └─→ Return report
```

## Database Schema

### Entity Relationship Diagram

```
┌──────────────┐
│   Artists    │
│──────────────│
│ ArtistId (PK)│
│ Name         │
│ CreatedAt    │
│ UpdatedAt    │
└──────────────┘
       │ 1
       │
       │ *
┌──────────────┐
│   Albums     │
│──────────────│
│ AlbumId (PK) │
│ Title        │
│ ArtistId (FK)│
│ CoverArtData │
│ CreatedAt    │
│ UpdatedAt    │
└──────────────┘
       │ 1
       │
       │ *
┌──────────────┐
│   Tracks     │
│──────────────│
│ TrackId (PK) │
│ Title        │
│ ArtistId (FK)│
│ AlbumId (FK) │
│ TrackNumber  │
│ Duration     │
│ FilePath     │
│ CreatedAt    │
│ UpdatedAt    │
└──────────────┘
```

### Relationships

- **One-to-Many**: Artist → Albums
- **One-to-Many**: Artist → Tracks
- **One-to-Many**: Album → Tracks
- **Cascade Delete**: Enabled on all foreign keys

### Indexes

Performance indexes on:
- `Artists.Name` - For artist searches
- `Albums.Title` - For album searches
- `Albums.ArtistId` - For joins
- `Tracks.Title` - For track searches
- `Tracks.ArtistId` - For joins
- `Tracks.AlbumId` - For joins
- `Tracks.FilePath` - Unique constraint

## Component Details

### Mp3ScannerService

**Purpose**: Scan directories for MP3 files and extract metadata

**Key Methods:**
- `ScanFolderAsync()` - Recursive directory scan
- `ExtractMetadataAsync()` - Read ID3 tags

**Events:**
- `ProgressChanged` - Scan progress updates
- `ErrorOccurred` - Error notifications

**Dependencies:**
- TagLibSharp - ID3 tag reading

### DatabaseService

**Purpose**: Data access layer for database operations

**Key Methods:**
- `InitializeDatabaseAsync()` - Apply migrations
- `ImportMp3FilesAsync()` - Batch import
- `GetLibraryStatsAsync()` - Statistics
- `GetOrCreateArtistAsync()` - Artist management
- `GetOrCreateAlbumAsync()` - Album management

**Pattern**: Repository pattern

### SearchService

**Purpose**: Execute search queries on the database

**Key Methods:**
- `SearchAlbumsByArtistAsync()` - Find albums by artist
- `SearchTracksByArtistAsync()` - Find tracks by artist
- `SearchAlbumsByTrackTitleAsync()` - Find albums by track
- `SearchTracksByAlbumAsync()` - Find tracks by album

**Optimization**: Includes related entities to avoid N+1 queries

### ReportService

**Purpose**: Generate formatted reports

**Key Methods:**
- `GenerateAlbumReportAsync()` - Complete album report
- `GenerateArtistReportAsync()` - Artist-specific report

**Features:**
- Sorted by artist and album
- Includes album art
- Track listings with durations

## UI Architecture

### Navigation

```
MainWindow (NavigationView)
│
├── LibraryPage (Scan & Stats)
├── SearchPage (Search Interface)
├── ReportsPage (Report Generation)
└── SettingsPage (Configuration)
```

### Data Binding

**Two-Way Binding:**
```xaml
<TextBox Text="{x:Bind ViewModel.SearchQuery, Mode=TwoWay}"/>
```

**One-Way Binding:**
```xaml
<TextBlock Text="{x:Bind ViewModel.TotalTracks, Mode=OneWay}"/>
```

**Command Binding:**
```xaml
<Button Command="{x:Bind ViewModel.StartScanCommand}"/>
```

### Value Converters

Custom converters for data display:
- `BoolNegationConverter` - Invert boolean
- `StringToBoolConverter` - String to visibility
- `IntToBoolConverter` - Integer to visibility
- `ResultCountConverter` - Format result count
- `BoolToColorConverter` - Success/error colors

## Performance Considerations

### Database Optimization
- **Batch Inserts**: Process multiple records in transactions
- **Eager Loading**: Use `Include()` to avoid N+1 queries
- **Indexes**: Strategic indexes on search columns
- **Connection Pooling**: Built into Npgsql

### UI Performance
- **Async Operations**: All I/O operations use async/await
- **Progress Indicators**: Show progress during long operations
- **Virtualization**: ListView uses UI virtualization
- **Throttling**: Limit UI updates during scan

### Memory Management
- **Dispose Pattern**: Proper resource cleanup
- **Weak References**: For event handlers (where appropriate)
- **Streaming**: Process large files in chunks

## Security Considerations

### Database Security
- **Parameterized Queries**: EF Core prevents SQL injection
- **Connection Strings**: Store securely (not in source control)
- **Least Privilege**: Use dedicated database user

### File System
- **Path Validation**: Validate user-selected paths
- **Exception Handling**: Catch and handle file access errors
- **Permissions**: Check before accessing files

### Error Handling
- **Try-Catch Blocks**: Wrap I/O operations
- **User-Friendly Messages**: Display actionable errors
- **Logging**: Log errors for debugging

## Testing Strategy

### Unit Tests (Recommended)
- Test ViewModels in isolation
- Mock services and repositories
- Test business logic

### Integration Tests (Recommended)
- Test database operations
- Test service interactions
- Use test database

### Manual Testing (Required)
- Test UI workflows
- Test with various MP3 files
- Test error scenarios
- Test performance with large libraries

## Deployment

### Requirements
- Windows 10 (17763+) or Windows 11
- .NET 8.0 Runtime
- PostgreSQL 12+

### Distribution Options
1. **MSIX Package** - Microsoft Store distribution
2. **Self-Contained** - Include .NET runtime
3. **Framework-Dependent** - Require .NET installation

### Configuration
- `appsettings.json` - Application configuration
- Connection string must be configured
- Database must be initialized

## Future Enhancements

### Planned Features
- PDF/HTML report export
- Playlist management
- Music player integration
- Duplicate detection
- Batch tag editing
- Cloud backup

### Scalability
- Support for 100,000+ tracks
- Multi-threaded scanning
- Database sharding (if needed)
- Caching layer

## References

- [WinUI 3 Documentation](https://learn.microsoft.com/en-us/windows/apps/winui/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [MVVM Pattern](https://learn.microsoft.com/en-us/windows/apps/develop/data-binding/data-binding-and-mvvm)
- [TagLibSharp](https://github.com/mono/taglib-sharp)
