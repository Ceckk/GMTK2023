using System;

public static class TimeUtility
{
    private const int RESET_HOUR = 3;

    public static int timeOffset = 0;

    public static DateTime GetTime()
    {
        return DateTime.Now.AddMinutes(timeOffset);
    }

    public static DateTime GetTimeUtc()
    {
        return DateTime.UtcNow.AddMinutes(timeOffset);
    }

    public static TimeSpan GetTimeLeftUntil(long fileTime)
    {
        DateTime currentTime = GetTime();
        DateTime targetTime = DateTime.FromFileTime(fileTime);

        TimeSpan diff = targetTime.Subtract(currentTime);

        return diff;
    }

    public static TimeSpan GetUtcTimeLeftUntil(long fileTime)
    {
        DateTime currentTime = GetTimeUtc();
        DateTime targetTime = DateTime.FromFileTimeUtc(fileTime);

        TimeSpan diff = targetTime.Subtract(currentTime);

        return diff;
    }

    public static TimeSpan GetTimeSince(long fileTime)
    {
        DateTime currentTime = GetTime();
        DateTime targetTime = DateTime.FromFileTime(fileTime);

        TimeSpan diff = currentTime.Subtract(targetTime);

        return diff;
    }

    public static TimeSpan GetUtcTimeSince(long fileTime)
    {
        DateTime currentTime = GetTimeUtc();
        DateTime targetTime = DateTime.FromFileTimeUtc(fileTime);

        TimeSpan diff = currentTime.Subtract(targetTime);

        return diff;
    }

    public static DateTime GetNextDayUtc()
    {
        var utc = GetTimeUtc();
        var nextDay = new DateTime(utc.Year, utc.Month, utc.Day, RESET_HOUR, 0, 0);
        if (utc.Hour >= RESET_HOUR)
        {
            nextDay = nextDay.AddDays(1);
        }

        return nextDay;
    }

    public static DateTime GetNextWeekUtc()
    {
        var utc = GetTimeUtc();
        var nextWeek = new DateTime(utc.Year, utc.Month, utc.Day, 0, 0, 0);
        var difference = (7 + DayOfWeek.Monday - nextWeek.DayOfWeek) % 7;

        if (difference == 0)
        {
            nextWeek = nextWeek.AddDays(7);
        }
        else
        {
            nextWeek = nextWeek.AddDays(difference);
        }

        return nextWeek;
    }

    public static long ToUnixSeconds(DateTime date)
    {
        return new DateTimeOffset(date).ToUnixTimeSeconds();
    }

    public static DateTime FromUnixSeconds(long seconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime;
    }
}
