using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Level.Dinamico.Configuration
{
    public class SerilogHttpContextEnricher : ILogEventEnricher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Action<LogEvent, ILogEventPropertyFactory, HttpContext> _enrichAction;

        public SerilogHttpContextEnricher(IServiceProvider serviceProvider) : this(serviceProvider, null)
        { }

        public SerilogHttpContextEnricher(IServiceProvider serviceProvider, Action<LogEvent, ILogEventPropertyFactory, HttpContext> enrichAction)
        {
            _serviceProvider = serviceProvider;
            if (enrichAction == null)
            {
                _enrichAction = (logEvent, propertyFactory, httpContext) =>
                {
                    if (httpContext.User.Claims.Count() > 0)
                    {
                        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_usuario_projur",
                            httpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value));
                    }
                    else
                    {
                        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_web_hook","api_digesto"));
                    }
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_host", httpContext.Request.Host));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_origin", httpContext.Request.Headers["Origin"]));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_path", httpContext.Request.Path));
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("request_method", httpContext.Request.Method));
                    if (httpContext.Response.HasStarted)
                    {
                        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("response_status", httpContext.Response.StatusCode));
                    }
                };
            }
            else
            {
                _enrichAction = enrichAction;
            }
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext;
            if (null != httpContext)
            {
                _enrichAction.Invoke(logEvent, propertyFactory, httpContext);
            }
        }
    }

    public class HttpContextLogMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpContextLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var serviceProvider = context.RequestServices;
            using (LogContext.Push(new SerilogHttpContextEnricher(serviceProvider)))
            {
                await _next(context);
            }
        }
    }
}
