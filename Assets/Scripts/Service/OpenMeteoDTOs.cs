using System;

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
    public float temperature_2m;
    public float relative_humidity_2m;
    public float wind_speed_10m;
    public float wind_direction_10m;
    public int weather_code;
}

[Serializable]
public class HourlyData
{
    public string[] time;
    public float[] temperature_2m;
    public int[] precipitation_probability;
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
    public float[] temperature_2m_max;
    public float[] temperature_2m_min;
    public int[] weather_code;
}