using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using WeatherApp.JobService.Commands;

namespace WeatherApp.JobService
{
    public class App
    {
        private GrabPopularCitiesWeatherCommand _grabWeatherCommand;


        public App(GrabPopularCitiesWeatherCommand grabWeatherCommand)
        {
            _grabWeatherCommand = grabWeatherCommand;
        }


        public async Task Run(string[] args)
        {
            var app = new CommandLineApplication { Name = "JobService" };
            app.HelpOption("-?|-h|--help");

            app.OnExecute(async () =>
            {

                return 0;
            });


            app.Command("grab-weather", _grabWeatherCommand.SetCommand);

            app.Execute(args);
        }
    }
}
