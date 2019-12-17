namespace WeatherApp.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "app_city",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)                ;
            
            CreateTable(
                "app_weather_data",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        date_utc = c.DateTime(nullable: false, precision: 0),
                        city_id = c.Long(nullable: false),
                        min_temp = c.Int(nullable: false),
                        max_temp = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)                
                .ForeignKey("app_city", t => t.city_id, cascadeDelete: true)
                //.Index(t => t.city_id)
                ;
            
        }
        
        public override void Down()
        {
            DropForeignKey("app_weather_data", "city_id", "app_city");
            DropIndex("app_weather_data", new[] { "city_id" });
            DropTable("app_weather_data");
            DropTable("app_city");
        }
    }
}
