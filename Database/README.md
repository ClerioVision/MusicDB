# Docker Setup for Music Library Database

This directory contains Docker configuration for running PostgreSQL locally for development.

## Quick Start

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- No local PostgreSQL instance running on port 5432

### Start Database

```bash
# Start PostgreSQL and pgAdmin
docker-compose up -d

# Check status
docker-compose ps

# View logs
docker-compose logs -f postgres
```

### Access Services

**PostgreSQL Database:**
- Host: `localhost`
- Port: `5432`
- Database: `musiclibrary`
- Username: `postgres`
- Password: `postgres`

**Connection String:**
```
Host=localhost;Port=5432;Database=musiclibrary;Username=postgres;Password=postgres
```

**pgAdmin Web Interface:**
- URL: http://localhost:5050
- Email: `admin@musicdb.local`
- Password: `admin`

### Stop Database

```bash
# Stop services (keeps data)
docker-compose stop

# Stop and remove containers (keeps data)
docker-compose down

# Stop and remove everything including data
docker-compose down -v
```

## Using pgAdmin

1. Open http://localhost:5050 in your browser
2. Login with credentials above
3. Click "Add New Server"
4. In "General" tab:
   - Name: `MusicDB Local`
5. In "Connection" tab:
   - Host: `postgres` (container name)
   - Port: `5432`
   - Database: `musiclibrary`
   - Username: `postgres`
   - Password: `postgres`
6. Click "Save"

## Database Initialization

The database is automatically initialized with:
1. `setup.sql` - Creates database and user
2. `sample-data.sql` - Inserts test data (optional)

To skip sample data, comment out the volume mount in `docker-compose.yml`:
```yaml
# - ./Database/sample-data.sql:/docker-entrypoint-initdb.d/02-sample-data.sql
```

## Useful Commands

### Database Backup
```bash
# Backup database
docker exec musicdb-postgres pg_dump -U postgres musiclibrary > backup.sql

# Restore database
docker exec -i musicdb-postgres psql -U postgres musiclibrary < backup.sql
```

### Connect to Database
```bash
# Using psql
docker exec -it musicdb-postgres psql -U postgres -d musiclibrary

# Example queries
\dt                          # List tables
\d "Artists"                 # Describe Artists table
SELECT * FROM "Artists";     # Query artists
```

### Database Shell
```bash
# Get a bash shell in the container
docker exec -it musicdb-postgres bash

# Run psql
psql -U postgres -d musiclibrary
```

### Reset Database
```bash
# Stop and remove everything
docker-compose down -v

# Start fresh
docker-compose up -d
```

## Troubleshooting

### Port Already in Use
If port 5432 is already in use:
1. Stop local PostgreSQL service
2. Or change port in `docker-compose.yml`:
   ```yaml
   ports:
     - "5433:5432"  # Use 5433 instead
   ```
3. Update connection string accordingly

### Can't Connect from Application
- Ensure Docker is running
- Check firewall settings
- Verify connection string
- Check container logs: `docker-compose logs postgres`

### Database Not Initializing
- Check logs: `docker-compose logs postgres`
- Verify SQL files exist in `Database/` folder
- Try removing volumes: `docker-compose down -v`

### Performance Issues
- Allocate more memory to Docker Desktop
- Check Docker Desktop settings
- Consider using native PostgreSQL for production

## Production Use

⚠️ **Warning**: This setup is for development only!

For production:
- Use strong passwords
- Configure SSL/TLS
- Set up proper backup strategy
- Use managed database services
- Configure resource limits
- Enable authentication
- Restrict network access

## Additional Resources

- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [pgAdmin Documentation](https://www.pgadmin.org/docs/)
