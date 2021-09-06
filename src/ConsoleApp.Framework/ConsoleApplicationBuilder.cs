using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Framework
{
    public class ConsoleApplicationBuilder
    {
        private readonly ServiceCollection serviceCollection;
        private readonly IConfigurationBuilder configurationBuilder;
        private Type startupClassType;
        private Action<ILoggingBuilder> configureLoggingDelegate;

        public ConsoleApplicationBuilder(string[] args)
        {
            serviceCollection = new ServiceCollection();
            configurationBuilder = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true);
        }

        public ConsoleApplicationBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            configureDelegate(configurationBuilder);

            return this;
        }

        public ConsoleApplicationBuilder ConfigureLogging(Action<ILoggingBuilder> configureDelegate)
        {
            configureLoggingDelegate = configureDelegate;

            return this;
        }

        public ConsoleApplicationBuilder UseStartup<T>() where T : IStartup
        {
            startupClassType = typeof(T);

            return this;
        }

        public ConsoleApplication Build()
        {
            return new ConsoleApplication
            {
                ConfigurationBuilder = configurationBuilder,
                ConfigureLoggingDelegate = configureLoggingDelegate,
                StartupClassType = startupClassType,
                ServiceCollection = serviceCollection
            };
        }
    }
}
