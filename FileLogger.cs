using System;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class FileLogger : ILogger
{
    private string file_path;

    public FileLogger(string filePath)
    {
        file_path = filePath;
    }

    public void Log(LogRecord log)
    {       
        var json = JsonSerializer.Serialize(log, new JsonSerializerOptions { WriteIndented = true });
        File.AppendAllText(file_path, json + Environment.NewLine);
    }
   
}


