using System;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// WeatherController is responsible for fetching weather data from the OpenMeteo API and updating the UI accordingly. 
/// It initializes the weather service, retrieves the current weather, hourly forecast, and daily forecast, and renders this information in the UI.
/// </summary>

public class WeatherController : MonoBehaviour
{
    [Header("UI & Configuration")]
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private WeatherIconDB _iconDatabase;
    [SerializeField] private string _locationName = "Hagen, Germany";
    [SerializeField] private float _latitude = 51.3671f;
    [SerializeField] private float _longitude = 7.4633f;

    private IWeatherService _weatherService;
    private WeatherDashboardView _view;

    private void Awake()
    {
        _weatherService = new WeatherService();
        _view = new WeatherDashboardView();

        if (_uiDocument != null)
        {
            _view.Initialize(_uiDocument.rootVisualElement);
        }
        else
        {
            Debug.LogError("[WeatherController] UIDocument is missing!");
        }
    }

    private async Awaitable Start()
    {
        OpenMeteoResponse weather = await _weatherService.GetWeatherAsync(_latitude, _longitude);

        if (weather == null)
        {
            Debug.LogError("[WeatherController] Failed to retrieve weather data.");
            return;
        }

        RenderDashboard(weather);
    }

    private void RenderDashboard(OpenMeteoResponse weather)
    {
        _view.SetHeader(_locationName, DateTime.Now.ToString("dddd, d MMMM"));

        if (weather.current != null)
        {
            RenderCurrentWeather(weather.current, weather.current_units?.wind_speed_10m ?? "km/h");
        }

        if (weather.daily?.sunrise != null && weather.daily.sunrise.Length > 0)
        {
            _view.SetSunTimes(
                WeatherFormatter.FormatIsoTime(weather.daily.sunrise[0]),
                WeatherFormatter.FormatIsoTime(weather.daily.sunset[0])
            );
        }

        RenderHourly(weather.hourly);
        RenderDaily(weather.daily);
    }

    private void RenderCurrentWeather(CurrentData current, string speedUnit)
    {
        bool isDay = DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 21;
        Sprite icon = _iconDatabase.GetWeatherSprite(current.weather_code, isDay);

        string windText = $"{Mathf.RoundToInt(current.wind_speed)} {speedUnit}";

        _view.SetHeroData(
            $"{Mathf.RoundToInt(current.temperature)}°",
            icon,
            $"{current.humidity}%",
            windText
        );
    }

    private void RenderHourly(HourlyData hourly)
    {
        if (hourly?.time == null) return;

        int startIndex = WeatherFormatter.GetCurrentHourIndex(hourly.time);

        for (int i = 0; i < 4; i++)
        {
            int dataIndex = startIndex + i;
            if (dataIndex >= hourly.time.Length) break;

            string time = (i == 0) ? "Now" : WeatherFormatter.FormatHourTime(hourly.time[dataIndex]);
            string rain = $"{hourly.rain_probability[dataIndex]}%";
            string temp = $"{Mathf.RoundToInt(hourly.temperature[dataIndex])}°";

            bool isDay = WeatherFormatter.IsHourDaytime(hourly.time[dataIndex]);
            Sprite icon = _iconDatabase.GetWeatherSprite(hourly.weather_code[dataIndex], isDay);

            _view.SetHourlySlot(i, time, rain, temp, icon);
        }
    }

    private void RenderDaily(DailyData daily)
    {
        if (daily?.time == null) return;

        for (int i = 0; i < 3; i++)
        {
            if (i >= daily.time.Length) break;

            string date = WeatherFormatter.FormatDailyDate(daily.time[i]);
            int min = Mathf.RoundToInt(daily.temperature_min[i]);
            int max = Mathf.RoundToInt(daily.temperature_max[i]);

            _view.SetDailySlot(i, date, $"{min}°/{max}°");
        }
    }
}