using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CurrencyConverterService
{
    public class Logging 
    {
        ServiceProvider ServiceProvider { get; set; }
        ILogger Logger { get; set; }
        public Logging()
        {
            this.ServiceProvider = new ServiceCollection()
                .AddLogging(cfg => cfg.AddConsole())
                .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Debug)
                .BuildServiceProvider();
            this.Logger = ServiceProvider
                .GetService<ILogger<Program>>();
        }

        public void AddInformation(string message)
        {
            Logger.LogInformation($"{DateTime.Now}: {message}");
        }

        public void AddWarning(string message)
        {
            Logger.LogWarning($"{DateTime.Now}: {message}");
        }

        public void AddError(string message)
        {
            Logger.LogError($"{DateTime.Now}: {message}");
        }
    }
}
