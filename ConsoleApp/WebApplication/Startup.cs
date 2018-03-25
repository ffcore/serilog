using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using WebApplication.App_Start;
using WebApplication.Middleware;

[assembly: OwinStartup(typeof(WebApplication.Startup))]

namespace WebApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseLoggingMiddleware();
        }
    }
}
