using System;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ReservationService : IReservationService
{
    private ReservationHandler _reservationHandler;

    public ReservationService(ReservationHandler reservationHandler)
    {
        _reservationHandler = reservationHandler;
    }

    public void AddReservation(Reservation reservation, string reserverName)
    {
        _reservationHandler.AddReservation(reservation, reserverName);
    }
    public void DeleteReservation(Reservation reservation)
    {
        _reservationHandler.DeleteReservation(reservation);
    }
    public void DisplayWeeklySchedule()
   {
    var allReservations = _reservationHandler.GetAllReservations();
    var weekStart = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday);
    var weekEnd = weekStart.AddDays(7);

    Console.WriteLine("This week's reservations:");
    foreach (var reservation in allReservations)
    {
        if (reservation.date >= weekStart && reservation.date < weekEnd)
        {
            Console.WriteLine($"{reservation.date.ToShortDateString()} at {reservation.time.ToShortTimeString()}: Reservation for {reservation.room.RoomName} by {reservation.reserverName}");
        }
    }
  }

}
public static class DateTimeExtensions
{
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
}

