using System;
using RisingWin.Library;

namespace RS2
{
	public class PlayerDataDynamic
	{
		public static PlayerDataDynamic Instance;

		private const string STEAM_ID_KEY = "STEAM_ID_KEY";

		private const string CLOUD_VERSION_KEY = "CLOUD_VERSION_KEY";

		public ulong steamID;

		public long cloudVersion;

		public static void Init()
		{
			Instance = new PlayerDataDynamic();
		}

		public PlayerDataDynamic()
		{
			InitPlayer();
		}

		public void Clean()
		{
		}

		internal void SetCloudVersion(object datetime)
		{
			throw new NotImplementedException();
		}

		public void SetSteamID(ulong steamID)
		{
			this.steamID = steamID;
			PlayerPrefsAdapter.SetUlong("STEAM_ID_KEY", this.steamID);
		}

		public void SetCloudVersion(long cloudVersion)
		{
			this.cloudVersion = cloudVersion;
			PlayerPrefsAdapter.SetLong("CLOUD_VERSION_KEY", this.cloudVersion);
		}

		public ulong GetSteamID()
		{
			return PlayerPrefsAdapter.GetUlong("STEAM_ID_KEY", 0uL);
		}

		public long GetCloudVerstion()
		{
			return PlayerPrefsAdapter.GetLong("CLOUD_VERSION_KEY", 0L);
		}

		private void InitPlayer()
		{
		}
	}
}
