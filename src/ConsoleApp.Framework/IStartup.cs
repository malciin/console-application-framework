using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Framework
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);

        Task StartAsync(IServiceProvider serviceProvider);
    }
}
