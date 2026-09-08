using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeatherData", menuName = "Scriptable Objects/Weather Data")]
public class WeatherData : ScriptableObject
{
    [Header("Current Conditions")]
    public string LocationName;
    public float Temperature;
    public int WeatherCode;
    public float Humidity;
    public float WindSpeed;


    [Header("Environment State Flags")]
    public bool IsRaining;
    public bool IsClear;


    [Header("Raw Forecast Reference")]
    public OpenMeteoResponse RawResponse;

    public event Action OnWeatherChanged;

    public void Populate(OpenMeteoResponse response)
    {
        if (response == null) return;
        RawResponse = response;

        if (response.current != null)
        {
            Temperature = response.current.temperature_2m;
            WeatherCode = response.current.weather_code;
            Humidity = response.current.relative_humidity_2m;
            WindSpeed = response.current.wind_speed_10m;

            IsRaining = WeatherCode is (>= 51 and <= 67) or (>= 80 and <= 82) or (>= 95 and <= 99);
            IsClear = WeatherCode == 0;
        }

        OnWeatherChanged?.Invoke();
    }
}
