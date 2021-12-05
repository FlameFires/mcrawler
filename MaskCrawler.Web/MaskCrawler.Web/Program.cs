using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;

using System.Runtime.InteropServices;

namespace MaskCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    //var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

                    //// IMPORTANT: This needs to be added *before* configuration is loaded, this lets
                    //// the defaults be overridden by the configuration.
                    //if (isWindows)
                    //{
                    //    // Default the EventLogLoggerProvider to warning or above
                    //    logging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Warning);
                    //}

                    //logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    //logging.AddConsole();
                    //logging.AddDebug();
                    //logging.AddEventSourceLogger();

                    //if (isWindows)
                    //{
                    //    // Add the EventLogLoggerProvider on windows machines
                    //    logging.AddEventLog();
                    //}
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
