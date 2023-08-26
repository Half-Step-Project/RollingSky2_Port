using System;
using UnityEngine;

namespace Foundation
{
	[Serializable]
	public sealed class BuildInfo
	{
		[Serializable]
		public sealed class VersionInfo
		{
			public string Platform;

			public Version Version;
		}

		[Serializable]
		public sealed class Version
		{
			public int VersionListLength;

			public uint VersionListCrc;
		}

		public bool ForceUpdateGame;

		public string ApplicableGameVersion;

		public int InternalResourceVersion;

		public string ResourceUrlPrefix;

		public VersionInfo[] VersionInfos;

		private Version m_CurrentVersion;

		private string m_CurrentFolder;

		public Version CurrentVersion
		{
			get
			{
				if (m_CurrentVersion == null)
				{
					string text = null;
					switch (Application.platform)
					{
					case RuntimePlatform.WindowsPlayer:
					case RuntimePlatform.WindowsEditor:
						text = "StandaloneWindows";
						m_CurrentFolder = "windows";
						break;
					case RuntimePlatform.Android:
						text = "Android";
						m_CurrentFolder = "android";
						break;
					case RuntimePlatform.IPhonePlayer:
						text = "iOS";
						m_CurrentFolder = "ios";
						break;
					default:
						return null;
					}
					for (int i = 0; i < VersionInfos.Length; i++)
					{
						if (VersionInfos[i].Platform == text)
						{
							m_CurrentVersion = VersionInfos[i].Version;
							break;
						}
					}
				}
				return m_CurrentVersion;
			}
		}

		public string CurrentFolder
		{
			get
			{
				Version currentVersion = CurrentVersion;
				return m_CurrentFolder;
			}
		}

		public static BuildInfo FromJson(string json)
		{
			return JsonUtility.FromJson<BuildInfo>(json);
		}

		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}
	}
}
