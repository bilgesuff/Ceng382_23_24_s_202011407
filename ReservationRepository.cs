using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ReservationRepository : IReservationRepository
{
    private List<Reservation> _reservations = new List<Reservation>();
    private ILogger _logger;
    private string _dataFilePath = "ReservationData.json"; 
    public ReservationRepository(ILogger logger, string dataFilePath)
    {
        _dataFilePath = dataFilePath;
        _logger = logger;
        LoadReservations(); 
    }

   private void LoadReservations()
  {
    if (File.Exists(_dataFilePath))
    {
        string json = File.ReadAllText(_dataFilePath);
        if (!string.IsNullOrWhiteSpace(json))
        {
            try
            {
                _reservations = JsonSerializer.Deserialize<List<Reservation>>(json) ?? new List<Reservation>();
            }
            catch (JsonException ex)
            {
                //_logger.Log($"Error deserializing JSON: {ex.Message}");
                _reservations = new List<Reservation>();
            }
        }
        else
        {
            _reservations = new List<Reservation>();
        }
    }
  }


    private void SaveReservations()
    {
        string json = JsonSerializer.Serialize(_reservations, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_dataFilePath, json);
    }
    public void AddReservation(Reservation reservation)
    {
        _reservations.Add(reservation);
        SaveReservations(); 
        _logger.Log(new LogRecord(DateTime.Now, reservation.reserverName, reservation.room.RoomName));
    }

    public void DeleteReservation(Reservation reservation)
    {
        _reservations.Remove(reservation);
        SaveReservations(); 
        _logger.Log(new LogRecord(DateTime.Now, reservation.reserverName, reservation.room.RoomName));
    }
    public List<Reservation> GetAllReservations()
    {
        return _reservations;
    }
}
