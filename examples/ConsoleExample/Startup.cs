using System;
using System.Threading.Tasks;
using ConsoleApp.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleExample
{
    public class Startup : IStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public Task StartAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();
            logger.LogInformation($"Hello from {nameof(ConsoleExample)}!");

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            logger.LogInformation($"Here is configuration value readed from appsettings.json: {configuration["someSetting"]}");

            return Task.CompletedTask;
        }
    }
}
