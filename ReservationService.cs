using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public  class ReservationService : IReservationService
{
    
    private static ReservationHandler _reservationHandler;
    public static List<Reservation> _reservations = new List<Reservation>();

   public ReservationService(ReservationHandler reservationHandler){
        _reservationHandler = reservationHandler;
    }

    public static  List<Reservation> InitializeReservations(string jsonFilePath)
    {
        try
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            return JsonSerializer.Deserialize<List<Reservation>>(
                jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<Reservation>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<Reservation>();
        }
    }

    public  static void PrintReservations()
    {
        foreach (var reservation in _reservations)
        {
            Console.WriteLine($"DateTime: {reservation.date}, Reserver: {reservation.reserverName}, Room: {reservation.room}");
        }
    }
      public static void PrintReservations(List<Reservation> reservations){
        foreach (var reservation in reservations){
            Console.WriteLine($"DataTime : {reservation.date}, Reserver : {reservation.reserverName}, Room : {reservation.room} , Capacity : {reservation.room.Capacity}");
        }
      }

     public static void DisplayReservationByReserver(string name)
    {
        var filteredReservations = _reservations.Where(r => r.reserverName.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!filteredReservations.Any())
        {
            Console.WriteLine($"\nNo reservations found for: {name}");
        }
        else
        {
            Console.WriteLine($"\nReservations for {name}:");
            PrintReservations(filteredReservations);
        }
    }
      public static void DisplayReservationByRoomId(string Id){
    var roomName = RoomHandler.GetRoomByNameId(Id);
    var filteredReservations = filterByRoomId(Id);
    if(filteredReservations.Count == 0){
         Console.WriteLine($"\nNo reservations found for: {Id}");
    }
    else{
        if (roomName != null && roomName.Count > 0){
            foreach (var room in roomName){
                Console.WriteLine($"\n Reservations for {room}:");
                PrintReservations(filteredReservations);
            }
    }
        else{   
            Console.WriteLine("\nRoomID does not exist.");
            }
    }

   
}
    private static List<Reservation> filterByRoomId (string Id){
        var filteredReservations = _reservations.Where(r => r.room.romId.Equals(Id,StringComparison.OrdinalIgnoreCase)).ToList();
        return filteredReservations;
    }


    public  void AddReservation(Reservation reservation, string reserverName)
    {
        _reservationHandler.AddReservation(reservation);
    }

    public  void DeleteReservation(Reservation reservation)
    {
        _reservationHandler.DeleteReservation(reservation);
    }

      public   void DisplayWeeklySchedule()
    {
        
        var weekStart = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
        var weekEnd = weekStart.AddDays(7);

        
        var thisWeekReservations = _reservationHandler.GetAllReservations()
            .Where(r => r.date >= weekStart && r.date < weekEnd)
            .ToList();

        
        foreach (var reservation in thisWeekReservations)
        {
            Console.WriteLine($"{reservation.date.ToShortDateString()} - {reservation.time.ToShortTimeString()}, {reservation.room.RoomName}, Reserved by: {reservation.reserverName}");
        }
    }
}

public static class DateTimeExtensions
{
    
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek firstDayOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - firstDayOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
}




