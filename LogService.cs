using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class LogService{

    public record LogEntry(string Timestamp, string reserverName, string roomName);

    public record Reservation(DateTime Date, string ReserverName, string RoomName);

    public static List<Reservation> logs = new List<Reservation>();
 public static void FormatJsonFile(string jsonFilePath)
{
    var lines = File.ReadAllLines(jsonFilePath);
    using (var outputFile = new StreamWriter(jsonFilePath + ".formatted.json"))
    {
        outputFile.WriteLine("[");  
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            line = line.Replace(",,", ",");
            if (line.StartsWith("{,"))
            {
                line = "{" + line.Substring(2);
            }
            if (line.EndsWith(","))
            {
                line = line.Substring(0, line.Length - 1);
            }
            outputFile.Write(line);
            if (i < lines.Length - 1)
            {
                outputFile.WriteLine(",");
            }
            else
            {
                outputFile.WriteLine();
            }
        }

        outputFile.WriteLine("]");  
    }
}


 public static void InitializeLogs(string jsonFilePath)
    {
        string jsonString = File.ReadAllText(jsonFilePath);
        logs = JsonSerializer.Deserialize<List<LogEntry>>(jsonString) .Select(log => new Reservation(
                DateTime.Parse(log.Timestamp), 
                log.reserverName,
                log.roomName
            )).ToList();
    }




    public static List<Reservation> DisplayLogsByName(string name){
        return logs.Where(r => r.ReserverName.Equals(name,StringComparison.OrdinalIgnoreCase)).ToList();
    }
    public static List<Reservation> DisplayLogs(DateTime start, DateTime end){
        return logs.Where(r=> r.Date >= start && r.Date <= end).ToList();
    }
}