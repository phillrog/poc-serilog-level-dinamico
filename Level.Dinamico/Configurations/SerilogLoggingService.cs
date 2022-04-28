using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;

namespace Level.Dinamico.Configuration
{
    public interface ISerilogLoggingService
    {
        void SetLoggingLevel(LogEventLevel logEventLevel);
        LoggingLevelSwitch GetLoggingLevelSwitch();
    }

    public class SerilogLoggingService : ISerilogLoggingService
    {
        public readonly LoggingLevelSwitch _loggingLevelSwitch;

        public SerilogLoggingService()
        {
            _loggingLevelSwitch = new LoggingLevelSwitch();
        }

        public void SetLoggingLevel(LogEventLevel logEventLevel)
        {            
            _loggingLevelSwitch.MinimumLevel = logEventLevel;            
        }

        public LoggingLevelSwitch GetLoggingLevelSwitch()
        {
            return _loggingLevelSwitch;
        }
    }    
}
