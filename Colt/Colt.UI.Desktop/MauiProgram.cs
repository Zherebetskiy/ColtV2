using Colt.Application;
using Colt.Infrastructure;
using Colt.UI.Desktop.Helpers;
using Colt.UI.Desktop.ViewModels.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Colt.UI.Desktop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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

            builder.Services.AddTransient<AddProductViewModel>();

            #if DEBUG
            builder.Logging.AddDebug();
            #endif

            var app = builder.Build();

            ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
