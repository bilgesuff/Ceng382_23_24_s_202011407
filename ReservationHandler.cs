using System;
using System.Collections.Generic;

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
    public void AddReservation(Reservation reservation, string reserverName)
    {        
        _reservationRepository.AddReservation(reservation);        
        _logHandler.AddLog(new LogRecord(DateTime.Now, reserverName, reservation.room.RoomName));
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
    public List<Room> GetRooms()
    {        
        return _roomHandler.GetRooms();
    }
    public void SaveRooms(List<Room> rooms)
    {        
        _roomHandler.SaveRooms(rooms);
    }
}
