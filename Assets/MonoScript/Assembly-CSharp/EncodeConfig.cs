using System;
using RisingWin.Library;

public static class EncodeConfig
{
	public static void setConfig(string key, string values)
	{
		PlayerPrefsAdapter.SetString(SystemSafety.StringEncryptByDES(key), SystemSafety.StringEncryptByDES(values));
	}

	public static string getConfig(string key, string defaultValue = "")
	{
		string key2 = SystemSafety.StringEncryptByDES(key);
		if (PlayerPrefsAdapter.HasKey(key2))
		{
			return SystemSafety.StringDecryptByDES(PlayerPrefsAdapter.GetString(key2));
		}
		return defaultValue;
	}

	public static void clearConfig(string key)
	{
		string key2 = SystemSafety.StringEncryptByDES(key);
		if (PlayerPrefsAdapter.HasKey(key2))
		{
			PlayerPrefsAdapter.DeleteKey(key2);
		}
	}

	public static void setInt(string key, int values)
	{
		PlayerPrefsAdapter.SetString(SystemSafety.StringEncryptByDES(key), SystemSafety.StringEncryptByDES(values.ToString()));
	}

	public static int getInt(string key, int defaultValue = 0)
	{
		string key2 = SystemSafety.StringEncryptByDES(key);
		if (PlayerPrefsAdapter.HasKey(key2))
		{
			string s = SystemSafety.StringDecryptByDES(PlayerPrefsAdapter.GetString(key2));
			try
			{
				return int.Parse(s);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
		return defaultValue;
	}
}
