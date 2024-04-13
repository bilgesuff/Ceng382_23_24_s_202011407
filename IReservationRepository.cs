using System;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

public interface IReservationRepository
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(Reservation reservation);
    List<Reservation> GetAllReservations();
}
