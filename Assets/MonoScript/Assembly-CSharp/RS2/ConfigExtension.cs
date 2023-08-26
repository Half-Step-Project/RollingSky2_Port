using Foundation;

namespace RS2
{
	public static class ConfigExtension
	{
		public static void LoadConfig(this ConfigMod self, string name, object userData = null)
		{
			if (string.IsNullOrEmpty(name))
			{
				Log.Warning("Config name is invalid.");
			}
			else
			{
				self.Load(AssetUtility.GetConfigAsset(name), userData);
			}
		}
	}
}
