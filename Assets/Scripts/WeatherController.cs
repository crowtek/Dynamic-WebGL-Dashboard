using System;
using UnityEngine;
using UnityEngine.UIElements;

public class WeatherController : MonoBehaviour
{
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

        if (weather != null)
        {
            RenderDashboard(weather);
        }
        else
        {
            Debug.LogError("[WeatherController] Failed to retrieve weather data.");
        }
    }

    private void RenderDashboard(OpenMeteoResponse weather)
    {
        _view.SetHeader(_locationName, DateTime.Now.ToString("dddd, d MMMM"));

        if (weather.current != null)
        {
            bool isDay = DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 21;
            Sprite icon = _iconDatabase.GetWeatherSprite(weather.current.weather_code, isDay);

            string windDir = GetCardinalDirection(weather.current.wind_direction_10m);
            string unit = weather.current_units?.wind_speed_10m ?? "km/h";
            string windText = $"{Mathf.RoundToInt(weather.current.wind_speed_10m)} {unit} {windDir}";

            _view.SetHeroData(
                $"{Mathf.RoundToInt(weather.current.temperature_2m)}°",
                icon,
                $"{weather.current.relative_humidity_2m}%",
                windText
            );
        }

        if (weather.daily?.sunrise != null && weather.daily.sunrise.Length > 0)
        {
            _view.SetSunTimes(FormatIsoTime(weather.daily.sunrise[0]), FormatIsoTime(weather.daily.sunset[0]));
        }

        RenderHourly(weather.hourly);
        RenderDaily(weather.daily);
    }

    private void RenderHourly(HourlyData hourly)
    {
        if (hourly?.time == null) return;

        int startIndex = GetCurrentHourIndex(hourly.time);

        for (int i = 0; i < 4; i++)
        {
            int dataIndex = startIndex + i;
            if (dataIndex >= hourly.time.Length) break;

            string time = (i == 0) ? "Now" : FormatHourTime(hourly.time[dataIndex]);
            string rain = $"{hourly.precipitation_probability[dataIndex]}%";
            string temp = $"{Mathf.RoundToInt(hourly.temperature_2m[dataIndex])}°";

            bool isDay = IsHourDaytime(hourly.time[dataIndex]);
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

            string date = FormatDailyDate(daily.time[i]);
            int min = Mathf.RoundToInt(daily.temperature_2m_min[i]);
            int max = Mathf.RoundToInt(daily.temperature_2m_max[i]);

            _view.SetDailySlot(i, date, $"{min}°/{max}°");
        }
    }

    private bool IsHourDaytime(string isoTime) => DateTime.TryParse(isoTime, out DateTime t) && t.Hour >= 6 && t.Hour < 21;

    private int GetCurrentHourIndex(string[] hourlyTimes)
    {
        DateTime now = DateTime.Now;
        for (int i = 0; i < hourlyTimes.Length; i++)
        {
            if (DateTime.TryParse(hourlyTimes[i], out DateTime t) && t.Date == now.Date && t.Hour == now.Hour)
                return i;
        }
        return 0;
    }

    private string FormatIsoTime(string iso) => DateTime.TryParse(iso, out DateTime t) ? t.ToString("hh:mm tt") : iso;
    private string FormatHourTime(string iso) => DateTime.TryParse(iso, out DateTime t) ? t.ToString("HH:mm") : iso;
    private string FormatDailyDate(string iso) => DateTime.TryParse(iso, out DateTime t) ? t.ToString("d MMM") : iso;

    private string GetCardinalDirection(float degrees)
    {
        string[] dirs = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
        return dirs[Mathf.RoundToInt(degrees / 45f) % 8];
    }
}