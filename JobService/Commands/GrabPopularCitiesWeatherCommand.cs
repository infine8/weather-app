using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.CommandLineUtils;
using WeatherApp.DAL;
using WeatherApp.DAL.Models;

namespace WeatherApp.JobService.Commands
{
    public class GrabPopularCitiesWeatherCommand : BaseCommand<GrabPopularCitiesWeatherCommand>
    {
        public static readonly string DOMAIN_URL = "https://www.gismeteo.ru/";

        private Dictionary<string, string> CityWeatherUrlDict { get; } = new Dictionary<string, string>();

        private List<City> Cities { get; set; }

        public GrabPopularCitiesWeatherCommand(ApplicationDbContext dbContext) : base(dbContext) { }

        public override void SetCommand(CommandLineApplication cmd)
        {
            base.SetCommand(cmd);

            cmd.OnExecute(async () =>
            {
                await Update();

                return 0;
            });
        }

        private async Task Update()
        {
            await ParseCities();

            foreach (var item in CityWeatherUrlDict)
            {
                await ParseCityWeather(item.Key, item.Value);
            }
        }


        private async Task ParseCities()
        {
            Cities = await DbContext.Cities.ToListAsync();

            var doc = new HtmlWeb().Load(DOMAIN_URL);

            foreach (var node in doc.DocumentNode.SelectNodes("//noscript[@id='noscript']/a"))
            {
                var name = node.Attributes["data-name"]?.Value.Trim();
                if (string.IsNullOrEmpty(name)) continue;

                CityWeatherUrlDict[name] = $"{DOMAIN_URL}{node.Attributes["data-url"].Value}10-days/";

                if (Cities.Any(x => x.Name == name)) continue;

                var city = new City { Name = name };
                Cities.Add(city);
                DbContext.Cities.Add(city);
            }

            await DbContext.SaveChangesAsync();
        }


        private async Task ParseCityWeather(string name, string url)
        {
            var city = Cities.First(x => x.Name == name);

            var doc = new HtmlWeb().Load(url);

            var date = ParseWeatherFirstDate(doc.DocumentNode.SelectSingleNode("//div[@class='widget__row widget__row_date']/div[@class='widget__item']/div/a/span")?.InnerHtml.Trim());


            foreach (var item in doc.DocumentNode.SelectSingleNode("//div[@class='chart chart__temperature']").SelectNodes("div[@class='values']/div"))
            {
                int.TryParse(item.SelectSingleNode("div[@class='maxt']/span[@class='unit unit_temperature_c']")?.InnerHtml.Trim().Replace("&minus;", "-"), out var maxTemp);
                int.TryParse(item.SelectSingleNode("div[@class='mint']/span[@class='unit unit_temperature_c']")?.InnerHtml.Trim().Replace("&minus;", "-"), out var minTemp);

                var weatherData = new WeatherData { CityId = city.Id, DateUtc = date, MaxTemp = maxTemp, MinTemp = minTemp };

                date = date.AddDays(1);

                if (await DbContext.WeatherData.AnyAsync(x => x.CityId == weatherData.CityId && x.DateUtc == weatherData.DateUtc)) continue;

                DbContext.WeatherData.Add(weatherData);
            }

            await DbContext.SaveChangesAsync();
        }


        private DateTime ParseWeatherFirstDate(string text)
        {
            var digitMatch = Regex.Match(text.Trim(), @"^\d+");

            if (!digitMatch.Success) return DateTime.Now.Date;

            var digit = int.Parse(digitMatch.Value);

            if (digit == DateTime.Now.Date.Day) return DateTime.Now.Date;
            if (digit == DateTime.Now.Date.AddDays(-1).Day) return DateTime.Now.AddDays(-1).Date;
            if (digit == DateTime.Now.Date.AddDays(1).Day) return DateTime.Now.AddDays(1).Date;

            return DateTime.Now.Date;
        }
    }
}
