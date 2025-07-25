using MovieBooking.Models;
using System.Collections.Concurrent;

namespace MovieBooking.Services;

public class BookingService
{
    private readonly Movie _movie;
    private readonly object _movieLock = new();
    private readonly ConcurrentBag<Booking> _bookings = new();
    private readonly ConcurrentBag<Booking> _nonBookings = new();

    public BookingService(Movie movie)
    {
        _movie = movie;
    }

    public async Task<bool> TryBookSeatAsync(User user, CancellationToken cancellationToken)
    {
        await Task.Delay(Random.Shared.Next(100, 500), cancellationToken); // simulate external latency

        lock (_movieLock)
        {
            if (_movie.AvailableSeats.Count == 0)
            {
                Console.WriteLine($"[User {user.Id}] No seats left for {_movie.Title}");
                _nonBookings.Add(new Booking(user, 0));
                return false;
            }

            int seat = _movie.AvailableSeats[0];
            _movie.AvailableSeats.RemoveAt(0);
            _bookings.Add(new Booking(user, seat));

            Console.WriteLine($"[User {user.Id}] Booked seat {seat} for {_movie.Title}");
            return true;
        }
    }

    public List<Booking> GetAllBookings() => _bookings.ToList();
    public List<Booking> GetAllNonBookings() => _nonBookings.ToList();
}