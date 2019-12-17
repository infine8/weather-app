using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WeatherApp.WebApplication.Models;

namespace WeatherApp.WebApplication.Services
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetWeatherDataAsync/")]
        Task<WeatherDataViewModel> GetWeatherDataAsync(long cityId, DateTime date);
    }
}
