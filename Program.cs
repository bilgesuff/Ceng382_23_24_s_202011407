using System;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

public class RoomData{
    [JsonPropertyName("Room")]
    public required Room[] Rooms { get; set; } 
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
     private static void CreateIfNotExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            File.WriteAllText(filePath, "[]"); 
        }
    }

     static void DeleteReservation(ReservationHandler reservationHandler)
{
    var reservations = reservationHandler.GetAllReservations();
    if (reservations.Count == 0)
    {
        Console.WriteLine("There are no reservations to delete.");
        return;
    }

    Console.WriteLine("Please select a reservation to delete:");
    for (int i = 0; i < reservations.Count; i++)
    {
        var reservation = reservations[i];
        Console.WriteLine($"{i + 1}. Reservation for {reservation.room.RoomName} by {reservation.reserverName} on {reservation.date.ToShortDateString()} at {reservation.time.ToShortTimeString()}");
    }

    Console.Write("Choice (enter number): ");
    if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= reservations.Count)
    {
        var selectedReservation = reservations[index - 1];
        reservationHandler.DeleteReservation(selectedReservation);
        Console.WriteLine("Reservation deleted successfully.");
    }
    else
    {
        Console.WriteLine("Invalid selection.");
    }
}
   
    static void Main(string[] args)
    {
        
        string logFilePath = "LogData.json";
        string reservationDataFilePath = "ReservationData.json";
        string roomsDataFilePath = "Data.json";
        string formattedJson = "LogDataFormatted.json";
        string json = File.ReadAllText("Data.json");
        RoomData? roomData = JsonSerializer.Deserialize<RoomData>(json);
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
        var rooms = RoomHandler.GetRooms();
        LogService.InitializeLogs(formattedJson);
    

        
        
        
        //Room room = roomData.Rooms[randomIndex];
        bool process = true;
    while(process){
        
        ReservationService.InitializeReservations(reservationDataFilePath);
        Console.WriteLine("\nPlease Select a transaction: ");
        Console.WriteLine("1. Create New Reservation");
        Console.WriteLine("2. Delete a Reservation");
        Console.WriteLine("3. View the Reservations of this week");
        Console.WriteLine("4. Search for Reservations by entering Reserver Name");
        Console.WriteLine("5. Search for Reservations by entering Room Id ");
        Console.WriteLine("6. Display All Logs by entering Reserver Name ");
        Console.WriteLine("7. Exit");
        Console.Write("Choice: ");
        string? choice = Console.ReadLine();
        switch(choice){
            case "1":
                randomIndex = random.Next(roomData.Rooms.Length);
                Room? room = roomData.Rooms[randomIndex];
                Console.WriteLine("Enter your Name: ");
                string? name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Name cannot be empty.");
                    return; 
                }
                Console.WriteLine("Your reservation successfully created.");
                Reservation r = new Reservation(DateTime.Now,DateTime.Now,name,room);
                reservationHandler.AddReservation(r);
                Thread.Sleep(1000);
                break;
            case "2":
                DeleteReservation(reservationHandler);
                Thread.Sleep(1000);
                break;
            case "3":
                reservationService.DisplayWeeklySchedule();
                Thread.Sleep(1000);
                break;
            case "4":
                Console.WriteLine("Enter Reserver Name: ");
                string? reserver_name = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(reserver_name)){
                    Console.WriteLine("Name cannot be empty!");
                    return;
                }
                ReservationService.DisplayReservationByReserver(reserver_name);
                Thread.Sleep(1000);
                break;
            case "5":
                Console.WriteLine("Enter the Room ID: ");
                string? room_id = Console.ReadLine();
                  if(string.IsNullOrWhiteSpace(room_id)){
                    Console.WriteLine("The Room ID cannot be empty!");
                    return;
                }
                ReservationService.DisplayReservationByRoomId(room_id);
                Thread.Sleep(1000);
                break;
            case "6":
                Console.WriteLine("Enter the Reserver Name: ");
                string? room_id2 = Console.ReadLine();
                var enteringLogs = LogService.DisplayLogsByName(room_id2);
                if (enteringLogs.Any())
                    {
                        foreach (var log in enteringLogs)
                        {
                            Console.WriteLine($"Date: {log.Date}, Reserver: {log.ReserverName}, Room: {log.RoomName}");
                        }
                    }
                else
                {
                            Console.WriteLine("No logs found.");
                }
                break;
            case "7":
                process = false;
                break;
            default:
                Console.WriteLine("Invalid Choice");
                break;
    }
    }

   
}

}



