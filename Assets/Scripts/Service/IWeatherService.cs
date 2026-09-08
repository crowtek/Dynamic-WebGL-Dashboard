using UnityEngine;

public interface IWeatherService
{
    Awaitable<OpenMeteoResponse> GetWeatherAsync(float latitude, float longitude);
}