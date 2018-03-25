using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Host.SystemWeb;
using System.Threading.Tasks;
using System.Diagnostics;
using Serilog.Events;
using System.Net.Http;
using Microsoft.Owin;
using Owin;
using WebApplication.log;
using StructureMap;
using WebApplication.App_Start;

namespace WebApplication.Middleware
{
    internal static class LoggingMiddlewareHandler
    {
        public static IAppBuilder UseLoggingMiddleware(this IAppBuilder app)
        {
            app.MapWhen(context => context.Request.Path.StartsWithSegments(new PathString("/api")), 
                    appBuilder => 
                        {
                            appBuilder.Use(typeof(LoggingMiddleware));
                        });
            return app;
        }
    }

    public class LoggingMiddleware :  OwinMiddleware
    {
        const string MessageTemplate =
            "{ID} - HTTP {RequestMethod} {RequestPath} responded . {StatusCode} in {Elapsed:0.0000} ms";

        public LoggingMiddleware(OwinMiddleware next) :
         base(next)
        {
            
        }

        public override async Task Invoke(IOwinContext context)
        {
            ILogger logger = StructuremapMvc.StructureMapDependencyScope.GetInstance<ILogger>();
            string traceId = Guid.NewGuid().ToString("N");
            traceId = $"{traceId.Substring(0, 5)}{traceId.Substring(traceId.Length - 5, 5)}";

            var sw = Stopwatch.StartNew();
            try
            {
                context.Set("TraceID", traceId);

                await Next.Invoke(context);
                sw.Stop();

                var statusCode = context.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                ILogger log = level == LogEventLevel.Error ? LogForErrorContext(context, logger) : logger;
                log.Write(level, MessageTemplate, traceId, context.Request.Method, context.Request.Path, statusCode, sw.Elapsed.TotalMilliseconds);

            }
            catch (Exception ex) when (LogException(context, traceId, logger, sw, ex)) { }
        }

        static bool LogException(IOwinContext httpContext, string traceId, ILogger logger, Stopwatch sw, Exception ex)
        {
            sw.Stop();

            LogForErrorContext(httpContext, logger)
                .Error(ex, MessageTemplate, traceId, httpContext.Request.Method, httpContext.Request.Path, 500, sw.Elapsed.TotalMilliseconds);
            
            return false;
        }

        static ILogger LogForErrorContext(IOwinContext context, ILogger logger)
        {
            var request = context.Request;

            var result = logger
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            return result;
        }

    }
}