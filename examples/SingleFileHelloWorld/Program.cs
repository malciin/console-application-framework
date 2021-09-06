using System;
using System.Threading.Tasks;
using ConsoleApp.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace SingleFileHelloWorld
{
    public class Program : IStartup
    {
        static Task Main(string[] args)
            => ConsoleApplication.CreateDefaultBuilder(args).UseStartup<Program>().Build().RunAsync();

        public void ConfigureServices(IServiceCollection services) { }

        public Task StartAsync(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Hello world!");

            return Task.CompletedTask;
        }
    }
}
