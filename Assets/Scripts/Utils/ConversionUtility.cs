using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class ConversionUtility
{
    public static int ConvertToInt(this string entry, int defaultValue = 0)
    {
        if (int.TryParse(entry, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning(string.Format("Unable to convert {0}, default value of {1} returned", entry, defaultValue));
            return defaultValue;
        }
    }

    public static float ConvertToFloat(this string entry, float defaultValue = 0)
    {
        if (float.TryParse(entry, NumberStyles.Any, CultureInfo.InvariantCulture, out float value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning(string.Format("Unable to convert {0}, default value of {1} returned", entry, defaultValue));
            return defaultValue;
        }
    }

    public static bool ConvertToBool(this string entry, bool defaultValue = false)
    {
        if (bool.TryParse(entry, out bool value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning(string.Format("Unable to convert {0}, default value of {1} returned", entry, defaultValue));
            return defaultValue;
        }
    }

    public static byte[] ConvertToByteArray(this string entry)
    {
        return System.Text.Encoding.UTF8.GetBytes(entry);
    }

    public static string ConvertFromByteArray(this byte[] entry)
    {
        return System.Text.Encoding.UTF8.GetString(entry);
    }

    public static string ConvertToId(this string entry)
    {
        return entry.ToLowerInvariant().Trim();
    }

    public static DateTime UnixTimeToDateTime(long unixtime)
    {
        var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
        return dtDateTime;
    }
}
