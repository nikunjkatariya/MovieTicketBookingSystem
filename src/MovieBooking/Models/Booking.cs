namespace MovieBooking.Models;

public class Booking
{
    public User User { get; }
    public int SeatNumber { get; }

    public Booking(User user, int seatNumber)
    {
        User = user;
        SeatNumber = seatNumber;
    }
}