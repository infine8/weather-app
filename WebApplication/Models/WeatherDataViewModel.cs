using System;

namespace WeatherApp.WebApplication.Models
{
    public class WeatherDataViewModel
    {
        public long CityId { get; set; }
        public DateTime Date { get; set; }
        public int MinTemp { get; set; }
        public int MaxTemp { get; set; }
    }
}