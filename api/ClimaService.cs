using Newtonsoft.Json.Linq;
using System.Net.Http;

public static class ClimaService
{
    public static async Task<string> ObtenerClima(string ciudad)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("DiscordBot");

        try
        {
            
            string geoUrl =
                $"https://nominatim.openstreetmap.org/search?q={ciudad}&format=json&limit=1";

            var geoResponse = await client.GetStringAsync(geoUrl);
            var geoJson = JArray.Parse(geoResponse);

            if (geoJson.Count == 0)
                return " Ciudad no encontrada.";

            string lat = geoJson[0]["lat"]!.ToString();
            string lon = geoJson[0]["lon"]!.ToString();

            // 2️ Obtener clima
            string climaUrl =
                $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";

            var climaResponse = await client.GetStringAsync(climaUrl);
            var climaJson = JObject.Parse(climaResponse);

            var clima = climaJson["current_weather"]!;

            string temperatura = clima["temperature"]!.ToString();
            string viento = clima["windspeed"]!.ToString();

            return
                $"🌤 **Clima en {ciudad}**\n" +
                $"🌡 Temperatura: **{temperatura}°C**\n" +
                $"💨 Viento: **{viento} km/h**";
        }
        catch
        {
            return " Error obteniendo el clima.";
        }
    }
}
