using System;
using System.Collections.Concurrent;
using System.Linq;
using ConsoleApp.Framework.Extensions;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Framework
{
    public class InstantConsoleLogger : ILogger
    {
        public InstantConsoleLogger(string categoryName, object synchronizationObject)
        {
            this.categoryName = categoryName;
            this.synchronizationObject = synchronizationObject;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            lock (synchronizationObject)
            {
                Console.Write($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} ");
                WriteColoredLogLevel(logLevel);
                Console.WriteLine($" {categoryName.Split('.').Last()}: {formatter(state, exception)}");

                int indent = 0;
                while (exception != null)
                {
                    Console.WriteLine(" ".Repeat(indent) + $"Exception fullname: {exception.GetType().FullName}");
                    Console.WriteLine(" ".Repeat(indent) + "Exception message:");
                    Console.WriteLine(" ".Repeat(indent) + exception.Message);
                    Console.WriteLine(" ".Repeat(indent) + "Exception stacktrace:");
                    Console.WriteLine(" ".Repeat(indent) + exception.StackTrace);

                    exception = exception.InnerException;

                    if (exception != null)
                    {
                        indent += 4;
                        Console.WriteLine(" ".Repeat(indent) + "Inner exception");
                    }
                }
            }
        }

        private void WriteColoredLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogLevel.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.Write($"[{logLevel}]");

            Console.ResetColor();
        }

        private readonly string categoryName;
        private readonly object synchronizationObject;
    }

    public class InstantConsoleLoggerProvider : ILoggerProvider
    {
        public InstantConsoleLoggerProvider()
        {
            loggers = new ConcurrentDictionary<string, InstantConsoleLogger>();
            synchronizationObject = new object();
        }

        public ILogger CreateLogger(string categoryName)
            => loggers.GetOrAdd(categoryName, new InstantConsoleLogger(categoryName, synchronizationObject));

        public void Dispose() { }

        private readonly ConcurrentDictionary<string, InstantConsoleLogger> loggers;
        private readonly object synchronizationObject;
    }
}
