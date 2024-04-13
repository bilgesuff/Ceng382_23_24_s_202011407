using System;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

public interface ILogger {
    void Log(LogRecord log);
    //void Log(string v);
}