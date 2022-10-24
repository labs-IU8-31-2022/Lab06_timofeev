using System.Data;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;


namespace WeatherBot;

struct Weather
{
    public string? Country { get; }
    public string? Name { get; }
    public decimal Temp { get; }
    public string Description { get; }

    public Weather(string country, string name, decimal temp, string description) =>
        (Country, Name, Temp, Description) = (country, name, temp, description);

    public static async Task<Weather> GetAsync(decimal lat, decimal lon)
    {
        if (lat < -90 && lat > 90 && lon < -180 && lon > 180)
        {
            throw new Exception("Check range of coordinates");
        }

        HttpClient client = new();
        client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/weather");
        var response =
            await client.GetAsync($"?lat={lat}&lon={lon}&appid={api}&units=metric");
        Console.WriteLine(response.EnsureSuccessStatusCode());
        var (weather, code) = FromJsonDeserializer(await response.Content.ReadAsStringAsync());
        
        if (code == 200 && weather.Country is not null && weather.Name is not null)
        {
            return weather;
        }

        if (code == 429)
        {
            throw new HttpRequestException("Too fast response");
        }

        if (weather.Country is null || weather.Name is null)
        {
            throw new DataException("Null country or city");
        }

        throw new Exception("Не знаю что произошло, смотри сам");
    }

    private static (Weather w, int code) FromJsonDeserializer(string json)
    {
        var dynamic = JsonConvert.DeserializeObject<dynamic>(json);

        var country = Convert.ToString(dynamic?.sys.country);
        var name = Convert.ToString(dynamic?.name);
        var temp = Convert.ToDecimal(dynamic?.main.temp);
        var description = Convert.ToString(dynamic?.weather[0].description);
        return (new Weather(country, name, temp, description), dynamic?.cod);
    }
}