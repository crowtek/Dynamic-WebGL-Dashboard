using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherService : IWeatherService
{
    private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";

    public async Awaitable<OpenMeteoResponse> GetWeatherAsync(float latitude, float longitude)
    {
        // Force dots instead of commas for decimals
        string latStr = latitude.ToString(CultureInfo.InvariantCulture);
        string lonStr = longitude.ToString(CultureInfo.InvariantCulture);

        string currentParams = "current=temperature_2m,relative_humidity_2m,apparent_temperature,surface_pressure,wind_speed_10m,wind_direction_10m,weather_code";
        string hourlyParams = "hourly=temperature_2m,precipitation_probability,visibility,uv_index,weather_code";
        string dailyParams = "daily=temperature_2m_max,temperature_2m_min,sunrise,sunset,weather_code";

        string url = $"{BaseUrl}?latitude={latStr}&longitude={lonStr}&{currentParams}&{hourlyParams}&{dailyParams}&timezone=auto";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[WeatherService] HTTP Error: {request.error} | Details: {request.downloadHandler?.text}");
                return null;
            }

            return JsonUtility.FromJson<OpenMeteoResponse>(request.downloadHandler.text);
        }
    }
}