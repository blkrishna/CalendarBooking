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

- **.NET 8**
- **Entity Framework Core**
- **SQL Server Express LocalDB**
- **Dependency Injection**
- **xUnit and Moq for Unit Testing**

## Setup

### Prerequisites

- .NET SDK
- SQL Server Express LocalDB

- Update the database connection string in `Program.cs` if necessary. This configuration defined in the `CreateHostBuilder` method within `Program.cs`

### Running the Application

1. Clone the repository:

    ```bash
    git clone <https://github.com/blkrishna/CalendarBooking.git>
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
- - `AppointmentScheduler/`: Contains the main application code.
- `AppointmentScheduler.Tests/`: Contains the unit tests for the application.
- `AppointmentScheduler.Data/`: Contains the data context and model definitions.

## Dependency Injection

The project uses dependency injection to manage dependencies, specifically the database context and the main application class. 
This is set up in the `Program.cs` file using the `HostBuilder`.
The `Program.cs` file sets up Dependency Injection using `IHostBuilder` and `ConfigureServices`:

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
            services.AddDbContext<AppointmentContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AppointmentSchedulerDB;Trusted_Connection=True;"))
            .AddTransient<Application>());

## Unit Testing

Unit tests are written using the xUnit framework and can be found in the `AppointmentScheduler.Tests` project. 
The tests cover the main functionalities of adding, deleting, finding, and keeping appointments.

To run the tests:
```bash
dotnet test

 ## Areas of improvement

While the current implementation meets the basic requirements, below mentioned are few possible areas for potential improvement:

- Error Handling: Enhance error handling to cover more edge cases and provide more informative error messages.
- Validation: Add more thorough input validation to ensure that all commands and parameters are correctly formatted and within valid ranges.
- Configuration: Externalize the configuration settings, such as database connection strings, to a configuration file or environment variables.
- Logging: Improve logging by integrating a more robust logging framework and allowing for different log levels and outputs.
- Scalability: Refactor the application to handle larger data sets and concurrent operations more efficiently.
- User Interface: Develop a graphical user interface (GUI) or a web interface to improve user experience.
- Scheduling Conflicts: Implement more advanced conflict resolution strategies for scheduling appointments, 
                        such as notifying users of conflicts and suggesting alternative times.