using System.Threading.Tasks;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField] private float latitude = 0f;
    [SerializeField] private float longitude = 0f;

    private WeatherService _weatherService;

    void Awake()
    {
        _weatherService = new WeatherService();
    }

    private async Awaitable Start()
    {
        OpenMeteoResponse weather = await _weatherService.GetWeatherAsync(latitude, longitude);

        if (weather != null)
        {
            Debug.Log($"Temperature: {weather.current.temperature_2m} {weather.current_units.temperature_2m}");
            Debug.Log($"Humidity: {weather.current.relative_humidity_2m} {weather.current_units.relative_humidity_2m}");
        }
    }
}
