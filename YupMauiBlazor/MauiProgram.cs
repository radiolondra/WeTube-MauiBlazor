using Microsoft.AspNetCore.Components.WebView.Maui;
using YupMauiBlazor.Data;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using BlazorSpinner;

namespace YupMauiBlazor;

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
			});

		builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		
		builder.Services.AddSingleton<WeatherForecastService>();

        builder.Services
			.AddBlazorise(options =>
			{
				options.Immediate = true;
			})
			.AddBootstrapProviders()			
			.AddFontAwesomeIcons();


        builder.Services.AddScoped<SpinnerService>();

        return builder.Build();
	}
}
