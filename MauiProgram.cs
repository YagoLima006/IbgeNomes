using IbgeNomes.Services;
using IbgeNomes.ViewModels;
using IbgeNomes.Views;
using Microsoft.Extensions.Logging;

namespace IbgeNomes;

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

        // ---------- Camada de Serviços ----------
        // Um único HttpClient para o app todo (boa prática: evita esgotar conexões).
        builder.Services.AddSingleton(_ =>
        {
            var http = new HttpClient
            {
                BaseAddress = new Uri(IbgeApi.BaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
            http.DefaultRequestHeaders.UserAgent.ParseAdd("IbgeNomesApp/1.0");
            http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            return http;
        });
        builder.Services.AddSingleton<ILocalidadesService, LocalidadesService>();
        builder.Services.AddSingleton<INomesService, NomesService>();

        // ---------- Camada de Apresentação ----------
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
