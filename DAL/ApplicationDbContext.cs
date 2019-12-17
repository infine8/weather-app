using System.Data.Entity;
using MySql.Data.EntityFramework;
using WeatherApp.DAL.Models;

namespace WeatherApp.DAL
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<WeatherData> WeatherData { get; set; }

        public ApplicationDbContext(string connectionString) : base(connectionString)
        {
            Configuration.ValidateOnSaveEnabled = false;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Properties<string>().Configure(p => p.IsUnicode(true));
        }
    }
}
