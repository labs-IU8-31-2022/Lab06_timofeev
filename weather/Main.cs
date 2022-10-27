using WeatherBot;

Weather t1 = new();

//var t2 = Weather.GetAsync(45, 23).Result;
//var t3 = Weather.GetAsync(0, 0).Result;
//Console.ReadLine();

var weathers = new List<Weather>(50);

while (weathers.Count < 5)
{
    Random rand = new();
    var lat = (decimal)rand.Next(0, 18000000) / 100000 - 90;
    var lon = (decimal)rand.Next(0, 36000000) / 100000 - 180;
    try
    {
        var temp = Weather.GetAsync(lat, lon).Result;
        weathers.Add(temp);
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(
            $"{lat}<>{lon} \nCountry: {temp.Country} City: {temp.Name} " +
            $"\nWeather: {temp.Description} \nTemperature: {temp.Temp}°C");
        Console.ResetColor();
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine(e.Message);
        Thread.Sleep(9000);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        Thread.Sleep(1000);
    }
}
Console.ReadLine();