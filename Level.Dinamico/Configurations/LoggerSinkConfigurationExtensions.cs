using Serilog;
using Serilog.Configuration;

namespace Level.Dinamico.Configuration
{
    public static class LoggerSinkConfigurationExtensions
    {
        public static LoggerConfiguration Switch(
            this LoggerSinkConfiguration lsc,
            Action<LoggerSinkConfiguration> writeTo, IServiceCollection services)
        {
            return LoggerSinkConfiguration.Wrap(
                lsc,
                wrapped => new SwitchLevelLogWrapper(wrapped, services),
                writeTo);
        }
    }
}
