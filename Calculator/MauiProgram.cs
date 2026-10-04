using Microsoft.Extensions.Logging;

namespace Calculator
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

#if ANDROID
            Microsoft.Maui.Handlers.LabelHandler.Mapper.AppendToMapping(
                "AutoSizeCalculatorDisplay",
                (handler, view) =>
                {
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                    {
                        handler.PlatformView.SetAutoSizeTextTypeUniformWithConfiguration(
                            1,
                            (int)view.Font.Size,
                            1,
                            (int)Android.Util.ComplexUnitType.Sp);
                    }
                });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
