-- Sample Data for Music Library Database
-- Use this to populate the database with test data

-- Connect to the database
\c musiclibrary

-- ============================================
-- Insert Sample Artists
-- ============================================

INSERT INTO "Artists" ("Name", "CreatedAt", "UpdatedAt") VALUES
('The Beatles', NOW(), NOW()),
('Pink Floyd', NOW(), NOW()),
('Led Zeppelin', NOW(), NOW()),
('Queen', NOW(), NOW()),
('The Rolling Stones', NOW(), NOW());

-- ============================================
-- Insert Sample Albums
-- ============================================

-- The Beatles Albums
INSERT INTO "Albums" ("Title", "ArtistId", "CreatedAt", "UpdatedAt") VALUES
('Abbey Road', 1, NOW(), NOW()),
('Sgt. Pepper''s Lonely Hearts Club Band', 1, NOW(), NOW()),
('Revolver', 1, NOW(), NOW());

-- Pink Floyd Albums
INSERT INTO "Albums" ("Title", "ArtistId", "CreatedAt", "UpdatedAt") VALUES
('The Dark Side of the Moon', 2, NOW(), NOW()),
('The Wall', 2, NOW(), NOW()),
('Wish You Were Here', 2, NOW(), NOW());

-- Led Zeppelin Albums
INSERT INTO "Albums" ("Title", "ArtistId", "CreatedAt", "UpdatedAt") VALUES
('Led Zeppelin IV', 3, NOW(), NOW()),
('Physical Graffiti', 3, NOW(), NOW());

-- Queen Albums
INSERT INTO "Albums" ("Title", "ArtistId", "CreatedAt", "UpdatedAt") VALUES
('A Night at the Opera', 4, NOW(), NOW()),
('News of the World', 4, NOW(), NOW());

-- The Rolling Stones Albums
INSERT INTO "Albums" ("Title", "ArtistId", "CreatedAt", "UpdatedAt") VALUES
('Sticky Fingers', 5, NOW(), NOW()),
('Exile on Main St.', 5, NOW(), NOW());

-- ============================================
-- Insert Sample Tracks
-- ============================================

-- Abbey Road tracks
INSERT INTO "Tracks" ("Title", "ArtistId", "AlbumId", "TrackNumber", "DurationSeconds", "FilePath", "CreatedAt", "UpdatedAt") VALUES
('Come Together', 1, 1, 1, 259, 'C:\Music\The Beatles\Abbey Road\01 Come Together.mp3', NOW(), NOW()),
('Something', 1, 1, 2, 183, 'C:\Music\The Beatles\Abbey Road\02 Something.mp3', NOW(), NOW()),
('Here Comes the Sun', 1, 1, 7, 185, 'C:\Music\The Beatles\Abbey Road\07 Here Comes the Sun.mp3', NOW(), NOW());

-- Sgt. Pepper's tracks
INSERT INTO "Tracks" ("Title", "ArtistId", "AlbumId", "TrackNumber", "DurationSeconds", "FilePath", "CreatedAt", "UpdatedAt") VALUES
('Sgt. Pepper''s Lonely Hearts Club Band', 1, 2, 1, 122, 'C:\Music\The Beatles\Sgt Peppers\01 Sgt Peppers.mp3', NOW(), NOW()),
('With a Little Help from My Friends', 1, 2, 2, 164, 'C:\Music\The Beatles\Sgt Peppers\02 With a Little Help.mp3', NOW(), NOW()),
('Lucy in the Sky with Diamonds', 1, 2, 3, 208, 'C:\Music\The Beatles\Sgt Peppers\03 Lucy in the Sky.mp3', NOW(), NOW());

-- Dark Side of the Moon tracks
INSERT INTO "Tracks" ("Title", "ArtistId", "AlbumId", "TrackNumber", "DurationSeconds", "FilePath", "CreatedAt", "UpdatedAt") VALUES
('Speak to Me', 2, 4, 1, 68, 'C:\Music\Pink Floyd\Dark Side\01 Speak to Me.mp3', NOW(), NOW()),
('Breathe', 2, 4, 2, 169, 'C:\Music\Pink Floyd\Dark Side\02 Breathe.mp3', NOW(), NOW()),
('Time', 2, 4, 4, 413, 'C:\Music\Pink Floyd\Dark Side\04 Time.mp3', NOW(), NOW()),
('Money', 2, 4, 6, 382, 'C:\Music\Pink Floyd\Dark Side\06 Money.mp3', NOW(), NOW());

-- The Wall tracks
INSERT INTO "Tracks" ("Title", "ArtistId", "AlbumId", "TrackNumber", "DurationSeconds", "FilePath", "CreatedAt", "UpdatedAt") VALUES
('Another Brick in the Wall (Part 2)', 2, 5, 3, 238, 'C:\Music\Pink Floyd\The Wall\03 Another Brick.mp3', NOW(), NOW()),
('Comfortably Numb', 2, 5, 6, 382, 'C:\Music\Pink Floyd\The Wall\06 Comfortably Numb.mp3', NOW(), NOW());

-- Led Zeppelin IV tracks
INSERT INTO "Tracks" ("Title", "ArtistId", "AlbumId", "TrackNumber", "DurationSeconds", "FilePath", "CreatedAt", "UpdatedAt") VALUES
('Black Dog', 3, 7, 1, 296, 'C:\Music\Led Zeppelin\Led Zeppelin IV\01 Black Dog.mp3', NOW(), NOW()),
('Rock and Roll', 3, 7, 2, 220, 'C:\Music\Led Zeppelin\Led Zeppelin IV\02 Rock and Roll.mp3', NOW(), NOW()),
('Stairway to Heaven', 3, 7, 4, 482, 'C:\Music\Led Zeppelin\Led Zeppelin IV\04 Stairway to Heaven.mp3', NOW(), NOW());

-- A Night at the Opera tracks
INSERT INTO "Tracks" ("Title", "ArtistId", "AlbumId", "TrackNumber", "DurationSeconds", "FilePath", "CreatedAt", "UpdatedAt") VALUES
('Death on Two Legs', 4, 9, 1, 223, 'C:\Music\Queen\A Night at the Opera\01 Death on Two Legs.mp3', NOW(), NOW()),
('Bohemian Rhapsody', 4, 9, 11, 355, 'C:\Music\Queen\A Night at the Opera\11 Bohemian Rhapsody.mp3', NOW(), NOW());

-- News of the World tracks
INSERT INTO "Tracks" ("Title", "ArtistId", "AlbumId", "TrackNumber", "DurationSeconds", "FilePath", "CreatedAt", "UpdatedAt") VALUES
('We Will Rock You', 4, 10, 1, 122, 'C:\Music\Queen\News of the World\01 We Will Rock You.mp3', NOW(), NOW()),
('We Are the Champions', 4, 10, 2, 179, 'C:\Music\Queen\News of the World\02 We Are the Champions.mp3', NOW(), NOW());

-- Sticky Fingers tracks
INSERT INTO "Tracks" ("Title", "ArtistId", "AlbumId", "TrackNumber", "DurationSeconds", "FilePath", "CreatedAt", "UpdatedAt") VALUES
('Brown Sugar', 5, 11, 1, 229, 'C:\Music\Rolling Stones\Sticky Fingers\01 Brown Sugar.mp3', NOW(), NOW()),
('Wild Horses', 5, 11, 3, 343, 'C:\Music\Rolling Stones\Sticky Fingers\03 Wild Horses.mp3', NOW(), NOW());

-- ============================================
-- Verify Data
-- ============================================

-- Count records
SELECT 
    (SELECT COUNT(*) FROM "Artists") as artists,
    (SELECT COUNT(*) FROM "Albums") as albums,
    (SELECT COUNT(*) FROM "Tracks") as tracks;

-- List all artists with album count
SELECT 
    a."Name" as artist,
    COUNT(al."AlbumId") as album_count
FROM "Artists" a
LEFT JOIN "Albums" al ON a."ArtistId" = al."ArtistId"
GROUP BY a."Name"
ORDER BY a."Name";

-- List all albums with track count
SELECT 
    ar."Name" as artist,
    al."Title" as album,
    COUNT(t."TrackId") as track_count
FROM "Albums" al
JOIN "Artists" ar ON al."ArtistId" = ar."ArtistId"
LEFT JOIN "Tracks" t ON al."AlbumId" = t."AlbumId"
GROUP BY ar."Name", al."Title"
ORDER BY ar."Name", al."Title";
