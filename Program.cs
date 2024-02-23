// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

Console.WriteLine("Hello, World!");
int count = 10000;

var data = new F().Get();

Stopwatch sw = Stopwatch.StartNew();
string res = "";
for (int i = 0; i < count; i++)
{
   res = CustomSerialization(data);
}
sw.Stop();
var elapsedCUSTOM = sw.Elapsed;
Console.WriteLine("Custom json: " + res);
Console.WriteLine("Custom ser.: "+ elapsedCUSTOM.Milliseconds + "ms.");

sw.Restart();
for (int i = 0; i < count; i++)
{
    res = JsonSerializeDefault(data);
}
sw.Stop();
var elapsedDEFAULT = sw.Elapsed;
Console.WriteLine("Delault json: " + res);
Console.WriteLine("Default ser.: " + elapsedDEFAULT.Milliseconds + "ms.");
string JsonSerializeDefault(F data)
{ 
    string json = "";
    json = JsonSerializer.Serialize(data);
    return json;
}
string CustomSerialization(F data)
{
    string json = "{";

    var fields = data.GetType().GetProperties();
    foreach (var prop in fields)
    {
        json += $"\"{prop.Name}\":{prop.GetValue(data)},";
    }

    json = json.TrimEnd(',');
    json += "}";
    return json;
}


public class F { public int i1 { get; set; }  public int  i2 { get; set; } public int  i3 { get; set; }  public int i4 { get; set; } public int  i5 { get; set; } public F Get() => new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 }; }