using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ZXing.Net.Maui.Controls;

namespace Shortee
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBarcodeReader()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Feather.ttf", "Icons");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Register Views and ViewModels
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HistoryPage>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddTransient<ScanQRPage>();

            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddTransient<HistoryViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddTransient<ScanQRViewModel>();

            // Register Modals
            builder.Services.AddTransient<DetailsPage>();
            builder.Services.AddTransient<DetailsViewModel>();

            // Register other services
            builder.Services.AddSingleton<IUrlShortenerService, UrlShortenerService>();
            builder.Services.AddSingleton<IDataService, DataService>();

            return builder.Build();
        }
    }
}
