using My.Core;

namespace RS2
{
	public sealed class CloudConfigHelper : My.Core.Singleton<CloudConfigHelper>
	{
		private const string textConfig1_def = "";

		public string TextConfig1
		{
			get
			{
				return GetCloudConfigString(FunctionType.GameConfig_IOS, SelectionType.clould_gamecommon_config, CloudConfigKey.gamecommon_config, "");
			}
		}

		private string GetCloudConfigString(FunctionType func, SelectionType selection, CloudConfigKey key, string def)
		{
			string empty = string.Empty;
			return CMPlaySDKUtils.Instance.getStringValue((int)func, selection.ToString(), key.ToString(), def);
		}

		private int GetCloudConfigInt(FunctionType func, SelectionType selection, CloudConfigKey key, int def)
		{
			return CMPlaySDKUtils.Instance.getIntValue((int)func, selection.ToString(), key.ToString(), def);
		}

		private long GetCloudConfigLong(FunctionType func, SelectionType selection, CloudConfigKey key, long def)
		{
			return CMPlaySDKUtils.Instance.getLongValue((int)func, selection.ToString(), key.ToString(), def);
		}

		private bool GetCloudConfigBoolean(FunctionType func, SelectionType selection, CloudConfigKey key, bool def)
		{
			return CMPlaySDKUtils.Instance.getBooleanValue((int)func, selection.ToString(), key.ToString(), def);
		}

		private double GetCloudConfigDouble(FunctionType func, SelectionType selection, CloudConfigKey key, double def)
		{
			return CMPlaySDKUtils.Instance.getDoubleValue((int)func, selection.ToString(), key.ToString(), def);
		}

		public void PullCloudConfigDataWithLanguage(string lan)
		{
			CMPlaySDKUtils.Instance.PullCloudConfigData(lan);
		}
	}
}
