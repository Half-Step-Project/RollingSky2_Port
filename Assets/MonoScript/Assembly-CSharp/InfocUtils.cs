using System;

public static class InfocUtils
{
	public static string GetPointTime()
	{
		return "&player_time=" + DateTime.Now.ToString("MM-dd-HH:mm:ss") + "&uptime=" + ConvertDateTimeInt(DateTime.Now);
	}

	public static long ConvertDateTimeInt(DateTime time)
	{
		DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
		return (time.Ticks - dateTime.Ticks) / 10000000;
	}

	public static void ReportInfoc(string table, string info, bool isForce = true)
	{
	}

	public static void ReportEvent(int action)
	{
	}

	public static void Report_rollingsky2_games_currency(int powerups, int cost, int produce, int level = 0, int remark = 0)
	{
		string info = string.Format("powerups={0}&cost={1}&produce={2}&level={3}&remark={4}", powerups, cost, produce, level, remark);
		ReportInfoc(InfocTable.rollingsky2_games_currency, info);
	}

	public static void Report_rollingsky2_games_pageshow(int page, int button, int action, int level = 0, int button_source = 0, int days = 0)
	{
		string info = string.Format("page={0}&button={1}&level={2}&button_source={3}&action={4}&days={5}", page, button, level, button_source, action, days);
		ReportInfoc(InfocTable.rollingsky2_games_pageshow, info);
	}

	public static void Report_rollingsky2_games_neigou(int id, int action, int remark = 0)
	{
		string info = string.Format("purchaseTime={0}&purchasetoken={1}&signature={2}&scenes={3}&action={4}&remark={5}", "0", "0", "0", id, action, remark);
		ReportInfoc(InfocTable.rollingsky2_games_neigou, info);
	}

	public static void Report_rollingsky2_games_Unlock(int action, int levelId)
	{
		string info = string.Format("action={0}&level={1}", action, levelId);
		ReportInfoc(InfocTable.rollingsky2_games_Unlock, info);
	}

	public static void Report_rollingsky2_games_gamefiller(int scenes, int action = 0)
	{
		string info = string.Format("scenes={0}&action={1}", scenes, action);
		ReportInfoc(InfocTable.rollingsky2_games_gamefiller, info);
	}

	public static void Report_rollingsky2_games_ads(int scenes, int scenes_source, int ad_source, int level, int action, int source)
	{
		string info = string.Format("scenes={0}&scenes_source={1}&ad_source={2}&action={3}&source={4}&level={5}", scenes, scenes_source, ad_source, action, source, level);
		ReportInfoc(InfocTable.rollingsky2_games_ads, info);
	}

	public static void Report_rollingsky2_games_Runningtime(int level, int scenes, int run_score)
	{
		string info = string.Format("scenes={0}&run_score={1}&level={2}", scenes, run_score, level);
		ReportInfoc(InfocTable.rollingsky2_games_Runningtime, info);
	}

	public static void Report_rollingsky2_games_levelnum(int level, int score, int diamond, int crown, bool livebuff, bool shieldbuff, bool revivebuff, int session, int box)
	{
		string info = string.Format("level={0}&score={1}&diamond={2}&crown={3}&session={4}&revive={5}&revive_time={6}&livebuff={7}&shieldbuff={8}&revivebuff={9}&box={10}&boxlevel={11}", level, score, diamond, crown, session, 0, 0, livebuff ? 1 : 0, shieldbuff ? 1 : 0, revivebuff ? 1 : 0, box, level);
		ReportInfoc(InfocTable.rollingsky2_games_levelnum, info);
	}

	public static void Report_rollingsky2_games_Mission(int scenes, int star)
	{
		string info = string.Format("scenes={0}&star={1}", scenes, star);
		ReportInfoc(InfocTable.rollingsky2_games_Mission, info);
	}

	public static void ReportRewardAdShowChance(int scene = 0)
	{
		CMPAdsManager.ReportRewardAdShowChance(scene);
	}

	public static void ReportInterstitialAdShowChance(int scene = 0)
	{
		CMPAdsManager.ReportInterstitialAdShowChance(scene);
	}
}
