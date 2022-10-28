using WeatherBot;

var weathers = new List<Weather>(50);
await using (var file = new StreamWriter("Result.txt", false))
{
    await file.WriteLineAsync("///Starting///");
}

while (weathers.Count < 5)
{
    Random rand = new();
    var lat = (decimal)rand.Next(0, 18000000) / 100000 - 90;
    var lon = (decimal)rand.Next(0, 36000000) / 100000 - 180;
    try
    {
        var temp = await Weather.GetAsync(lat, lon);
        //var temp = tempN.Result;
        weathers.Add(temp);
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        var tempString = $"{lat}<>{lon} \nCountry: {temp.Country} City: {temp.Name} " +
                         $"\nWeather: {temp.Description} \nTemperature: {temp.Temp}°C";
        Console.WriteLine(tempString);
        await using (var file = new StreamWriter("Result.txt", true))
        {
            await file.WriteLineAsync($"{tempString}\n");
        }
    }
    catch (HttpRequestException e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.Message);
        Thread.Sleep(9000);
    }
    catch (Exception e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.Message);
        //Console.WriteLine(e.Data);
        //Console.WriteLine(e.HelpLink);
        //Console.WriteLine(e.Source);
        //Console.WriteLine(e.StackTrace);
    }
    finally
    {
        Thread.Sleep(1000);
        Console.ResetColor();
    }
}

var maxTemp = weathers.MaxBy(i => i.Temp);
Console.WriteLine($"Country with max temperature: {maxTemp.Country} {maxTemp.Temp}");

var minTemp = weathers.MinBy(i => i.Temp);
Console.WriteLine($"Country with min temperature: {minTemp.Country} {minTemp.Temp}");

Console.WriteLine($"Mean temperature: {weathers.Average(i => i.Temp)}");

Console.WriteLine($"Number of unique countries: {weathers.Select(i => i.Country).Distinct().ToList().Count}");

var selectCountry = weathers.FirstOrDefault(i =>
    i.Description is "clear sky" or "rain" or "few clouds");
Console.WriteLine(
    $"First place with 'clear sky','rain' or 'few clouds': {selectCountry.Country} City: {selectCountry.Name}");


await using (var file = new StreamWriter("Result.txt", true))
{
    await file.WriteLineAsync($"Country with max temperature: {maxTemp.Country} {maxTemp.Temp}");
    await file.WriteLineAsync($"Country with min temperature: {minTemp.Country} {minTemp.Temp}");
    await file.WriteLineAsync($"Mean temperature: {weathers.Average(i => i.Temp)}");
    await file.WriteLineAsync(
        $"Number of unique countries: {weathers.Select(i => i.Country).Distinct().ToList().Count}");
    await file.WriteLineAsync(
        $"First place with 'clear sky','rain' or 'few clouds': {selectCountry.Country} City: {selectCountry.Name}");
}
