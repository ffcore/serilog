using Serilog;
using System;
using System.IO;

namespace ConsoleAppWithConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.AppSettings()
               .CreateLogger();

            for (int i = 0; i < 10000; ++i)
            {
                Log.Information("Hello, world!");

                int a = 10, b = 0;
                try
                {
                    Log.Debug("Dividing {A} by {B}", a, b);
                    Console.WriteLine(a / b);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Something went wrong");
                }
            }

            Log.CloseAndFlush();
             Console.ReadKey();
            
        }
    }
}
