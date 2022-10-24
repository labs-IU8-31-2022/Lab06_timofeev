using WeatherBot;

Weather t1 = new();

var t2 = Weather.GetAsync(45, 23).Result;
var t3 = Weather.GetAsync(0, 0).Result;
Console.ReadLine();
