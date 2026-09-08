using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class OpenMeteoResponse
{
    public float latitude;
    public float longitude;
    public CurrentUnits current_units;
    public CurrentData current;
}

[Serializable]
public class CurrentUnits
{
    public string time;
    public string temperature_2m;
    public string relative_humidity_2m;
}

[Serializable]
public class CurrentData
{
    public string time;
    public float temperature_2m;
    public float relative_humidity_2m;
}

public class WeatherService
{
    private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";

    public async Awaitable<OpenMeteoResponse> GetWeatherAsync(float latitude, float longitude)
    {
        string url = $"{BaseUrl}?latitude={latitude}&longitude={longitude}&current=temperature_2m,relative_humidity_2m";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[WeatherService] HTTP Error: {request.error}");
                return null;
            }

            string jsonResponse = request.downloadHandler.text;
            return JsonUtility.FromJson<OpenMeteoResponse>(jsonResponse);
        }
    }

}
