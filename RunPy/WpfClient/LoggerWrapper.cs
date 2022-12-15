using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient
{
    public class LoggerWrapper : ILoggerWrapper
    {
        ILogger logger;
        public LoggerWrapper()
        {
            logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File("Logs", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();
        }

        public void Info(string message)
        {
            logger.Information(message);
        }

        public void Error(Exception x)
        {
            logger.Error(x.Message, x);
        }
    }
}
