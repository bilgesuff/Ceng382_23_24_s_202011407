using System;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

public class RoomData{
    [JsonPropertyName("Room")]
    public  Room[] Rooms {get; set;}
}
public record Room(
    [property: JsonPropertyName("roomId")] string romId,
    [property: JsonPropertyName("roomName")] string RoomName,
    [property: JsonPropertyName("capacity")] int Capacity);

public record LogRecord(DateTime TimeStamp, string ReserverName, string roomName);
public class ReservationHandler{ // This class should have used an interface instead of using an object of another class. Using an object from the Reservation class is against DI.
// This class is against the Single Responsibility Principle because it does the job of both adding and removing reservations and printing weekly reservations. According to this principle, each class has a single task.
//The use of SRP and DI is important because it increases the understandability and ease of implementation of the code (reduces its complexity).
private  Reservation[,] reservations;
public ReservationHandler(){
    reservations = new Reservation[7,8];
}
public  bool  addReservation(Reservation r){
        int dayIndex = (int)r.date.DayOfWeek - 1; // monday is 1
        if (dayIndex == -1) 
        {
            dayIndex = 6;
        }

        int timeSlot = r.time.Hour - 9;

        if (dayIndex < 0 || dayIndex > 6 || timeSlot < 0 || timeSlot > 7)
        {
            Console.WriteLine("Invalid reservation time.");
            return false;
        }

        if (reservations[dayIndex, timeSlot] == null)
        {
            reservations[dayIndex, timeSlot] = r;
            Console.WriteLine("Reservation added successfully.");
            return true;
        }
        else
        {
            Console.WriteLine(" Slot is already reserved.");
            return false;
        }
}
public bool deleteReservation(Reservation r){
        int dayIndex = (int)r.date.DayOfWeek - 1; 
        if (dayIndex == -1) 
        {
            dayIndex = 6;
        }

        int timeSlot = r.time.Hour - 9;

        if (dayIndex < 0 || dayIndex > 6 || timeSlot < 0 || timeSlot > 7)
        {
            Console.WriteLine("Invalid reservation time.");
            return false;
        }

        if (reservations[dayIndex, timeSlot] != null)
        {
            reservations[dayIndex, timeSlot] = null;
            Console.WriteLine("Reservation deleted successfully.");
            return true;
        }
        else
        {
            Console.WriteLine("No reservation found.");
            return false;
        }
}
public void WeeklySchedule(){
        DateTime startWeek = new DateTime(2024,3,25);
        DateTime endOfWeek = new DateTime(2024,3,31);
        Console.WriteLine("Reservation List between 25 - 31 March 2024: ");
        for(int i=0; i<reservations.GetLength(0); i++){ 
            string Nameofday;
            if(i==6){
                Nameofday = DayOfWeek.Sunday.ToString();
            }
            else{
                Nameofday= ((DayOfWeek)(i+1)).ToString();
            }
            for(int j=0; j<reservations.GetLength(1); j++){             
                if(reservations[i,j] != null){
                   DateTime reservationDate = reservations[i,j].date;                  
                   if(reservationDate >= startWeek && reservationDate <= endOfWeek){
                         Console.WriteLine($"Day {Nameofday}, Hour {j+9}: 00 : Reserved by {reservations[i,j].reserverName} in room {reservations[i,j].room.roomName}");
                   }
                  
                }
            
                else{
                    Console.WriteLine($"Day {Nameofday}, Hour {j+9}: 00 : There is no reservation.");
                }
            }
        }
    }
}


public record Reservation(DateTime time, DateTime date, string reserverName, Room room);

class Program {

    static Random random = new Random();

 
    
    static void Main(string [] args)
    {
        ReservationHandler handler = new ReservationHandler();
        DateTime inputDate, inputTime;
        string name;
        int rand;
        
        try{
        string jsonFilePath = "Data.json";
        string jsonString = File.ReadAllText(jsonFilePath);
        // options to read
        var options = new JsonSerializerOptions(){
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
        };
        // read try catch
        var roomData = JsonSerializer.Deserialize<RoomData>(jsonString,options);
        // print
        /* if(roomData?.Rooms != null)
        {
            foreach(var room in roomData.Rooms){
                Console.WriteLine($"Room ID: {room.roomId}, Name: {room.roomName}, Capacity {room.capacity}");
            }
        } */
         rand = random.Next(roomData.Rooms.Length);

        Room room = roomData.Rooms[rand];
        int rand2 = random.Next(roomData.Rooms.Length);
        Room room2 = roomData.Rooms[rand2];

        Reservation reservation1 = new Reservation{
            time = new DateTime(2024,3,25,9,0,0),
            date = new DateTime(2024,3,25),
            reserverName = "Bilgesu Findik",
            room = room
             };
        Reservation reservation2 = new Reservation{
            time = new DateTime(2024,3,26,10,0,0),
            date = new DateTime(2024,3,26),
            reserverName = "Findik Bilgesu",
            room = room2
        }; 
     
        while(true){
            Console.WriteLine("Press '1' to Generate new reservation. ");
            Console.WriteLine("Press '2' to exit. ");
            int selection = int.Parse(Console.ReadLine());
            if(selection == 1){
            
                Console.WriteLine("Enter your name: ");
                name = Console.ReadLine();
                int year,month,day,hour;
                Console.WriteLine("Enter a Month: ");
                while(!int.TryParse(Console.ReadLine(), out month) || month < 1 || month > 12)
                {
                    Console.WriteLine("Invalid . Please enter a valid month (1-12):");
                }

                Console.WriteLine("Enter a Day: ");
                while(!int.TryParse(Console.ReadLine(), out day) || day < 1 || day > 31)
                {
                    Console.WriteLine("Invalid input. Please enter a valid day (1-31):");
                }

                Console.WriteLine("Enter a Year: ");
                while(!int.TryParse(Console.ReadLine(), out year) || year < 2024)
                {
                    Console.WriteLine("Invalid input. Please enter a valid year:");
                }               

                
                Console.WriteLine("Enter hour of  reservation: ");
                while(!int.TryParse(Console.ReadLine(), out hour) || hour < 9 || hour > 16)
                {
                    Console.WriteLine("Invalid. Please enter a valid hour (9-16):");
                }
            try{
                rand= random.Next(roomData.Rooms.Length);
                inputDate = new DateTime(year,month,day);
                inputTime = new DateTime(year,month,day,hour,0,0);
                Room room3 = roomData.Rooms[rand];
                Reservation r = new Reservation{
                    reserverName = name,
                    time = inputTime,
                    date = inputDate,
                    room = room3
                    };

                
                handler.addReservation(r);

                }
            catch(ArgumentOutOfRangeException) {
                Console.WriteLine("The date or time entered is not valid.");
                }
            }
            else if(selection == 2){
                break;
            }
             else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
      

        handler.addReservation(reservation1);
        handler.addReservation(reservation2);
        handler.WeeklySchedule();
        }



        catch (Exception ex)
        {
        Console.WriteLine($"An error occured: {ex.Message}");
        }

    }



    
}