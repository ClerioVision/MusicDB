-- Music Library Database Setup Script
-- PostgreSQL 12+
-- This script creates the database and user for the Music Library application

-- ============================================
-- 1. Create Database
-- ============================================

-- Run this as postgres superuser or with CREATE DATABASE privilege
CREATE DATABASE musiclibrary
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.UTF-8'
    LC_CTYPE = 'en_US.UTF-8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

COMMENT ON DATABASE musiclibrary IS 'Music Library application database';

-- ============================================
-- 2. Create Application User (Optional but recommended)
-- ============================================

-- Create a dedicated user for the application
CREATE USER musicuser WITH 
    LOGIN
    PASSWORD 'ChangeThisPassword123!'
    NOSUPERUSER
    NOCREATEDB
    NOCREATEROLE;

-- Grant privileges
GRANT CONNECT ON DATABASE musiclibrary TO musicuser;
GRANT USAGE ON SCHEMA public TO musicuser;
GRANT CREATE ON SCHEMA public TO musicuser;

-- ============================================
-- 3. Connect to the database
-- ============================================

\c musiclibrary

-- ============================================
-- 4. Grant Table Privileges (after EF creates tables)
-- ============================================

-- Run these after the application creates the tables:
-- GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO musicuser;
-- GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO musicuser;

-- ============================================
-- 5. Enable Extensions (if needed)
-- ============================================

-- For full-text search (optional)
-- CREATE EXTENSION IF NOT EXISTS pg_trgm;

-- For UUID support (if needed in future)
-- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- ============================================
-- 6. Create Indexes (Optional - EF creates these)
-- ============================================

-- The Entity Framework migrations will create these,
-- but you can manually create additional indexes if needed:

-- CREATE INDEX idx_artists_name_trgm ON Artists USING gin(Name gin_trgm_ops);
-- CREATE INDEX idx_albums_title_trgm ON Albums USING gin(Title gin_trgm_ops);
-- CREATE INDEX idx_tracks_title_trgm ON Tracks USING gin(Title gin_trgm_ops);

-- ============================================
-- 7. Verify Setup
-- ============================================

-- Check database
SELECT datname, pg_encoding_to_char(encoding), datcollate, datctype 
FROM pg_database 
WHERE datname = 'musiclibrary';

-- Check user
SELECT usename, usecreatedb, usesuper 
FROM pg_user 
WHERE usename = 'musicuser';

-- ============================================
-- Connection String Examples
-- ============================================

/*
For postgres user:
Host=localhost;Port=5432;Database=musiclibrary;Username=postgres;Password=your_postgres_password

For dedicated user:
Host=localhost;Port=5432;Database=musiclibrary;Username=musicuser;Password=ChangeThisPassword123!

For Docker PostgreSQL:
Host=localhost;Port=5432;Database=musiclibrary;Username=postgres;Password=postgres

For remote server:
Host=your-server.com;Port=5432;Database=musiclibrary;Username=musicuser;Password=your_password;SSL Mode=Require
*/
