using Microsoft.Extensions.CommandLineUtils;
using WeatherApp.DAL;

namespace WeatherApp.JobService.Commands
{
    public abstract class BaseCommand<T> where T : class
    {
        protected ApplicationDbContext DbContext { get; set; }

        protected BaseCommand(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual void SetCommand(CommandLineApplication cmd)
        {
            cmd.HelpOption("-?|-h|--help");
        }
    }
}
