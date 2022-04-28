using Serilog;
using Serilog.Events;

namespace Level.Dinamico.Configuration
{
    public static class SerilogConfiguration
    {
        public static IApplicationBuilder UseHttpContextLog(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpContextLogMiddleware>();
        }
        public static void AddConfigSerilog(this IServiceCollection services)
        {
            services.AddSingleton(Log.Logger);
        }

        public static void UseSerilogConfig(this IHostBuilder app, IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<ISerilogLoggingService>(new SerilogLoggingService());
            var _loggingService = services.BuildServiceProvider().GetService<ISerilogLoggingService>();
            app.ConfigureAppConfiguration((ctx, config) =>
                {
                    var settings = config.Build();
                    var minLevelDefault = configuration.GetSection("Serilog:MinimumLevel:Default");

                    _loggingService.SetLoggingLevel((LogEventLevel)Enum.Parse(typeof(LogEventLevel), minLevelDefault.Value, true));

                    Serilog.Log.Logger = new LoggerConfiguration()
                        .Destructure.ByTransforming<HttpRequest>(
                        r => new { Path = r.Path, Body = r.Body, Method = r.Method })
                        .MinimumLevel.ControlledBy(_loggingService.GetLoggingLevelSwitch())
                        .WriteTo.Switch(wt => wt.Console(levelSwitch: _loggingService.GetLoggingLevelSwitch()), services)
                        .ReadFrom.Configuration(configuration)
                        .CreateBootstrapLogger();

                    Log.Information("Iniciando Aplicação com Serilog");
                }); /*.UseSerilog();*/
        }
    }
}
