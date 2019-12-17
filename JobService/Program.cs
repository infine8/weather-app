using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.DAL;
using WeatherApp.JobService.Commands;

namespace WeatherApp.JobService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await MainAsync(args);
        }


        static async Task MainAsync(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            await serviceProvider.GetService<App>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(provider => new ApplicationDbContext("DefaultConnection"));

            serviceCollection.AddTransient<GrabPopularCitiesWeatherCommand>();

            serviceCollection.AddTransient<App>();
        }
    }
}
