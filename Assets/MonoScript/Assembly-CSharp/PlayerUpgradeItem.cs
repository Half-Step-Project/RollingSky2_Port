using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeItem : MonoBehaviour
{
	public PlayerUpgradeForm m_playerUpgradeForm;

	public PlayerUpgradeData m_playerUpgradeData;

	public Text m_showNumText;

	public Text m_showNumText2;

	public Text m_showTextText;

	public Text m_showTextText2;

	public Text m_showTitleText;

	public Text m_showTitleText2;

	public GameObject m_enableItem;

	public GameObject m_disableItem;

	public Image m_enableIcon;

	public Image m_disableIcon;

	private List<object> loadedAsserts = new List<object>();

	public void SetData(PlayerUpgradeData data)
	{
		m_playerUpgradeData = data;
		m_showNumText.text = data.m_showNum.ToString();
		m_showNumText2.text = data.m_showNum.ToString();
		m_showTextText.text = ((data.m_showText != null) ? data.m_showText : "");
		m_showTextText2.text = ((data.m_showText != null) ? data.m_showText : "");
		m_showTitleText.text = data.m_showTitle;
		m_showTitleText2.text = data.m_showTitle;
		m_enableItem.SetActive(data.m_ifUnlock);
		m_disableItem.SetActive(!data.m_ifUnlock);
		if (m_enableIcon.sprite == null)
		{
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(m_playerUpgradeData.m_iconId), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (m_enableIcon != null)
				{
					m_enableIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					loadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			}));
		}
		if (!(m_disableIcon.sprite == null))
		{
			return;
		}
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(-m_playerUpgradeData.m_iconId), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_disableIcon != null)
			{
				m_disableIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		}));
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}
}
