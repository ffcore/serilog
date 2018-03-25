using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication.log
{
    public static class LoggerFactory
    {
        public static ILogger CreateLogger(string appsettingsPath)
        {

            var configuration = new ConfigurationBuilder()
              .AddJsonFile(Path.Combine(appsettingsPath, "appsettings.json"))
              .Build();

            return new LoggerConfiguration()
              .ReadFrom.Configuration(configuration)
              .CreateLogger();
        }
    }
}