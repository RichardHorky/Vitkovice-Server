using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Web;
using NLog.Config;

namespace V.Server
{
    public class Program
    {
        private const string _NLOG_FILENAME = "NLog.config";
        private const string _NLOG_PATERN_FILENAME = "NLog.patern";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog();

        private static void ConfigNLogger(IConfigurationRoot config)
        {
            if (!File.Exists(_NLOG_FILENAME))
                File.Copy(_NLOG_PATERN_FILENAME, _NLOG_FILENAME);
            NLog.LogManager.Configuration = new XmlLoggingConfiguration(_NLOG_FILENAME);
        }
    }
}
