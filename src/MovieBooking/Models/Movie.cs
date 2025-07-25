namespace MovieBooking.Models;

public class Movie
{
    public string Title { get; set; }
    public int TotalSeats { get; set; }
    public List<int> AvailableSeats { get; set; }

    public Movie(string title, int totalSeats)
    {
        Title = title;
        TotalSeats = totalSeats;
        AvailableSeats = Enumerable.Range(1, totalSeats).ToList();
    }
}
