using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using ClerioVision.MusicDB.Data;
using ClerioVision.MusicDB.Services;
using ClerioVision.MusicDB.ViewModels;

namespace ClerioVision.MusicDB;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    public static Window? MainWindow { get; private set; }
    public static IServiceProvider? ServiceProvider { get; private set; }

    public App()
    {
        InitializeComponent();
        ConfigureServices();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();
        MainWindow.Activate();
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        // Configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        // Database
        var connectionString = configuration.GetConnectionString("MusicDatabase") 
            ?? throw new InvalidOperationException("Connection string 'MusicDatabase' not found.");

        services.AddDbContext<MusicDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Services
        services.AddSingleton<Mp3ScannerService>();
        services.AddScoped<DatabaseService>();
        services.AddScoped<SearchService>();
        services.AddScoped<ReportService>();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<LibraryViewModel>();
        services.AddTransient<SearchViewModel>();
        services.AddTransient<ReportsViewModel>();
        services.AddTransient<SettingsViewModel>();

        ServiceProvider = services.BuildServiceProvider();
    }

    public static T GetService<T>() where T : class
    {
        if (ServiceProvider == null)
            throw new InvalidOperationException("ServiceProvider is not initialized");

        return ServiceProvider.GetRequiredService<T>();
    }
}
