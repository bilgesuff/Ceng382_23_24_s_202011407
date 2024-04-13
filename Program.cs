using System;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

public class RoomData{
    [JsonPropertyName("Room")]
    public Room[] Rooms { get; set; } = new Room[0]; 
}

public record Room(
    [property: JsonPropertyName("roomId")] string romId,
    [property: JsonPropertyName("roomName")] string RoomName,
    [property: JsonPropertyName("capacity")] int Capacity);

public record LogRecord(DateTime TimeStamp, string ReserverName, string roomName);
public record Reservation(DateTime time, DateTime date, string reserverName, Room room);
  

class Program
{
    
    public static Random random = new Random(); 
   
    static void Main(string[] args)
    {
        
        string logFilePath = "LogData.json";
        string reservationDataFilePath = "ReservationData.json";
        string roomsDataFilePath = "Data.json";
        string json = File.ReadAllText("Data.json");
        RoomData roomData = JsonSerializer.Deserialize<RoomData>(json);
        int randomIndex;
        CreateIfNotExists(logFilePath);
        CreateIfNotExists(reservationDataFilePath);
        CreateIfNotExists(roomsDataFilePath);
        
        ILogger logger = new FileLogger(logFilePath);
        IReservationRepository reservationRepo = new ReservationRepository(logger, reservationDataFilePath);
        RoomHandler roomHandler = new RoomHandler(roomsDataFilePath);
        LogHandler logHandler = new LogHandler(logger);
        ReservationHandler reservationHandler = new ReservationHandler(reservationRepo, logHandler, roomHandler);
        
        ReservationService reservationService = new ReservationService(reservationHandler);
        
        // Display available rooms
        var rooms = roomHandler.GetRooms();
       
        
        
        
           randomIndex =  random.Next(roomData.Rooms.Length);
           Room room = roomData.Rooms[randomIndex];


        
            Console.WriteLine("Enter Reserver Name:");
            string reserverName = Console.ReadLine();
            Console.WriteLine("Enter Reservation Time (hours from now):");
            int hoursFromNow = int.Parse(Console.ReadLine());

            Reservation newReservation = new Reservation(DateTime.Now.AddHours(hoursFromNow), DateTime.Now.Date, reserverName,room);
            reservationService.AddReservation(newReservation, reserverName);

            Console.WriteLine("Reservation added successfully!");
        
        
        
            
        
        
        reservationService.DisplayWeeklySchedule();        
        Console.WriteLine("Program has completed its execution. Press any key to exit.");
        Console.ReadKey();
    }

    private static void CreateIfNotExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            File.WriteAllText(filePath, "[]"); 
        }
    }
}


