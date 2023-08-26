using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class LoadIconController : MonoBehaviour
{
	public Image m_Icon;

	private int m_iconId;

	private AssetLoadCallbacks m_assetLoadCallBack;

	private List<object> loadedAsserts = new List<object>();

	private bool m_isReleased;

	private void Awake()
	{
		if (m_Icon != null)
		{
			m_Icon.gameObject.SetActive(false);
		}
	}

	public void Init()
	{
		m_Icon.gameObject.SetActive(false);
		m_isReleased = false;
		m_assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_isReleased)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			else if (m_Icon != null)
			{
				m_Icon.gameObject.SetActive(true);
				m_Icon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		});
	}

	public void SetIconId(int iconId)
	{
		m_iconId = iconId;
		string assetName = iconId.ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), m_assetLoadCallBack);
	}

	public void Release()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
		m_isReleased = true;
	}
}
