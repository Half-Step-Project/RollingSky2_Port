using System;
using System.Globalization;

public class TimeTools
{
	public static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private const double cntTime = 8.0;

	public static ulong ChangeUnixTime(DateTime time)
	{
		return (ulong)(time.ToUniversalTime() - unixEpoch).TotalMilliseconds;
	}

	public static ulong ChangeUnixTimeCHT(DateTime time)
	{
		return ChangeUnixTime(time.AddHours(-8.0));
	}

	public static DateTime ChangeDateTime(ulong time)
	{
		return unixEpoch.AddMilliseconds(time);
	}

	public static DateTime ChangeDateTimeCHT(ulong time)
	{
		return ChangeDateTime(time).AddHours(8.0);
	}

	public static DateTime GetLocalTime()
	{
		return DateTime.UtcNow;
	}

	public static bool IsValidTime(ulong startTime, ulong endTime, ulong checkTime)
	{
		if (endTime == 0L)
		{
			endTime = ulong.MaxValue;
		}
		if (checkTime >= startTime)
		{
			return checkTime <= endTime;
		}
		return false;
	}

	public static bool IsSameDay(DateTime start, DateTime end)
	{
		int num = (int)start.ToOADate();
		int num2 = (int)end.ToOADate();
		if (num >= num2)
		{
			return true;
		}
		return false;
	}

	public static string DateTimeToString(DateTime dateTime, string dateFormat = "yyyy:MM:dd HH:mm:ss.fff")
	{
		return dateTime.ToString(dateFormat);
	}

	public static DateTime StringToDatetime(string dateTimeStr, string dateFormat = "yyyy:MM:dd HH:mm:ss.fff")
	{
		if (string.IsNullOrEmpty(dateTimeStr))
		{
			return DateTime.Now;
		}
		return DateTime.ParseExact(dateTimeStr, dateFormat, CultureInfo.InvariantCulture);
	}
}
