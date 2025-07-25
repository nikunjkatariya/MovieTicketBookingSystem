using MovieBooking.Models;

namespace MovieBooking.Services;

public class DeadlockBookingService
{
    private readonly object _lock = new();
    public Movie Movie { get; }

    public DeadlockBookingService(Movie movie)
    {
        Movie = movie;
    }

    public void BookSeatCross(Movie otherMovie, DeadlockBookingService otherService, User user)
    {
        var locks = new[] { this, otherService }.OrderBy(s => s.Movie.Title).ToArray();

        lock (locks[0]._lock) //(_lock)
        {
            Console.WriteLine($"[User {user.Id}] locked Movie: {Movie.Title}");

            Thread.Sleep(50); // Force overlap to increase deadlock chance

            lock (locks[1]._lock) //(otherService._lock)
            {
                Console.WriteLine($"[User {user.Id}] locked Other Movie: {otherMovie.Title}");

                if (Movie.AvailableSeats.Count == 0 || otherMovie.AvailableSeats.Count == 0)
                {
                    Console.WriteLine($"[User {user.Id}] No seats available.");
                    return;
                }

                var seat1 = Movie.AvailableSeats[0];
                Movie.AvailableSeats.RemoveAt(0);

                var seat2 = otherMovie.AvailableSeats[0];
                otherMovie.AvailableSeats.RemoveAt(0);

                Console.WriteLine($"[User {user.Id}] ✅ Booked {Movie.Title} seat {seat1}, {otherMovie.Title} seat {seat2}");
            }
        }
    }
}