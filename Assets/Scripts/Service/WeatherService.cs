using System;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class OpenMeteoResponse
{
    public float latitude;
    public float longitude;
    public CurrentData current;
    public CurrentUnits current_units;
    public HourlyData hourly;
    public DailyData daily;
}

[Serializable]
public class CurrentUnits
{
    public string temperature_2m;
    public string relative_humidity_2m;
    public string apparent_temperature;
    public string surface_pressure;
    public string wind_speed_10m;
}

[Serializable]
public class CurrentData
{
    public string time;
    public float temperature;
    public float humidity;
    public float wind_speed;
    public int weather_code;
}

[Serializable]
public class HourlyData
{
    public string[] time;
    public float[] temperature;
    public int[] rain_probability;
    public float[] visibility;
    public float[] uv_index;
    public int[] weather_code;
}

[Serializable]
public class DailyData
{
    public string[] time;
    public string[] sunrise;
    public string[] sunset;
    public float[] temperature_max;
    public float[] temperature_min;
    public int[] weather_code;
}

public class WeatherService: IWeatherService
{
    private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";

    public async Awaitable<OpenMeteoResponse> GetWeatherAsync(float latitude, float longitude)
    {
        string currentParams = "current=temperature_2m,relative_humidity_2m,apparent_temperature,surface_pressure,wind_speed_10m,wind_direction_10m,weather_code";
        string hourlyParams = "hourly=temperature_2m,precipitation_probability,visibility,uv_index,weather_code";
        string dailyParams = "daily=temperature_2m_max,temperature_2m_min,sunrise,sunset,weather_code";

        string url = $"{BaseUrl}?latitude={latitude}&longitude={longitude}&{currentParams}&{hourlyParams}&{dailyParams}&timezone=auto";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[WeatherService] HTTP Error: {request.error}");
                return null;
            }

            return JsonUtility.FromJson<OpenMeteoResponse>(request.downloadHandler.text);
        }
    }
}