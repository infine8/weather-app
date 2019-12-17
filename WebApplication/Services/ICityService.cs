using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WeatherApp.WebApplication.Models;

namespace WeatherApp.WebApplication.Services
{
    [ServiceContract]
    public interface ICityService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetCityListAsync/")]
        Task<IEnumerable<CityViewModel>> GetCityListAsync();
    }
}
