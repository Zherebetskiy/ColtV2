using Colt.Application;
using Colt.Infrastructure;
using Colt.UI.Desktop.Helpers;
using Colt.UI.Desktop.ViewModels.Products;
using CommunityToolkit.Maui;
using Microcharts.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Globalization;

namespace Colt.UI.Desktop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            builder.Configuration.AddConfiguration(config);
            builder.Services.AddSingleton<IConfiguration>(config);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(config);
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddTransient<AddProductViewModel>();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(Path.Combine(FileSystem.Current.AppDataDirectory, "logs.txt"))
                .WriteTo.MSSqlServer(
                    connectionString: config.GetConnectionString("DefaultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents" },
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            #if DEBUG
            builder.Logging.AddDebug();
            #endif

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("uk-UA");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("uk-UA");

            var app = builder.Build();

            ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
