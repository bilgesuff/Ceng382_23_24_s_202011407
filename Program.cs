using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

public class RoomData{
    [JsonPropertyName("Room")]
    public  Room[] Rooms {get; set;}
}
public class Room{
     [JsonPropertyName("roomId")]
     public string roomId{get; set;}

    [JsonPropertyName("roomName")]

    public string roomName {get; set;}

      [JsonPropertyName("capacity")]

    public int capacity {get; set;}




}
public class ReservationHandler{
private  Reservation[,] reservations;
public ReservationHandler(){
    reservations = new Reservation[7,8];
}

public void addReservation(Reservation r){
int dayOfWeek = ((int)r.date.DayOfWeek + 6) % 7; // 0 for Monday, 6 for Sunday
int slot = (r.time.Hour - 9 );
     if (slot >= 0 && slot < 8)
    {
        if (reservations[dayOfWeek, slot] == null)
        {
            reservations[dayOfWeek, slot] = r; 
            Console.WriteLine("Reservation added successfully.");
        }
        else
        {
            Console.WriteLine("The slot is already reserved. Please try again for another hour!");
        }
    }
    else
    {
        Console.WriteLine("Invalid time.");
    }
}
public void deleteReservation(Reservation r){
    int dayOfWeek = ((int)r.date.DayOfWeek + 6) % 7;
    int slot = r.time.Hour - 9;
    if(dayOfWeek >= 0 && dayOfWeek < 7 && slot >= 0 && slot < 8 ){
        if(reservations[dayOfWeek, slot]!= null && reservations[dayOfWeek, slot] == r){
            reservations[dayOfWeek, slot] = null;
            Console.WriteLine("Reservation deleted successfully.");
        }
        else{
            Console.WriteLine("No reservation found to delete.");
        }
    }
    else{
        Console.WriteLine("Invalid day or slot.");
    }

}
public void displayWeeklySchedule(){
        DateTime startOfWeek = new DateTime(2024,3,25);
        DateTime endOfWeek = new DateTime(2024,3,31);
        Console.WriteLine("Reservation List between the 25 March - 31 March 2024: ");
        for(int i=0; i<reservations.GetLength(0); i++){ // loop for days
            string dayName;
            if(i==6){
                dayName = DayOfWeek.Sunday.ToString();
            }
            else{
                 dayName = ((DayOfWeek)(i+1)).ToString();
            }
            for(int j=0; j<reservations.GetLength(1); j++){ // loop for hours
            
                if(reservations[i,j] != null){
                   DateTime reservationDate = reservations[i,j].date;
                  
                   if(reservationDate >= startOfWeek && reservationDate <= endOfWeek){
                         Console.WriteLine($"Day {dayName}, Hour {j+9}: 00 : Reserved by {reservations[i,j].reserverName} in room {reservations[i,j].room.roomName}");
                   }
                  
                }
            
                else{
                    Console.WriteLine($"Day {dayName}, Hour {j+9}: 00 : There is no reservation.");
                }
            }
        }
    }
}


public class Reservation{
public  DateTime time{get; set;}
public   DateTime date{get; set;}

public string reserverName{get; set;}

public Room room {get; set;}
}

class Program {

    static Random random = new Random();

 
    
    static void Main(string [] args)
    {
        ReservationHandler handler = new ReservationHandler();
        DateTime inputDate, inputTime;
        string name;
        int randomIndex3;
        // path to json
        // toDo inside try catch
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
        int randomIndex = random.Next(roomData.Rooms.Length);

        Room room = roomData.Rooms[randomIndex];
        int randomIndex2 = random.Next(roomData.Rooms.Length);
        Room room2 = roomData.Rooms[randomIndex2];

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
                    Console.WriteLine("Invalid input. Please enter a valid month (1-12):");
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
               

                
                Console.WriteLine("Enter hour of the reservation: ");
                while(!int.TryParse(Console.ReadLine(), out hour) || hour < 9 || hour > 16)
                {
                    Console.WriteLine("Invalid input. Please enter a valid hour (9-16):");
                }
            try{
                inputDate = new DateTime(year,month,day);
                inputTime = new DateTime(year,month,day,hour,0,0);
                randomIndex3 = random.Next(roomData.Rooms.Length);
                Room room3 = roomData.Rooms[randomIndex3];
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
        }
      

        handler.addReservation(reservation1);
        handler.addReservation(reservation2);
        handler.displayWeeklySchedule();
        }



        catch (Exception ex)
        {
        Console.WriteLine($"An error occured: {ex.Message}");
        }

    }



    
}