# CalendarBooking
Technical Test Calendar Booking C#

# Appointment Scheduler

## Overview

The Appointment Scheduler is a simple console application built using C# and .NET. It demonstrates various key concepts such as Database Development with Entity Framework Core, Dependency Injection, and Unit Testing. The application allows users to manage appointments through command-line inputs.

## Features

- **ADD DD/MM hh:mm**: Adds an appointment.
- **DELETE DD/MM hh:mm**: Removes an appointment.
- **FIND DD/MM**: Finds a free timeslot for the specified day.
- **KEEP hh:mm**: Keeps a timeslot for any day.

## Constraints

- The acceptable time is between 9AM and 5PM.
- The time slot from 4 PM to 5 PM on each second Tuesday of the third week of any month is reserved and unavailable.

## Technologies Used

- **.NET 6**
- **Entity Framework Core**
- **SQL Server Express LocalDB**
- **Dependency Injection**
- **xUnit and Moq for Unit Testing**

## Setup

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- SQL Server Express LocalDB

### Running the Application

1. Clone the repository:

    ```bash
    git clone <repository-url>
    cd AppointmentScheduler
    ```

2. Install the required packages:

    ```bash
    dotnet restore
    ```

3. Apply database migrations:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

4. Build and run the application:

    ```bash
    dotnet build
    dotnet run -- ADD 24/05 10:00
    ```

### Command Examples

- **Add an appointment**:
    ```bash
    dotnet run -- ADD 24/05 10:00
    ```

- **Delete an appointment**:
    ```bash
    dotnet run -- DELETE 24/05 10:00
    ```

- **Find a free timeslot**:
    ```bash
    dotnet run -- FIND 24/05
    ```

- **Keep a timeslot**:
    ```bash
    dotnet run -- KEEP 10:00
    ```

## Project Structure

- **Program.cs**: Sets up the DI container and configures services.
- **Application.cs**: Contains the main logic for handling commands.
- **Data/AppointmentContext.cs**: Defines the database context and the `Appointment` entity.

## Dependency Injection

The `Program.cs` file sets up Dependency Injection using `IHostBuilder` and `ConfigureServices`:

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
            services.AddDbContext<AppointmentContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AppointmentSchedulerDB;Trusted_Connection=True;"))
            .AddTransient<Application>());

