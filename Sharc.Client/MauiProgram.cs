using Microsoft.Extensions.Logging;
using Sharc.Client.Pages;
using Sharc.Client.Services;
using Syncfusion.Maui.Core.Hosting;

namespace Sharc.Client;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder.ConfigureSyncfusionCore();
        Licensing.AddLicense();

        builder.Services.AddScoped<EventService>();
        builder.Services.AddTransient<CalendarPage>();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}