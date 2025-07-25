using MovieBooking.Models;
using MovieBooking.Services;
using System.Collections.Concurrent;

namespace MovieBooking.Orchestrator;

public class BookingOrchestrator
{
    private readonly Dictionary<string, BookingService> _bookingServices;
    private readonly SemaphoreSlim _globalThrottle;
    private readonly CancellationTokenSource _cts;

    public BookingOrchestrator(List<Movie> movies, int maxConcurrency = 100)
    {
        _bookingServices = movies.ToDictionary(
            m => m.Title,
            m => new BookingService(m));

        _globalThrottle = new SemaphoreSlim(maxConcurrency); // system-wide limit
        _cts = new CancellationTokenSource();
    }

    public async Task RunSimulatedBookingsAsync(int totalUsersPerMovie)
    {
        var allTasks = new List<Task>();

        foreach (var kvp in _bookingServices)
        {
            string movieTitle = kvp.Key;
            var service = kvp.Value;

            for (int i = 0; i < totalUsersPerMovie; i++)
            {
                var user = new User(i + 1, $"{movieTitle}_User{i + 1}");
                allTasks.Add(HandleBookingAsync(service, user));
            }
        }

        await Task.WhenAll(allTasks);
    }

    private async Task HandleBookingAsync(BookingService service, User user)
    {
        try
        {
            await _globalThrottle.WaitAsync(_cts.Token); // throttle system-wide

            await service.TryBookSeatAsync(user, _cts.Token); // now async-safe
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"[User {user.Id}] Booking canceled.");
        }
        finally
        {
            _globalThrottle.Release();
        }
    }

    public void Stop() => _cts.Cancel();

    public void PrintSummary()
    {
        Console.WriteLine("\nFinal Bookings:");
        foreach (var (movie, service) in _bookingServices)
        {
            int notBookedCount = service.GetAllNonBookings().Count;
            Console.WriteLine($"{movie}: {service.GetAllBookings().Count} bookings{(notBookedCount == 0 ? "" : $", ({notBookedCount} Not Booked)")}");
        }
    }
}
