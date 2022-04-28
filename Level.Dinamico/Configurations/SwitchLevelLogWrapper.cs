using Serilog.Core;
using Serilog.Events;

namespace Level.Dinamico.Configuration
{
    public class SwitchLevelLogWrapper : ILogEventSink, IDisposable
    {
        private readonly ILogEventSink _wrappedSink;
        private readonly IServiceCollection _services;

        public SwitchLevelLogWrapper(ILogEventSink wrappedSink,
            IServiceCollection services)
        {
            _wrappedSink = wrappedSink;
            _services = services;
        }

        public void Emit(LogEvent logEvent)
        {
            var logSwitch = _services.BuildServiceProvider().GetService<ISerilogLoggingService>();

            var boosted = new LogEvent(
                    logEvent.Timestamp,
                    logSwitch.GetLoggingLevelSwitch().MinimumLevel,
                    logEvent.Exception,
                    logEvent.MessageTemplate,
                    logEvent.Properties
                        .Select(kvp => new LogEventProperty(kvp.Key, kvp.Value)));

            _wrappedSink.Emit(boosted);           
        }

        public void Dispose()
        {
            (_wrappedSink as IDisposable)?.Dispose();
        }
    }
}
