using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyConverterService
{
    class Logging 
    {
        ServiceProvider serviceProvider { get; set; }
        ILogger logger { get; set; }
        public Logging()
        {
            this.serviceProvider = new ServiceCollection()
                .AddLogging(cfg => cfg.AddConsole())
                .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Debug)
                .BuildServiceProvider();
            this.logger = serviceProvider
                .GetService<ILogger<Program>>();
        }

        public void AddInformation(string message)
        {
            logger.LogInformation($"{DateTime.Now}: {message}");
        }

        public void AddWarning(string message)
        {
            logger.LogWarning($"{DateTime.Now}: {message}");
        }

        public void AddError(string message)
        {
            logger.LogError($"{DateTime.Now}: {message}");
        }
    }
}
