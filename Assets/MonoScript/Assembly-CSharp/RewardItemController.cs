using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemController : MonoBehaviour
{
	public Image m_bg;

	public Text m_NumTxt;

	public Image m_awardIcon;

	private AssetLoadCallbacks m_assetLoadCallBack;

	private List<object> loadedAsserts = new List<object>();

	private bool m_isReleased;

	private int goodsId;

	public void Init()
	{
		m_awardIcon.gameObject.SetActive(false);
		m_isReleased = false;
		m_assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_isReleased)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			else if (m_awardIcon != null)
			{
				m_awardIcon.gameObject.SetActive(true);
				m_awardIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		});
	}

	public void SetAwardId(int awardId, int count = 0)
	{
		Award_awardTable award_awardTable = Mod.DataTable.Get<Award_awardTable>()[awardId];
		string assetName = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(award_awardTable.GoodsID).ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), m_assetLoadCallBack);
		int num = award_awardTable.Count;
		if (count > 0)
		{
			num = count;
		}
		m_NumTxt.text = string.Format("X{0}", num);
	}

	public void SetGoodsId(int goodsId, int count = 0)
	{
		this.goodsId = goodsId;
		string assetName = MonoSingleton<GameTools>.Instacne.GetGoodsIconIdByGoodsId(goodsId).ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), m_assetLoadCallBack);
		if (goodsId == GameCommon.REMOVE_AD)
		{
			m_NumTxt.text = "";
		}
		else
		{
			m_NumTxt.text = string.Format("X{0}", count);
		}
	}

	public void OnClickItem()
	{
		if (goodsId != 0)
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
