using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIFreeProductItem : MonoBehaviour
{
	public enum ButtonState
	{
		FREE,
		NONEAD,
		COMETOMORROW
	}

	private class GoodsTeamData
	{
		public int m_goodID;

		public int m_count;

		public GoodsTeamData(int goodID, int count)
		{
			m_goodID = goodID;
			m_count = count;
		}
	}

	public Image m_productIcon1;

	public Text m_productCount1;

	public Image m_productIcon2;

	public Text m_productCount2;

	public Button m_buyButton;

	public Image m_buttonButtonIcon;

	public Text m_buttonButtonText;

	public Text m_buttonButtonOtherText;

	private int m_shopTableID = 3001;

	private int m_buttonButtonTextID = 42;

	private int m_buttonButtonInteractableTextID = 43;

	private int m_buttonButtonPreparingTextID = 45;

	private Shops_shopTable m_shopTable;

	private GoodsTeamData[] m_goodsTeamData;

	private List<object> m_loadedAsserts = new List<object>();

	public bool m_isDetectAd;

	public float m_detectAdTime;

	public float m_detectAdIntervalTime = 1f;

	private bool m_isInit;

	private ButtonState m_currentState;

	public bool IsAdCanShow
	{
		get
		{
			return MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.ShopView);
		}
	}

	public bool IsCanShowForCount
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<ShopDataModule>(DataNames.ShopDataModule).m_shopAdTimer.IsCanShowForCount();
		}
	}

	public void OnInit()
	{
		m_isInit = true;
		m_shopTable = Mod.DataTable.Get<Shops_shopTable>().Get(m_shopTableID);
		Dictionary<int, int> goodsTeam = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(m_shopTable.GoodsTeamid, m_shopTable.Count);
		m_goodsTeamData = GetGoodsTeamDataById(goodsTeam);
		if (m_goodsTeamData.Length != 2)
		{
			Log.Error("this data is error,count is twe");
		}
		else
		{
			Goods_goodsTable goods_goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(m_goodsTeamData[0].m_goodID);
			string _productIconName1 = goods_goodsTable.IconId.ToString();
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_productIconName1), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (!m_isInit)
				{
					OnRelease();
				}
				else if (m_productIcon1 != null && asset != null)
				{
					m_productIcon1.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					m_productIcon1.SetNativeSize();
					m_loadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _productIconName1, assetName, errorMessage));
			}));
			Goods_goodsTable goods_goodsTable2 = Mod.DataTable.Get<Goods_goodsTable>().Get(m_goodsTeamData[1].m_goodID);
			string _productIconName2 = goods_goodsTable2.IconId.ToString();
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(_productIconName2), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (!m_isInit)
				{
					OnRelease();
				}
				else if (m_productIcon2 != null && asset != null)
				{
					m_productIcon2.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					m_productIcon2.SetNativeSize();
					m_loadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", _productIconName2, assetName, errorMessage));
			}));
			m_productCount1.text = "X" + m_goodsTeamData[0].m_count;
			m_productCount2.text = "X" + m_goodsTeamData[1].m_count;
		}
		EventTriggerListener.Get(m_buyButton.gameObject).onClick = OnClickBuyButton;
	}

	public void OnOpen()
	{
		RefreshButton(GetRefreshState());
	}

	public void OnUpdate(float elapseSeconds, float realElapseSeconds)
	{
		if (!m_isDetectAd)
		{
			return;
		}
		m_detectAdTime += elapseSeconds;
		if (m_detectAdTime >= m_detectAdIntervalTime)
		{
			if (IsAdCanShow)
			{
				RefreshButton(GetRefreshState());
			}
			m_detectAdTime = 0f;
		}
	}

	public void OnClose()
	{
		m_isDetectAd = false;
	}

	public void OnRelease()
	{
		m_isInit = false;
		for (int i = 0; i < m_loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(m_loadedAsserts[i]);
		}
		m_loadedAsserts.Clear();
	}

	private void OnClickBuyButton(GameObject obj)
	{
		if (IsAdCanShow && IsCanShowForCount)
		{
			MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.ShopView, OnAdSuccessHandle);
		}
	}

	private void OnAdSuccessHandle(ADScene adScene)
	{
		if (adScene == ADScene.ShopView)
		{
			for (int i = 0; i < m_goodsTeamData.Length; i++)
			{
				Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).ChangePlayerGoodsNum(m_goodsTeamData[i].m_goodID, m_goodsTeamData[i].m_count, AssertChangeType.SHOP_KEY);
			}
			Singleton<DataModuleManager>.Instance.GetDataModule<ShopDataModule>(DataNames.ShopDataModule).m_shopAdTimer.ShowCount++;
			RefreshButton(GetRefreshState());
		}
	}

	private void RefreshButton(ButtonState state)
	{
		m_isDetectAd = false;
		switch (state)
		{
		case ButtonState.FREE:
			m_buttonButtonIcon.gameObject.SetActive(true);
			m_buttonButtonText.gameObject.SetActive(true);
			m_buttonButtonOtherText.gameObject.SetActive(false);
			m_buttonButtonText.text = Mod.Localization.GetInfoById(m_buttonButtonTextID);
			m_buyButton.interactable = true;
			break;
		case ButtonState.NONEAD:
			m_buttonButtonIcon.gameObject.SetActive(false);
			m_buttonButtonText.gameObject.SetActive(false);
			m_buttonButtonOtherText.gameObject.SetActive(true);
			m_buttonButtonOtherText.text = Mod.Localization.GetInfoById(m_buttonButtonPreparingTextID);
			m_buyButton.interactable = false;
			m_isDetectAd = true;
			break;
		case ButtonState.COMETOMORROW:
			m_buttonButtonIcon.gameObject.SetActive(false);
			m_buttonButtonText.gameObject.SetActive(false);
			m_buttonButtonOtherText.gameObject.SetActive(true);
			m_buttonButtonOtherText.text = Mod.Localization.GetInfoById(m_buttonButtonInteractableTextID);
			m_buyButton.interactable = false;
			break;
		}
		m_currentState = state;
	}

	private ButtonState GetRefreshState()
	{
		ButtonState buttonState = ButtonState.FREE;
		if (IsCanShowForCount)
		{
			if (IsAdCanShow)
			{
				return ButtonState.FREE;
			}
			return ButtonState.NONEAD;
		}
		return ButtonState.COMETOMORROW;
	}

	private GoodsTeamData[] GetGoodsTeamDataById(Dictionary<int, int> goodsTeam)
	{
		List<GoodsTeamData> list = new List<GoodsTeamData>();
		foreach (KeyValuePair<int, int> item in goodsTeam)
		{
			list.Add(new GoodsTeamData(item.Key, item.Value));
		}
		return list.ToArray();
	}
}
