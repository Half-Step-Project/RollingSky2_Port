using Foundation;
using RisingWin.Library;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RS2
{
	public class LanguageForm : MonoBehaviour
	{
		public MenuForm m_menuForm;

		public Sprite img_enable;

		public Sprite img_disenble;

		public Transform tr_buttons;

		private Image previousBtnImage;

		private Image nowBtnImage;

		public Button defaultButton;

		public void CloseLanguageForm()
		{
			m_menuForm.CloseLanguageForm();
		}

		protected virtual void DefaultSelect()
		{
			if (null != defaultButton && defaultButton.gameObject.activeSelf)
			{
				ButtonTool.Select(defaultButton);
			}
		}

		protected virtual void OnEnable()
		{
			BtnImageInit();
			DefaultSelect();
		}

		public void OnSelect(Button button)
		{
			ButtonTool.Select(button);
		}

		private void BtnImageInit()
		{
			int num = 0;
			switch (Mod.Localization.Language)
			{
			case SystemLanguage.English:
				num = 0;
				break;
			case SystemLanguage.Japanese:
				num = 1;
				break;
			case SystemLanguage.ChineseSimplified:
				num = 2;
				break;
			case SystemLanguage.ChineseTraditional:
				num = 3;
				break;
			case SystemLanguage.Spanish:
				num = 4;
				break;
			case SystemLanguage.French:
				num = 5;
				break;
			case SystemLanguage.German:
				num = 6;
				break;
			case SystemLanguage.Italian:
				num = 7;
				break;
			case SystemLanguage.Dutch:
				num = 8;
				break;
			case SystemLanguage.Russian:
				num = 9;
				break;
			case SystemLanguage.Korean:
				num = 10;
				break;
			default:
				num = 0;
				break;
			}
			defaultButton = tr_buttons.GetChild(num).GetComponent<Button>();
			previousBtnImage = (nowBtnImage = defaultButton.GetComponent<Image>());
			nowBtnImage.sprite = img_enable;
		}

		private void OnSelectLanguage()
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<ChangeLanguageArgs>());
			PlayerPrefsAdapter.SetInt(PlayerLocalDatakey.SETTING_LANGUAGE, (int)Mod.Localization.Language);
			previousBtnImage = nowBtnImage;
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				nowBtnImage = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
				previousBtnImage.sprite = img_disenble;
				nowBtnImage.sprite = img_enable;
			}
		}

		public void English()
		{
			Mod.Localization.Language = SystemLanguage.English;
			OnSelectLanguage();
		}

		public void Japanese()
		{
			Mod.Localization.Language = SystemLanguage.Japanese;
			OnSelectLanguage();
		}

		public void ChineseSimplified()
		{
			Mod.Localization.Language = SystemLanguage.ChineseSimplified;
			OnSelectLanguage();
		}

		public void ChineseTraditional()
		{
			Mod.Localization.Language = SystemLanguage.ChineseTraditional;
			OnSelectLanguage();
		}

		public void Spanish()
		{
			Mod.Localization.Language = SystemLanguage.Spanish;
			OnSelectLanguage();
		}

		public void French()
		{
			Mod.Localization.Language = SystemLanguage.French;
			OnSelectLanguage();
		}

		public void German()
		{
			Mod.Localization.Language = SystemLanguage.German;
			OnSelectLanguage();
		}

		public void Italian()
		{
			Mod.Localization.Language = SystemLanguage.Italian;
			OnSelectLanguage();
		}

		public void Dutch()
		{
			Mod.Localization.Language = SystemLanguage.Dutch;
			OnSelectLanguage();
		}

		public void Russian()
		{
			Mod.Localization.Language = SystemLanguage.Russian;
			OnSelectLanguage();
		}

		public void Korean()
		{
			Mod.Localization.Language = SystemLanguage.Korean;
			OnSelectLanguage();
		}
	}
}
