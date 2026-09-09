using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherService : IWeatherService
{
    private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";
    private const string CacheKey = "CachedWeatherData";

    public async Awaitable<OpenMeteoResponse> GetWeatherAsync(float latitude, float longitude)
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("[WeatherService] No internet connection. Attempting to load cached weather data.");
            return LoadFallbackData();
        }
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
            string jsonResponse = request.downloadHandler.text;

            PlayerPrefs.SetString(CacheKey, jsonResponse);
            PlayerPrefs.Save();

            return JsonUtility.FromJson<OpenMeteoResponse>(jsonResponse);
        }
    }

    private OpenMeteoResponse LoadFallbackData()
    {
        if (PlayerPrefs.HasKey(CacheKey))
        {
            string cachedJson = PlayerPrefs.GetString(CacheKey);
            return JsonUtility.FromJson<OpenMeteoResponse>(cachedJson);
        }

        Debug.LogWarning("[WeatherService] No local cache found. Loading bundled default fallback data.");
        return JsonUtility.FromJson<OpenMeteoResponse>(GetDefaultFallbackJson());
    }

    private string GetDefaultFallbackJson()
    {
        return @"{
            ""latitude"": 51.3671,
            ""longitude"": 7.4633,
            ""current_units"": { ""wind_speed_10m"": ""km/h"" },
            ""current"": {
                ""time"": ""2026-09-09T12:00"",
                ""temperature_2m"": 18.0,
                ""relative_humidity_2m"": 55,
                ""wind_speed_10m"": 12.0,
                ""wind_direction_10m"": 180,
                ""weather_code"": 0
            },
            ""hourly"": {
                ""time"": [""2026-09-09T12:00"", ""2026-09-09T13:00"", ""2026-09-09T14:00"", ""2026-09-09T15:00""],
                ""temperature_2m"": [18.0, 19.0, 19.5, 18.5],
                ""precipitation_probability"": [0, 0, 10, 10],
                ""weather_code"": [0, 0, 1, 1]
            },
            ""daily"": {
                ""time"": [""2026-09-09"", ""2026-09-10"", ""2026-09-11""],
                ""sunrise"": [""2026-09-09T06:30"", ""2026-09-10T06:31"", ""2026-09-11T06:33""],
                ""sunset"": [""2026-09-09T19:45"", ""2026-09-10T19:43"", ""2026-09-11T19:41""],
                ""temperature_2m_max"": [20.0, 21.0, 18.0],
                ""temperature_2m_min"": [11.0, 12.0, 10.0]
            }
        }";
    }
}