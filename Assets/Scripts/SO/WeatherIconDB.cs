using UnityEngine;

[CreateAssetMenu(fileName = "WeatherIconDB", menuName = "Scriptable Objects/Icon Database")]
public class WeatherIconDB : ScriptableObject
{
    [SerializeField] private Sprite _sunnyIcon;
    [SerializeField] private Sprite _clearNightIcon;
    [SerializeField] private Sprite _cloudyIcon;
    [SerializeField] private Sprite _bitCloudyIcon;
    [SerializeField] private Sprite _cloudyNightIcon;
    [SerializeField] private Sprite _rainIcon;
    [SerializeField] private Sprite _stormIcon;
    [SerializeField] private Sprite _snowIcon;

    public Sprite GetWeatherSprite(int code, bool isDay)
    {
        return code switch
        {
            0 => isDay ? _sunnyIcon : _clearNightIcon,
            1 or 2 => isDay ? _bitCloudyIcon : _cloudyNightIcon,
            3 or 45 or 48 => _cloudyIcon,
            >= 51 and <= 67 => _rainIcon,
            >= 71 and <= 77 => _snowIcon,
            >= 80 and <= 82 => _rainIcon,
            >= 85 and <= 86 => _snowIcon,
            >= 95 and <= 99 => _stormIcon,
            _ => _cloudyIcon
        };
    }
}
