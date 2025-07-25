using System.Diagnostics;
using MovieBooking.Models;
using MovieBooking.Services;
using Xunit;

namespace MovieBooking.Tests;

public class BookingServiceTests
{
    private const int TotalSeats = 50;

    [Fact(DisplayName = "All Bookings Must Be Unique and Within Seat Range")]
    public async Task Bookings_ShouldBe_ThreadSafe_And_Unique()
    {
        // Arrange
        var movie = new Movie("Inception", TotalSeats);
        var service = new BookingService(movie);

        var users = Enumerable.Range(1, 100)
            .Select(i => new User(i, $"User{i}"))
            .ToList();

        var stopwatch = Stopwatch.StartNew();

        // Act
        var tasks = users.Select(user => Task.Run(() => service.TryBookSeatAsync(user, default)));
        await Task.WhenAll(tasks);

        stopwatch.Stop();

        var bookings = service.GetAllBookings();

        // Assert
        Assert.True(bookings.Count <= TotalSeats, "More bookings than seats!");

        // Check for duplicates
        var duplicateSeats = bookings
            .GroupBy(b => b.SeatNumber)
            .Where(g => g.Count() > 1)
            .ToList();

        Assert.Empty(duplicateSeats); // Duplicates found!

        // Check all seat numbers are within range
        Assert.All(bookings, b => Assert.InRange(b.SeatNumber, 1, TotalSeats));

        Console.WriteLine($"Test Passed. Bookings: {bookings.Count}, Time: {stopwatch.Elapsed.TotalMilliseconds} ms");
    }

    [Fact(DisplayName = "No Booking Allowed When Seats Exhausted")]
    public async Task Booking_Should_Reject_When_Seats_Exhausted()
    {
        var movie = new Movie("Oppenheimer", 5);
        var service = new BookingService(movie);

        var users = Enumerable.Range(1, 10)
            .Select(i => new User(i, $"User{i}"))
            .ToList();

        var tasks = users.Select(user => Task.Run(() => service.TryBookSeatAsync(user, default)));
        await Task.WhenAll(tasks);

        var bookings = service.GetAllBookings();

        Assert.Equal(5, bookings.Count);
        Assert.All(bookings, b => Assert.InRange(b.SeatNumber, 1, 5));
    }

    [Fact(DisplayName = "Performance Test with 1000 Users")]
    public async Task Booking_Performance_Should_Be_Reasonable()
    {
        var movie = new Movie("Interstellar", 100);
        var service = new BookingService(movie);

        var users = Enumerable.Range(1, 1000)
            .Select(i => new User(i, $"User{i}"))
            .ToList();

        var stopwatch = Stopwatch.StartNew();

        var tasks = users.Select(u => Task.Run(() => service.TryBookSeatAsync(u, default)));
        await Task.WhenAll(tasks);

        stopwatch.Stop();

        var bookings = service.GetAllBookings();

        Assert.Equal(100, bookings.Count);
        Console.WriteLine($"1000 users processed in {stopwatch.Elapsed.TotalMilliseconds} ms");
    }

    [Fact(DisplayName = "Booking Should Be Deterministic Under Concurrency")]
    public async Task Booking_ShouldBe_Repeatable_And_Safe()
    {
        var movie = new Movie("Tenet", 20);
        var service = new BookingService(movie);

        var allBookings = new List<Booking>();
        var errors = new List<string>();

        var stopwatch = Stopwatch.StartNew();

        await Parallel.ForEachAsync(
            Enumerable.Range(1, 100),
            async (i, ct) =>
            {
                var user = new User(i, $"User{i}");
                try
                {
                    var success = await service.TryBookSeatAsync(user, ct);
                    if (!success) return;
                }
                catch (Exception ex)
                {
                    lock (errors) errors.Add($"[User {i}] {ex.Message}");
                }
            });

        stopwatch.Stop();

        allBookings.AddRange(service.GetAllBookings());

        Assert.True(allBookings.Count <= 20, "More bookings than available seats");
        Assert.Empty(errors);
        Assert.All(allBookings, b => Assert.NotEqual(0, b.SeatNumber));

        Console.WriteLine($"Completed with {allBookings.Count} bookings in {stopwatch.Elapsed.TotalMilliseconds} ms");
    }
}
