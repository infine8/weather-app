using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.DAL;
using WeatherApp.WebApplication.Models;

namespace WeatherApp.WebApplication.Services
{
    public class CityService : ICityService
    {
        public async Task<IEnumerable<CityViewModel>> GetCityListAsync()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var data = await dbContext.Cities.ToListAsync();

                return data.Select(x => new CityViewModel { Id = x.Id, Name = x.Name });
            }
        }
    }
}
