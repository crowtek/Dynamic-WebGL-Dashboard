using UnityEngine;
using UnityEngine.UIElements;

public class WeatherDashboardView
{
    private Label _locationLabel;
    private Label _dateLabel;
    private Label _tempLabel;
    private VisualElement _heroIcon;

    private Label _sunriseLabel;
    private Label _sunsetLabel;
    private Label _windLabel;
    private Label _humidityLabel;

    private readonly Label[] _hourlyTimeLabels = new Label[4];
    private readonly Label[] _hourlyRainLabels = new Label[4];
    private readonly VisualElement[] _hourlyIconElements = new VisualElement[4];
    private readonly Label[] _hourlyTempLabels = new Label[4];

    private readonly Label[] _dailyDateLabels = new Label[3];
    private readonly Label[] _dailyRangeLabels = new Label[3];

    public void Initialize(VisualElement root)
    {
        _locationLabel = root.Q<Label>("location-label");
        _dateLabel = root.Q<Label>("date-label");
        _tempLabel = root.Q<Label>("temp-label");
        _heroIcon = root.Q<VisualElement>("hero-icon");

        _sunriseLabel = root.Q<Label>("sunrise-label");
        _sunsetLabel = root.Q<Label>("sunset-label");
        _windLabel = root.Q<Label>("wind-label");
        _humidityLabel = root.Q<Label>("humidity-label");

        for (int i = 0; i < 4; i++)
        {
            _hourlyTimeLabels[i] = root.Q<Label>($"hourly-time-{i}");
            _hourlyRainLabels[i] = root.Q<Label>($"hourly-rain-{i}");
            _hourlyIconElements[i] = root.Q<VisualElement>($"hourly-icon-{i}");
            _hourlyTempLabels[i] = root.Q<Label>($"hourly-temp-{i}");
        }

        for (int i = 0; i < 3; i++)
        {
            _dailyDateLabels[i] = root.Q<Label>($"daily-date-{i}");
            _dailyRangeLabels[i] = root.Q<Label>($"daily-range-{i}");
        }
    }

    public void SetHeader(string location, string date)
    {
        if (_locationLabel != null) _locationLabel.text = location;
        if (_dateLabel != null) _dateLabel.text = date;
    }

    public void SetHeroData(string temp, Sprite icon, string humidity, string wind)
    {
        if (_tempLabel != null) _tempLabel.text = temp;
        if (_heroIcon != null && icon != null) _heroIcon.style.backgroundImage = new StyleBackground(icon);
        if (_humidityLabel != null) _humidityLabel.text = humidity;
        if (_windLabel != null) _windLabel.text = wind;
    }

    public void SetSunTimes(string sunrise, string sunset)
    {
        if (_sunriseLabel != null) _sunriseLabel.text = sunrise;
        if (_sunsetLabel != null) _sunsetLabel.text = sunset;
    }

    public void SetHourlySlot(int index, string time, string rain, string temp, Sprite icon)
    {
        if (index < 0 || index >= 4) return;
        if (_hourlyTimeLabels[index] != null) _hourlyTimeLabels[index].text = time;
        if (_hourlyRainLabels[index] != null) _hourlyRainLabels[index].text = rain;
        if (_hourlyTempLabels[index] != null) _hourlyTempLabels[index].text = temp;
        if (_hourlyIconElements[index] != null && icon != null)
        {
            _hourlyIconElements[index].style.backgroundImage = new StyleBackground(icon);
        }
    }

    public void SetDailySlot(int index, string date, string tempRange)
    {
        if (index < 0 || index >= 3) return;
        if (_dailyDateLabels[index] != null) _dailyDateLabels[index].text = date;
        if (_dailyRangeLabels[index] != null) _dailyRangeLabels[index].text = tempRange;
    }
}