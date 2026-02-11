# Contributing to Music Library

Thank you for your interest in contributing to the Music Library project! This document provides guidelines and instructions for contributing.

## Getting Started

### Prerequisites
- Windows 10 (version 1809+) or Windows 11
- Visual Studio 2022 (version 17.8 or later)
- .NET 8.0 SDK
- PostgreSQL 12 or later
- Git

### Development Setup

1. **Fork the Repository**
   ```bash
   # Fork on GitHub, then clone your fork
   git clone https://github.com/YOUR_USERNAME/MusicDB.git
   cd MusicDB
   ```

2. **Set Up Database**
   ```sql
   CREATE DATABASE musiclibrary_dev;
   ```

3. **Configure Connection**
   - Copy `appsettings.json` to `appsettings.Development.json`
   - Update with your local database credentials

4. **Restore and Build**
   ```bash
   dotnet restore
   dotnet build
   ```

5. **Run Migrations**
   ```bash
   dotnet ef database update --project ClerioVision.MusicDB
   ```

## Development Workflow

### Branch Strategy
- `main` - Production-ready code
- `develop` - Integration branch for features
- `feature/*` - New features
- `bugfix/*` - Bug fixes
- `hotfix/*` - Critical production fixes

### Making Changes

1. **Create a Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make Your Changes**
   - Write clean, readable code
   - Follow existing code style
   - Add comments for complex logic
   - Update documentation as needed

3. **Test Your Changes**
   - Build the solution
   - Run the application
   - Test all affected features
   - Verify no regressions

4. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "feat: add new feature description"
   ```

### Commit Message Format

Follow [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

**Examples:**
```
feat(search): add fuzzy search for track titles
fix(scanner): handle MP3 files with missing tags
docs(readme): update installation instructions
```

### Pull Request Process

1. **Update Your Branch**
   ```bash
   git checkout main
   git pull origin main
   git checkout feature/your-feature-name
   git rebase main
   ```

2. **Push Your Branch**
   ```bash
   git push origin feature/your-feature-name
   ```

3. **Create Pull Request**
   - Go to GitHub repository
   - Click "New Pull Request"
   - Select your branch
   - Fill out the PR template
   - Link related issues

4. **PR Review**
   - Wait for code review
   - Address feedback
   - Make requested changes
   - Push updates to same branch

5. **Merge**
   - PR will be merged by maintainers
   - Delete your branch after merge

## Code Style Guidelines

### C# Style
- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable names
- Keep methods focused and concise
- Maximum line length: 120 characters
- Use async/await for I/O operations

### XAML Style
- Use proper indentation (4 spaces)
- Group related properties
- Use meaningful x:Name values
- Keep UI logic in ViewModels

### Naming Conventions
- Classes: PascalCase (e.g., `MusicDbContext`)
- Methods: PascalCase (e.g., `ScanFolderAsync`)
- Private fields: _camelCase (e.g., `_scannerService`)
- Properties: PascalCase (e.g., `TotalArtists`)
- Local variables: camelCase (e.g., `trackCount`)
- Constants: PascalCase (e.g., `MaxRetryCount`)

### Documentation
- Use XML documentation comments for public APIs
- Document complex algorithms
- Explain "why" not just "what"
- Keep comments up-to-date

Example:
```csharp
/// <summary>
/// Scans a folder recursively for MP3 files and extracts metadata.
/// </summary>
/// <param name="folderPath">The root folder path to scan</param>
/// <param name="cancellationToken">Token to cancel the operation</param>
/// <returns>List of MP3 file information</returns>
public async Task<List<Mp3FileInfo>> ScanFolderAsync(
    string folderPath, 
    CancellationToken cancellationToken = default)
{
    // Implementation
}
```

## Testing Guidelines

### Unit Tests (Future)
- Write tests for business logic
- Use xUnit or NUnit
- Aim for high code coverage
- Mock external dependencies

### Integration Tests (Future)
- Test database operations
- Test service interactions
- Use test database

### Manual Testing
Required for all changes:
1. Build solution successfully
2. Run application
3. Test affected features
4. Test edge cases
5. Verify error handling

## Reporting Issues

### Bug Reports
Include:
- Clear description
- Steps to reproduce
- Expected behavior
- Actual behavior
- Screenshots if applicable
- Environment details (OS, .NET version, etc.)

### Feature Requests
Include:
- Clear description
- Use case/motivation
- Proposed solution (optional)
- Alternative solutions (optional)

## Areas for Contribution

### High Priority
- PDF report export
- HTML report export
- Improved error handling
- Performance optimizations
- Unit tests
- Integration tests

### Features
- Playlist management
- Music player integration
- Duplicate detection
- Batch tag editing
- Cloud backup
- Statistics dashboard

### Documentation
- More code examples
- Video tutorials
- Architecture diagrams
- API documentation

### UI/UX
- Accessibility improvements
- Dark theme refinements
- Animation improvements
- Responsive design enhancements

## Code of Conduct

### Our Standards
- Be respectful and inclusive
- Welcome newcomers
- Focus on constructive feedback
- Accept differing viewpoints
- Show empathy

### Unacceptable Behavior
- Harassment or discrimination
- Trolling or insulting comments
- Personal attacks
- Publishing private information
- Unprofessional conduct

## Questions?

- **General Questions**: Open a GitHub Discussion
- **Bug Reports**: Create an issue with bug template
- **Feature Requests**: Create an issue with feature template
- **Security Issues**: Email maintainers directly

## License

By contributing, you agree that your contributions will be licensed under the same license as the project (MIT License).

## Recognition

Contributors will be recognized in:
- README.md contributors section
- Release notes
- GitHub contributors page

Thank you for contributing to Music Library! 🎵
