using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class GiftPackageController : MonoBehaviour
{
	public Image m_gitPackageIcon;

	public Text m_PackageName;

	private List<object> loadedAsserts = new List<object>();

	private AssetLoadCallbacks m_assetLoadCallBack;

	private bool m_IsRelease;

	public RewardItemController m_rewardItem;

	public GameObject m_rewardContent;

	public DiscountController m_discountController;

	public Text m_originalPriceTxt;

	public Text m_discountPriceTxt;

	public Transform m_originalPricePos;

	public Vector3 m_discountOriginalPricePos;

	public GameObject m_buyBtn;

	private List<RewardItemController> m_rewardList = new List<RewardItemController>();

	[Header("打折")]
	public GameObject m_discountGameObject;

	public Text m_discountTxt;

	[Header("当前的shopID")]
	[SerializeField]
	[Label]
	private int m_giftPackageId;

	public void Init()
	{
		m_IsRelease = false;
		m_discountOriginalPricePos = m_originalPriceTxt.transform.localPosition;
		if (!m_gitPackageIcon)
		{
			return;
		}
		m_gitPackageIcon.gameObject.SetActive(false);
		m_assetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
		{
			if (m_IsRelease)
			{
				Mod.Resource.UnloadAsset(asset);
			}
			else
			{
				if ((bool)m_gitPackageIcon)
				{
					m_gitPackageIcon.gameObject.SetActive(true);
				}
				if ((bool)m_gitPackageIcon)
				{
					m_gitPackageIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				}
				loadedAsserts.Add(asset);
			}
		}, delegate(string assetName, string errorMessage, object data2)
		{
			Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
		});
	}

	public void OnOpen()
	{
		AddEventListener();
	}

	public void OnReset()
	{
		RemoveEventListener();
	}

	private void AddEventListener()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_buyBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(Buyhandler));
	}

	private void RemoveEventListener()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_buyBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(Buyhandler));
	}

	private void Buyhandler(GameObject go)
	{
		MonoSingleton<GameTools>.Instacne.CommonBuyOperate(m_giftPackageId);
	}

	public void SetData(int giftPackageId)
	{
		m_giftPackageId = giftPackageId;
		Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[giftPackageId];
		string assetName = MonoSingleton<GameTools>.Instacne.GoodsTeamIconId(shops_shopTable.GoodsTeamid).ToString();
		Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(assetName), m_assetLoadCallBack);
		if ((bool)m_PackageName)
		{
			m_PackageName.text = MonoSingleton<GameTools>.Instacne.GetGoodsTeamName(shops_shopTable.GoodsTeamid);
		}
		SetDiscount(shops_shopTable.Discount);
		ShowGiftPackageContent();
	}

	private void ShowGiftPackageContent()
	{
		Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[m_giftPackageId];
		int goodsTeamid = shops_shopTable.GoodsTeamid;
		GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[goodsTeamid];
		string productRealPrice = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(m_giftPackageId);
		m_originalPriceTxt.text = productRealPrice;
		if (m_discountController != null)
		{
			m_discountController.SetDiscount(shops_shopTable.Discount);
		}
		float result = 0f;
		if (shops_shopTable.Discount >= 0)
		{
			m_originalPriceTxt.transform.localPosition = m_discountOriginalPricePos;
			m_discountPriceTxt.gameObject.SetActive(true);
			if (shops_shopTable.BuyType == 1)
			{
				if (float.TryParse(Regex.Replace(productRealPrice, "[^0-9.-]", ""), out result))
				{
					result = result * 100f / (float)shops_shopTable.Discount;
				}
			}
			else
			{
				result = int.Parse(shops_shopTable.Price);
				result = result * 100f / (float)shops_shopTable.Discount;
			}
			int num = (int)result;
			if (result - (float)num > 0.001f)
			{
				m_discountPriceTxt.text = result.ToString("0.00");
			}
			else
			{
				m_discountPriceTxt.text = num.ToString();
			}
		}
		else
		{
			m_originalPriceTxt.transform.localPosition = m_originalPricePos.localPosition;
			m_discountPriceTxt.gameObject.SetActive(false);
		}
		GameObject gameObject = null;
		RewardItemController rewardItemController = null;
		m_rewardList.Clear();
		Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsTeam_goodsTeamTable.Id).GetEnumerator();
		int i = 0;
		for (int childCount = m_rewardContent.transform.childCount; i < childCount; i++)
		{
			UnityEngine.Object.Destroy(m_rewardContent.transform.GetChild(i).gameObject);
		}
		while (enumerator.MoveNext())
		{
			gameObject = UnityEngine.Object.Instantiate(m_rewardItem.gameObject);
			rewardItemController = gameObject.GetComponent<RewardItemController>();
			gameObject.SetActive(true);
			m_rewardList.Add(rewardItemController);
			gameObject.transform.SetParent(m_rewardContent.transform);
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
			gameObject.transform.localScale = Vector3.one;
			rewardItemController.Init();
			rewardItemController.SetGoodsId(enumerator.Current.Key, enumerator.Current.Value * shops_shopTable.Count);
		}
		m_rewardContent.SetActive(true);
	}

	public void Release()
	{
		m_IsRelease = true;
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}

	private void SetDiscount(bool active, string DiscountValue)
	{
		if (m_discountGameObject != null)
		{
			m_discountGameObject.SetActive(active);
		}
		if (m_discountTxt != null)
		{
			m_discountTxt.text = DiscountValue;
		}
	}

	private void SetDiscount(int discount)
	{
		if (discount <= 0)
		{
			SetDiscount(false, string.Empty);
			return;
		}
		string discountValue = string.Format("{0}% more", Mathf.Abs(discount - 100));
		SetDiscount(true, discountValue);
	}
}
