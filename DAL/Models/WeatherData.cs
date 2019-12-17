using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.DAL.Models
{
    [Table("app_weather_data")]
    public class WeatherData : BaseDbEntity
    {
        [Column("date_utc"), Required]
        public DateTime DateUtc { get; set; }

        [Column("city_id"), Required]
        public long CityId { get; set; }

        public City City { get; set; }

        [Column("min_temp"), Required]
        public int MinTemp { get; set; }

        [Column("max_temp"), Required]
        public int MaxTemp { get; set; }
    }
}
