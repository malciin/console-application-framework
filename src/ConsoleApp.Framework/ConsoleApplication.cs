using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Framework
{
    public class ConsoleApplication
    {
        internal ConsoleApplication() { }

        internal Type StartupClassType { get; set; }

        internal IConfigurationBuilder ConfigurationBuilder { get; set; }

        internal Action<ILoggingBuilder> ConfigureLoggingDelegate { get; set; }

        internal ServiceCollection ServiceCollection { get; set; }

        public async Task RunAsync()
        {
            var startupInstance = Activator.CreateInstance(StartupClassType) as IStartup;

            ServiceCollection.AddSingleton<IConfiguration>(ConfigurationBuilder.Build());
            ServiceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddProvider(new InstantConsoleLoggerProvider());
                ConfigureLoggingDelegate?.Invoke(loggingBuilder);
            });
            startupInstance.ConfigureServices(ServiceCollection);

            using (var serviceProvider = ServiceCollection.BuildServiceProvider())
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                await startupInstance.StartAsync(serviceScope.ServiceProvider);
            }
        }

        public static ConsoleApplicationBuilder CreateDefaultBuilder(string[] args)
        {
            return new ConsoleApplicationBuilder(args);
        }
    }
}
