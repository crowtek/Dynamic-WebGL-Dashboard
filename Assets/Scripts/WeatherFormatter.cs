using System;
using UnityEngine;

/// <summary>
///  Utility class for formatting weather related data, such as time, date, and wind direction. 
/// </summary>

public static class WeatherFormatter
{
    public static string FormatIsoTime(string iso)
        => DateTime.TryParse(iso, out DateTime t) ? t.ToString("hh:mm tt") : iso;

    public static string FormatHourTime(string iso)
        => DateTime.TryParse(iso, out DateTime t) ? t.ToString("HH:mm") : iso;

    public static string FormatDailyDate(string iso)
        => DateTime.TryParse(iso, out DateTime t) ? t.ToString("d MMM") : iso;

    public static bool IsHourDaytime(string isoTime)
        => DateTime.TryParse(isoTime, out DateTime t) && t.Hour >= 6 && t.Hour < 21;

    public static int GetCurrentHourIndex(string[] hourlyTimes)
    {
        if (hourlyTimes == null) return 0;

        DateTime now = DateTime.Now;
        for (int i = 0; i < hourlyTimes.Length; i++)
        {
            if (DateTime.TryParse(hourlyTimes[i], out DateTime t) && t.Date == now.Date && t.Hour == now.Hour)
                return i;
        }
        return 0;
    }

    public static string GetCardinalDirection(float degrees)
    {
        string[] dirs = { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
        return dirs[Mathf.RoundToInt(degrees / 45f) % 8];
    }
}