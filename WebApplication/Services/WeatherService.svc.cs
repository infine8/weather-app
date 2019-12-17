using System;
using System.Data.Entity;
using System.Threading.Tasks;
using WeatherApp.DAL;
using WeatherApp.WebApplication.Models;

namespace WeatherApp.WebApplication.Services
{
    public class WeatherService : IWeatherService
    {
        public async Task<WeatherDataViewModel> GetWeatherDataAsync(long cityId, DateTime date)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var data = await dbContext.WeatherData.FirstOrDefaultAsync(x => x.CityId == cityId && x.DateUtc == date.Date);

                if (data == null) return null;

                return new WeatherDataViewModel { CityId = data.CityId, Date = data.DateUtc, MinTemp = data.MinTemp, MaxTemp = data.MaxTemp};
            }
        }
    }
}
