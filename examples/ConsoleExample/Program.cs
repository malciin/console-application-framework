using System.Threading.Tasks;
using ConsoleApp.Framework;

namespace ConsoleExample
{
    public class Program
    {
        static Task Main(string[] args)
        {
            return ConsoleApplication
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .RunAsync();
        }
    }
}
