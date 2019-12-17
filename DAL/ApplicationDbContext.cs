using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.EntityFramework;
using WeatherApp.DAL.Models;

namespace WeatherApp.DAL
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<WeatherData> WeatherData { get; set; }

        public ApplicationDbContext() : base("DefaultConnection")
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
