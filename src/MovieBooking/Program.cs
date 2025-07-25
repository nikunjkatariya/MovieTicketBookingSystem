using MovieBooking.Models;
using MovieBooking.Services;
using MovieBooking.Orchestrator;
using System.Diagnostics;

//BookingService
var movies = new List<Movie>
{
    new Movie("Oppenheimer", 200),
    new Movie("Inception", 150),
    new Movie("Interstellar", 180),
};

var orchestrator = new BookingOrchestrator(movies, maxConcurrency: 100); // throttle to 100

Console.WriteLine("Starting Booking Simulation...");
var stopwatch = Stopwatch.StartNew();
var simulationTask = orchestrator.RunSimulatedBookingsAsync(totalUsersPerMovie: 210);

// Optional cancellation support (press ENTER to cancel)
_ = Task.Run(() =>
{
    Console.ReadLine();
    orchestrator.Stop();
});

await simulationTask;
stopwatch.Stop();

orchestrator.PrintSummary();
Console.WriteLine($"\nTotal Booking Time: {stopwatch.Elapsed.TotalMilliseconds} ms");

//DeadlockBookingService
// var movieA = new Movie("Movie A", 5);
// var movieB = new Movie("Movie B", 5);
//
// var serviceA = new DeadlockBookingService(movieA);
// var serviceB = new DeadlockBookingService(movieB);
//
// var user1 = new User(1, "User1");
// var user2 = new User(2, "User2");
//
// var t1 = Task.Run(() => serviceA.BookSeatCross(movieB, serviceB, user1));
// var t2 = Task.Run(() => serviceB.BookSeatCross(movieA, serviceA, user2));
//
// await Task.WhenAll(t1, t2);
