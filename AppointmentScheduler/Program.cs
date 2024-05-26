using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AppointmentScheduler.Data;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;

namespace AppointmentScheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppointmentContext>();
                context.Database.Migrate();
            }

            var app = host.Services.GetRequiredService<Application>();
            app.Run(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppointmentContext>(options =>
                        options.UseSqlServer("Data Source=.\\SQLEXPRESS01;Initial Catalog=AppointmentSchedulerDB;Integrated Security=True;TrustServerCertificate=True;"));
                    services.AddTransient<Application>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); // Clear default providers
                    logging.AddConsole(); // Add console logging
                    logging.SetMinimumLevel(LogLevel.Warning); // Set log level to warning or higher
                });
    }
}
