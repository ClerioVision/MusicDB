# Quick Start Guide - Music Library

Get your Music Library application up and running in 5 minutes!

## Prerequisites Check

Before you begin, ensure you have:
- ✅ Windows 10 (build 17763+) or Windows 11
- ✅ Visual Studio 2022 (17.8+) with Windows App SDK
- ✅ .NET 8.0 SDK
- ✅ PostgreSQL 12+ OR Docker Desktop

## Option 1: Quick Start with Docker (Recommended for Testing)

### Step 1: Start Database
```bash
# In the project root directory
docker-compose up -d
```

This starts PostgreSQL on `localhost:5432` with database `musiclibrary`.

### Step 2: Open and Run
1. Open `ClerioVision.MusicDB.sln` in Visual Studio 2022
2. Press F5 to build and run
3. Go to Settings page and click "Initialize Database"
4. You're ready to scan music!

### Step 3: Scan Your Music
1. Click "Library" in navigation
2. Click "Select Folder"
3. Choose your music folder
4. Click "Start Scan"

## Option 2: Manual PostgreSQL Setup

### Step 1: Install PostgreSQL
1. Download from https://www.postgresql.org/download/
2. Install with default settings
3. Remember your password!

### Step 2: Create Database
```sql
-- Using pgAdmin or psql
CREATE DATABASE musiclibrary;
```

### Step 3: Configure Connection
Edit `ClerioVision.MusicDB/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "MusicDatabase": "Host=localhost;Port=5432;Database=musiclibrary;Username=postgres;Password=YOUR_PASSWORD"
  }
}
```

### Step 4: Run Application
1. Open `ClerioVision.MusicDB.sln` in Visual Studio
2. Build solution (Ctrl+Shift+B)
3. Start debugging (F5)
4. Go to Settings and click "Initialize Database"

## First-Time Setup

### Initialize Database
1. Navigate to **Settings** page
2. Verify connection string
3. Click **Test Connection**
4. Click **Initialize Database**
5. Wait for success message

### Scan Your Music Library
1. Navigate to **Library** page
2. Click **Select Folder** button
3. Browse to your music folder (e.g., `C:\Users\YourName\Music`)
4. Click **Start Scan**
5. Watch the progress bar
6. View library statistics when complete

## Testing the Application

### Search Features
1. Go to **Search** page
2. Select search type from dropdown
3. Enter artist name (e.g., "Beatles")
4. Click **Search**
5. View results

### Generate Reports
1. Go to **Reports** page
2. Click **Generate Full Report**
3. Scroll through formatted album listings
4. See album art, tracks, and durations

## Troubleshooting

### Build Errors
```
Problem: Missing Windows App SDK
Solution: 
- Update Visual Studio to 17.8+
- Install "Windows App Development" workload
- Restart Visual Studio
```

### Database Connection Fails
```
Problem: Cannot connect to database
Solution:
- Verify PostgreSQL is running
- Check connection string
- Test with pgAdmin or psql
- Check firewall settings
```

### No MP3 Files Found
```
Problem: Scan finds no files
Solution:
- Verify folder contains .mp3 files
- Check file permissions
- Try a different folder
- Ensure files aren't in use
```

## Next Steps

✅ **You're all set!** Your Music Library is ready to use.

### Learn More
- Read the full [README.md](README.md) for detailed documentation
- Check [CONTRIBUTING.md](CONTRIBUTING.md) to contribute
- Explore [Database/README.md](Database/README.md) for database tips

### Test with Sample Data
```bash
# Using Docker
docker exec -i musicdb-postgres psql -U postgres musiclibrary < Database/sample-data.sql

# Or using psql directly
psql -U postgres -d musiclibrary -f Database/sample-data.sql
```

### Common Commands

**Docker:**
```bash
# View logs
docker-compose logs -f

# Stop database
docker-compose down

# Restart database
docker-compose restart
```

**Entity Framework:**
```bash
# Create migration
dotnet ef migrations add MigrationName --project ClerioVision.MusicDB

# Update database
dotnet ef database update --project ClerioVision.MusicDB
```

## Need Help?

- 📖 Full documentation in [README.md](README.md)
- 🐛 Found a bug? Create an issue
- 💡 Have an idea? Share it in discussions
- 🤝 Want to contribute? See [CONTRIBUTING.md](CONTRIBUTING.md)

---

**Happy music organizing! 🎵**
