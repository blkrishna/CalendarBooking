using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AppointmentScheduler.Data;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler
{
    public class Application
    {
        private readonly AppointmentContext _context;

        public Application(AppointmentContext context)
        {
            _context = context;
        }

        public void Run(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid arguments.");
                return;
            }

            var command = args[0].ToUpper();
            switch (command)
            {
                case "ADD":
                    if (args.Length == 3 && DateTime.TryParseExact(args[1], "dd/MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime addDate) && TimeSpan.TryParse(args[2], out TimeSpan addTime))
                    {
                        AddAppointment(addDate, addTime).Wait();
                    }
                    else
                    {
                        Console.WriteLine("Invalid ADD command format.");
                    }
                    break;
                case "DELETE":
                    if (args.Length == 3 && DateTime.TryParseExact(args[1], "dd/MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime delDate) && TimeSpan.TryParse(args[2], out TimeSpan delTime))
                    {
                        DeleteAppointment(delDate, delTime).Wait();
                    }
                    else
                    {
                        Console.WriteLine("Invalid DELETE command format.");
                    }
                    break;
                case "FIND":
                    if (args.Length == 2 && DateTime.TryParseExact(args[1], "dd/MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime findDate))
                    {
                        FindFreeSlot(findDate).Wait();
                    }
                    else
                    {
                        Console.WriteLine("Invalid FIND command format.");
                    }
                    break;
                case "KEEP":
                    if (args.Length == 2 && TimeSpan.TryParse(args[1], out TimeSpan keepTime))
                    {
                        KeepSlot(keepTime).Wait();
                    }
                    else
                    {
                        Console.WriteLine("Invalid KEEP command format.");
                    }
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }

        public async Task AddAppointment(DateTime date, TimeSpan time)
        {
            if (IsSlotReserved(date, time))
            {
                Console.WriteLine("The time slot is reserved and cannot be used.");
                return;
            }

            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Date == date && a.Time == time);

            if (existingAppointment != null)
            {
                Console.WriteLine("Appointment already exists at this date and time.");
                return;
            }

            var appointment = new Appointment { Date = date, Time = time };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            Console.WriteLine("Appointment added successfully.");
        }

        public async Task DeleteAppointment(DateTime date, TimeSpan time)
        {
            var appointment = await _context.Appointments.FindAsync(date, time);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                Console.WriteLine("Appointment deleted successfully.");
            }
            else
            {
                Console.WriteLine("Appointment not found.");
            }
        }

        public async Task FindFreeSlot(DateTime date)
        {
            var timeslots = Enumerable.Range(0, 16).Select(i => new TimeSpan(9, 0, 0) + TimeSpan.FromMinutes(30 * i));
            foreach (var timeslot in timeslots)
            {
                if (!await _context.Appointments.AnyAsync(a => a.Date == date && a.Time == timeslot) && !IsSlotReserved(date, timeslot))
                {
                    Console.WriteLine($"Free timeslot: {timeslot}");
                    return;
                }
            }
            Console.WriteLine("No free timeslot found.");
        }

        public async Task KeepSlot(TimeSpan time)
        {
            var dates = Enumerable.Range(1, 28).Select(day => new DateTime(DateTime.Today.Year, DateTime.Today.Month, day));
            foreach (var date in dates)
            {
                if (!await _context.Appointments.AnyAsync(a => a.Date == date && a.Time == time) && !IsSlotReserved(date, time))
                {
                    var appointment = new Appointment { Date = date, Time = time };
                    _context.Appointments.Add(appointment);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Kept timeslot: {date.ToShortDateString()} {time}");
                    return;
                }
            }
            Console.WriteLine("No available slot to keep.");
        }

        private bool IsSlotReserved(DateTime date, TimeSpan time)
        {
            return time == new TimeSpan(16, 0, 0) && date.DayOfWeek == DayOfWeek.Tuesday && (date.Day - 1) / 7 == 2;
        }
    }
}
