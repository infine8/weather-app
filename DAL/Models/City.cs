using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApp.DAL.Models
{
    [Table("app_city")]
    public class City : BaseDbEntity
    {
        [Column("name"), MaxLength(256), Required]
        public string Name { get; set; }
    }
}
