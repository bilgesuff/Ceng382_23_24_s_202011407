using Microsoft.EntityFrameworkCore;

public class Reservation{
   
    public int Id {get;set;} 
    public string? ReserverName {get;set;}

  

    public DateTime Time{get;set;}

    public DateOnly Date{get;set;}

    public int? RoomId {get;set;} // foreign key
    public Room? Room {get; set;}

}