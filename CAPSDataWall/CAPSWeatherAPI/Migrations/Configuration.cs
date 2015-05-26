
namespace CAPSWeatherAPI.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Contexts.WeatherDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Contexts.WeatherDataContext context)
        {
        }
    }
}
