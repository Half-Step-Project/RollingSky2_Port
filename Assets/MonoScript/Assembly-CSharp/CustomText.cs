using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/CustomText", 21)]
public class CustomText : Text
{
	[SerializeField]
	private int m_LanguageId;

	[SerializeField]
	private SystemLanguage m_Language = SystemLanguage.English;

	private bool isInited;

	public int LanguageId
	{
		get
		{
			return m_LanguageId;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (!isInited)
		{
			if (Mod.Localization != null)
			{
				m_Language = Mod.Localization.Language;
			}
			SetContent();
			isInited = true;
		}
		AddEventListener();
	}

	protected override void OnDestroy()
	{
		RemoveEventListener();
		base.OnDestroy();
	}

	private void AddEventListener()
	{
		Mod.Event.Subscribe(EventArgs<ChangeLanguageArgs>.EventId, ChangeLanguage);
	}

	private void RemoveEventListener()
	{
		Mod.Event.Unsubscribe(EventArgs<ChangeLanguageArgs>.EventId, ChangeLanguage);
	}

	private void ChangeLanguage(object sender, EventArgs e)
	{
		SetContent();
	}

	public void SetContent()
	{
		string text = "";
		SwitchFont(Mod.Localization.Language);
		if (m_LanguageId > 0)
		{
			text = Mod.Localization.GetInfoById(m_LanguageId);
			if (string.IsNullOrEmpty(text))
			{
				Log.Error("Language  is Null Id is:" + m_LanguageId);
			}
			else
			{
				this.text = text.Replace("\\n", "\n");
			}
		}
	}

	public void SetContent(int languageId)
	{
		m_LanguageId = languageId;
		SetContent();
	}

	private Font LoadFontByName(string defaultFontName, string fontFormat)
	{
		Font obj = Resources.Load<Font>(string.Format("Builtin/{0}", defaultFontName));
		if (obj == null)
		{
			Log.Warning(string.Format("{0} font can not be loaded!", defaultFontName));
		}
		return obj;
	}

	public void SwitchFont(SystemLanguage m_Language)
	{
		string text = "AvenirNextCondensedRegular";
		string fontFormat = "ttf";
		string value = "SavanaRegular";
		string text2 = "HYQiHeiX2-45W";
		string fontFormat2 = "otf";
		string text3 = ((base.font != null) ? base.font.name : "");
		Font font = null;
		switch (m_Language)
		{
		case SystemLanguage.English:
			if (!text3.Equals(text) && !text3.Equals(value))
			{
				font = (base.font = LoadFontByName(text, fontFormat));
			}
			break;
		case SystemLanguage.Chinese:
		case SystemLanguage.ChineseSimplified:
			if (!text3.Equals(text2))
			{
				font = (base.font = LoadFontByName(text2, fontFormat2));
			}
			break;
		default:
			if (!text3.Equals(text))
			{
				font = (base.font = LoadFontByName(text, fontFormat));
			}
			break;
		}
	}
}
