using System;
using System.Collections.Generic;
using System.Text.Json;

public class ReservationHandler
{
    private IReservationRepository _reservationRepository;
    private LogHandler _logHandler;
    private RoomHandler _roomHandler; 
    public ReservationHandler(
        IReservationRepository reservationRepository, 
        LogHandler logHandler, 
        RoomHandler roomHandler) 
    {
        _reservationRepository = reservationRepository;
        _logHandler = logHandler;
        _roomHandler = roomHandler; 
    }
   public void AddReservation(Reservation reservation){

  _reservationRepository.AddReservation(reservation);
   var log = new LogRecord(DateTime.Now,reservation.reserverName, $"Added Reservation: {reservation.room.RoomName}");
  _logHandler.AddLog(log);
}
    public void DeleteReservation(Reservation reservation)
    {        
        _reservationRepository.DeleteReservation(reservation);
        _logHandler.AddLog(new LogRecord(DateTime.Now, reservation.reserverName, reservation.room.RoomName));
    }
    public List<Reservation> GetAllReservations()
    {
        return _reservationRepository.GetAllReservations();
    }
   public List<Room> GetRooms(){
      string jsonFilePath = "Data.json"; 
      string jsonString = File.ReadAllText(jsonFilePath);
      var roomData = JsonSerializer.Deserialize<RoomData>(jsonString);
      return roomData?.Rooms.ToList() ?? new List<Room>();
   }

    public void SaveRooms(List<Room> rooms)
    {        
        _roomHandler.SaveRooms(rooms);
    }
}
