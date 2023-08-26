using Foundation;
using UnityEngine;

namespace RS2
{
	public static class LocalizationExtension
	{
		public static void LoadDictionary(this LocalizationMod localization, string dictionaryName, object userData = null)
		{
			if (string.IsNullOrEmpty(dictionaryName))
			{
				Log.Warning("Dictionary name is invalid.");
			}
			else
			{
				localization.Load(AssetUtility.GetDictionaryAsset(dictionaryName), userData);
			}
		}

		public static string GetInfoById(this LocalizationMod localization, int id)
		{
			string result = "null";
			if (id < 1)
			{
				return result;
			}
			Language_languagetable language_languagetable = Mod.DataTable.Get<Language_languagetable>()[id];
			switch (localization.Language)
			{
			case SystemLanguage.English:
				return language_languagetable.English;
			case SystemLanguage.Spanish:
				return language_languagetable.Spanish;
			case SystemLanguage.ChineseSimplified:
				return language_languagetable.ChineseSimplified;
			case SystemLanguage.ChineseTraditional:
				return language_languagetable.ChineseTraditional;
			case SystemLanguage.Japanese:
				return language_languagetable.Japanese;
			case SystemLanguage.French:
				return language_languagetable.French;
			case SystemLanguage.German:
				return language_languagetable.German;
			case SystemLanguage.Italian:
				return language_languagetable.Italian;
			case SystemLanguage.Dutch:
				return language_languagetable.Dutch;
			case SystemLanguage.Russian:
				return language_languagetable.Russian;
			case SystemLanguage.Korean:
				return language_languagetable.Korean;
			default:
				return language_languagetable.English;
			}
		}
	}
}
