using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AppointmentScheduler.Data;
using static System.Net.Mime.MediaTypeNames;

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
                .ConfigureServices((_, services) =>
                    services.AddDbContext<AppointmentContext>(options =>
                        options.UseSqlServer("Data Source=.\\SQLEXPRESS01;Initial Catalog=AppointmentSchedulerDB;Integrated Security=True;TrustServerCertificate=True;"))
                    .AddTransient<Application>());
    }
}
