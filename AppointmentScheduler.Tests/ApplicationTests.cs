using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using AppointmentScheduler.Data;

namespace AppointmentScheduler.Tests
{
    public class ApplicationTests
    {
        private Application CreateApplication(AppointmentContext context)
        {
            return new Application(context);
        }

        private AppointmentContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppointmentContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new AppointmentContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task AddAppointment_AddsAppointmentToDatabase()
        {
            var context = CreateInMemoryContext();
            var app = CreateApplication(context);

            var date = new DateTime(2024, 5, 24);
            var time = new TimeSpan(10, 0, 0);

            await app.AddAppointment(date, time);

            var appointment = await context.Appointments.FindAsync(date, time);
            Assert.NotNull(appointment);
        }
                
        [Fact]
        public async Task DeleteAppointment_RemovesAppointmentFromDatabase()
        {
            var context = CreateInMemoryContext();
            var app = CreateApplication(context);

            var date = new DateTime(2024, 5, 24);
            var time = new TimeSpan(10, 0, 0);
            context.Appointments.Add(new Appointment { Date = date, Time = time });
            await context.SaveChangesAsync();

            await app.DeleteAppointment(date, time);

            var appointment = await context.Appointments.FindAsync(date, time);
            Assert.Null(appointment);
        }

        [Fact]
        public async Task FindFreeSlot_FindsFirstAvailableSlot()
        {
            var context = CreateInMemoryContext();
            var app = CreateApplication(context);

            var date = new DateTime(2024, 5, 24);
            var time = new TimeSpan(9, 0, 0);
            context.Appointments.Add(new Appointment { Date = date, Time = time });
            await context.SaveChangesAsync();

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                await app.FindFreeSlot(date);

                var result = sw.ToString().Trim();
                Assert.Equal($"Free timeslot: {new TimeSpan(9, 30, 0)}", result);
            }
        }

        [Fact]
        public async Task KeepSlot_KeepsFirstAvailableSlot()
        {
            var context = CreateInMemoryContext();
            var app = CreateApplication(context);

            var time = new TimeSpan(10, 0, 0);

            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                await app.KeepSlot(time);

                var keptSlot = await context.Appointments.FirstOrDefaultAsync(a => a.Time == time);
                Assert.NotNull(keptSlot);
                var result = sw.ToString().Trim();
                Assert.Contains("Kept timeslot:", result);
            }
        }
    }
}
