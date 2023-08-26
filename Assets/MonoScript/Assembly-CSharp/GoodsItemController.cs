using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class GoodsItemController : MonoBehaviour
{
	public Text m_NumTxt;

	public Text m_ContentTxt;

	public Image m_goodsIcon;

	private int goodsId;

	private bool m_isShwoTip;

	private AssetLoadCallbacks m_assetLoadCallBack;

	private List<object> loadedAsserts = new List<object>();

	private bool m_isReleased;

	public void Init()
	{
		m_goodsIcon.gameObject.SetActive(false);
		m_isReleased = false;
		m_assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_isReleased)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			else if (m_goodsIcon != null)
			{
				m_goodsIcon.gameObject.SetActive(true);
				m_goodsIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		});
	}

	public void SetGoodsId(int goodsId, int count, string content, bool isShowTip)
	{
		this.goodsId = goodsId;
		m_isShwoTip = isShowTip;
		string assetName = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(goodsId).ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), m_assetLoadCallBack);
		if (count > 0)
		{
			m_NumTxt.gameObject.SetActive(true);
			m_NumTxt.text = string.Format("X{0}", count);
		}
		if (!string.IsNullOrEmpty(content))
		{
			m_ContentTxt.text = content;
		}
	}

	public void OnClickItem()
	{
		if (goodsId != 0 && m_isShwoTip)
		{
			AssetPromptBoxFormData assetPromptBoxFormData = new AssetPromptBoxFormData();
			assetPromptBoxFormData.m_target = GetComponent<RectTransform>();
			assetPromptBoxFormData.m_goodID = goodsId;
			Mod.UI.OpenUIForm(UIFormId.AssetPromptBoxForm, assetPromptBoxFormData);
		}
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
