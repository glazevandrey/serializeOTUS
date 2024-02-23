// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

Console.WriteLine("Hello, World!");
int count = 10000;

var data = new F().Get();

//SERIALIZE
Console.WriteLine("SERIALIZE");
Console.WriteLine();
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

File.WriteAllText("test.csv", res);



Console.WriteLine();
//DESERIALIZE
Console.WriteLine("DESERIALIZE");
Console.WriteLine();
sw = Stopwatch.StartNew();
F f = new F();

var text = File.ReadAllText("test.csv");
for (int i = 0; i < count; i++)
{
    f = (F)CustomDeserialization(text, data.GetType());
}
sw.Stop();
 elapsedCUSTOM = sw.Elapsed;
Console.WriteLine("Custom des.: " + elapsedCUSTOM.Milliseconds + "ms.");

sw.Restart();
for (int i = 0; i < count; i++)
{
    f = JsonDeserializeDefault(text);
}
sw.Stop();
elapsedDEFAULT = sw.Elapsed;
Console.WriteLine("Default des.: " + elapsedDEFAULT.Milliseconds + "ms.");














string JsonSerializeDefault(F data)
{ 
    string json = "";
    json = JsonSerializer.Serialize(data);
    return json;
}
F JsonDeserializeDefault(string json)
{
    F data = new F();
    data = JsonSerializer.Deserialize<F>(json);
    return data;
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
object CustomDeserialization(string text, Type type)
{
    var data = Activator.CreateInstance(type)!;
    var json = JsonNode.Parse(text);

    if (json is null)
    {
        throw new Exception();
    }

    var props = data.GetType().GetProperties();
    
    foreach (var prop in props) 
    {
        var val = json[prop.Name].ToString();
        var correctType = Convert.ChangeType(val, prop.PropertyType);
        prop.SetValue(data, correctType);
    }

    return data;
}


class F { public int i1 { get; set; } public int i2 { get; set; } public int i3 { get; set; } public int i4 { get; set; } public int i5 { get; set; } public F Get() => new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 }; }
public static class StringExtensions
{
    public static string Capitalize(this string value)
    {
        var res = $"{char.ToUpper(value[0])}{value[1..]}";
        return res;
    }
}