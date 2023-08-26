using Foundation;
using UnityEngine;

namespace RS2
{
	public class SaveDate : MonoBehaviour
	{
		private void Update()
		{
			if (InputService.KeyDown(KeyCode.R))
			{
				PlayerPrefs.DeleteAll();
				Debug.Log("Delete All");
			}
			if (InputService.KeyDown(KeyCode.J))
			{
				Mod.Localization.Language = SystemLanguage.Japanese;
				Mod.Event.FireNow(this, Mod.Reference.Acquire<ChangeLanguageArgs>());
				Debug.Log("語言 日本語");
			}
			if (InputService.KeyDown(KeyCode.C))
			{
				Mod.Localization.Language = SystemLanguage.ChineseTraditional;
				Mod.Event.FireNow(this, Mod.Reference.Acquire<ChangeLanguageArgs>());
				Debug.Log("語言 中文");
			}
			if (InputService.KeyDown(KeyCode.E))
			{
				Mod.Localization.Language = SystemLanguage.English;
				Mod.Event.FireNow(this, Mod.Reference.Acquire<ChangeLanguageArgs>());
				Debug.Log("語言 English");
			}
		}

		private void OnGUI()
		{
		}
	}
}
