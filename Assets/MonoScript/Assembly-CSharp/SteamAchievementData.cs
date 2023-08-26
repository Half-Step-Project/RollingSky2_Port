using RisingWin.Library;

public class SteamAchievementData : CloudAchievementData
{
	public SteamAchievementData(string id)
		: base(id)
	{
	}

	public SteamAchievementData(string id, string name, string description, bool achieved)
		: base(id, name, description, achieved)
	{
	}
}
