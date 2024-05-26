using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Data
{
    public class AppointmentContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }

        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().HasKey(a => new { a.Date, a.Time });
        }
    }

    public class Appointment
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }
}
