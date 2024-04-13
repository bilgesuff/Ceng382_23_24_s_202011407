using System;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class RoomHandler
{
     private string _filePath;

    public RoomHandler(string filePath)
    {
        _filePath = filePath;
    }

   public List<Room> GetRooms()
{
    try
    {
        string json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Room>>(json) ?? new List<Room>();
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"An error occurred while deserializing the room data: {ex.Message}");
        return new List<Room>(); // Return an empty list or handle accordingly
    }
}


    public void SaveRooms(List<Room> rooms)
    {
        var roomsJson = JsonSerializer.Serialize(rooms, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, roomsJson);
    }
}
