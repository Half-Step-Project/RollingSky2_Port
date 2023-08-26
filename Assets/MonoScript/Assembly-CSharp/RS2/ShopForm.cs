using System.Collections.Generic;
using System.Linq;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class ShopForm : UGUIForm, IActiveForm
	{
		public GameObject root;

		public Image back;

		public Image upback;

		public Image downback_0;

		public Image downback_1;

		public Animator mAnimator;

		public ShopUIGroupProductList m_groupProductList;

		private ShopUIGroupProductListData m_groupProductListData;

		public UIPersonalAssetsList m_personalAssets;

		private List<object> loadedAsserts = new List<object>();

		private bool mIsShow;

		private ShopDataModule GetShopDataModule
		{
			get
			{
				return Singleton<DataModuleManager>.Instance.GetDataModule<ShopDataModule>(DataNames.ShopDataModule);
			}
		}

		public bool IsShowForm
		{
			get
			{
				return mIsShow;
			}
		}

		private void Awake()
		{
			root.SetActive(false);
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			InitUI();
			m_groupProductListData = new ShopUIGroupProductListData();
			m_groupProductList.OnInit(m_groupProductListData);
			m_personalAssets.OnInit();
			if (mAnimator == null)
			{
				Transform transform = base.gameObject.transform.Find("animation");
				if (transform != null)
				{
					mAnimator = transform.gameObject.GetComponent<Animator>();
				}
			}
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			NormalForm();
			RefreshNetWorkCommonPackageMoneyData();
			new ShopUITabItemData().m_selectIndex = 0;
			m_groupProductList.gameObject.SetActive(true);
			m_groupProductList.OnOpen();
			m_personalAssets.OnOpen(UIPersonalAssetsList.ParentType.Shop);
			InfocUtils.Report_rollingsky2_games_pageshow(7, 0, 1);
			Mod.Event.Subscribe(EventArgs<BuySuccessEventArgs>.EventId, BuyPackageSuccessHandler);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			Mod.Event.Unsubscribe(EventArgs<BuySuccessEventArgs>.EventId, BuyPackageSuccessHandler);
			m_groupProductList.OnClose();
			m_personalAssets.OnClose();
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			m_personalAssets.OnUpdate();
		}

		protected override void OnUnload()
		{
			m_groupProductList.OnRelease();
			m_personalAssets.OnRelease();
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
			base.OnUnload();
		}

		public void LoadData()
		{
			int num = 2;
			int num2 = 4;
			int num3 = 1;
			m_groupProductListData.m_normalDatas = new Dictionary<ShopType, List<ShopUIProductItemData>>();
			m_groupProductListData.m_responseDatas = new Dictionary<ShopType, ShopResponseData>();
			Shops_shopTable[] records = Mod.DataTable.Get<Shops_shopTable>().Records;
			foreach (Shops_shopTable shops_shopTable in records)
			{
				bool num4 = GetShopDataModule.IsWithinTimeLimit(shops_shopTable.Id);
				bool flag = GetShopDataModule.IsBeyondLimits(shops_shopTable.Id);
				bool flag2 = num4 && !flag;
				if (shops_shopTable.BuyType == num3 && shops_shopTable.Sort == 1 && flag2)
				{
					ShopItemData shopItemData = new ShopItemData();
					shopItemData.Init(shops_shopTable);
					ShopResponseData value = null;
					if (!m_groupProductListData.m_responseDatas.TryGetValue((ShopType)shops_shopTable.Type, out value) || value == null)
					{
						value = new ShopResponseData();
						value.shopItemList = new List<ShopItemData>();
					}
					value.shopItemList.Add(shopItemData);
					m_groupProductListData.m_responseDatas[(ShopType)shops_shopTable.Type] = value;
				}
				if (shops_shopTable.BuyType == num3 || shops_shopTable.Sort != 2 || !flag2)
				{
					continue;
				}
				GoodsTeam_goodsTeamTable goodsTeam_goodsTeamTable = Mod.DataTable.Get<GoodsTeam_goodsTeamTable>().Get(shops_shopTable.GoodsTeamid);
				int result = -1;
				if (!int.TryParse(goodsTeam_goodsTeamTable.GoodsIds, out result) || result == -1)
				{
					Debug.LogError("GoodsTeam_goodsTeamTable ids is error!");
					continue;
				}
				ShopUIProductItemData shopUIProductItemData = new ShopUIProductItemData();
				shopUIProductItemData.m_shopTable = shops_shopTable;
				shopUIProductItemData.m_productGoodsTeamTable = goodsTeam_goodsTeamTable;
				shopUIProductItemData.m_productGoodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(result);
				if (shops_shopTable.BuyType == num)
				{
					shopUIProductItemData.m_moneyGoodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(6);
				}
				else if (shops_shopTable.BuyType == num2)
				{
					shopUIProductItemData.m_moneyGoodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(3);
				}
				if (shopUIProductItemData.m_moneyGoodsTable != null && shops_shopTable.Type != 7)
				{
					List<ShopUIProductItemData> value2 = null;
					if (!m_groupProductListData.m_normalDatas.TryGetValue((ShopType)shops_shopTable.Type, out value2) || value2 == null)
					{
						value2 = new List<ShopUIProductItemData>();
					}
					value2.Add(shopUIProductItemData);
					m_groupProductListData.m_normalDatas[(ShopType)shops_shopTable.Type] = value2;
				}
			}
			ShopType[] array = m_groupProductListData.m_normalDatas.Keys.ToArray();
			for (int j = 0; j < array.Length; j++)
			{
				List<ShopUIProductItemData> source = m_groupProductListData.m_normalDatas[array[j]];
				source = source.OrderBy((ShopUIProductItemData c) => c.m_shopTable.Shopsort).ToList();
				m_groupProductListData.m_normalDatas[array[j]] = source;
			}
			ShopType[] array2 = m_groupProductListData.m_responseDatas.Keys.ToArray();
			for (int k = 0; k < array2.Length; k++)
			{
				List<ShopItemData> shopItemList = m_groupProductListData.m_responseDatas[array2[k]].shopItemList;
				shopItemList = shopItemList.OrderBy((ShopItemData c) => c.shopSort).ToList();
				m_groupProductListData.m_responseDatas[array2[k]].shopItemList = shopItemList;
			}
			int[] array3 = new int[3] { 10, 9, 13 };
			Dictionary<ShopType, ShopResponseData> dictionary = new Dictionary<ShopType, ShopResponseData>();
			for (int l = 0; l < array3.Length; l++)
			{
				ShopResponseData value3 = null;
				if (m_groupProductListData.m_responseDatas.TryGetValue((ShopType)array3[l], out value3) && value3 != null)
				{
					dictionary.Add((ShopType)array3[l], value3);
				}
			}
			m_groupProductListData.m_responseDatas = dictionary;
			RefreshNetWorkCommonPackageMoneyData();
		}

		private void InitUI()
		{
			if (DeviceManager.Instance.IsNeedSpecialAdapte())
			{
				MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(m_personalAssets.transform as RectTransform);
			}
		}

		private void RefreshNetWorkCommonPackageMoneyData()
		{
			ProductInfo productInfo = MonoSingleton<PluginManager>.Instacne.GetProductInfo();
			foreach (KeyValuePair<ShopType, ShopResponseData> responseData in m_groupProductListData.m_responseDatas)
			{
				for (int i = 0; i < responseData.Value.shopItemList.Count; i++)
				{
					string product_id_ios = responseData.Value.shopItemList[i].product_id_ios;
					if (productInfo == null)
					{
						continue;
					}
					for (int j = 0; j < productInfo.productinInfoList.Count; j++)
					{
						if (productInfo.productinInfoList[j].productId == product_id_ios)
						{
							responseData.Value.shopItemList[i].price = productInfo.productinInfoList[j].price;
							break;
						}
					}
				}
			}
		}

		private void BuyPackageSuccessHandler(object sender, EventArgs e)
		{
			if (e is BuySuccessEventArgs)
			{
				m_groupProductList.OnRefresh();
			}
		}

		public void Close()
		{
			HideForm();
		}

		public void ShowForm(float posY = 0f)
		{
			mIsShow = true;
			if (mAnimator != null)
			{
				mAnimator.SetTrigger("In");
			}
			m_groupProductList.m_rectTransform.SetLocalPositionY(posY);
		}

		public void HideForm()
		{
			if (mAnimator != null)
			{
				mAnimator.SetTrigger("Out");
			}
			mIsShow = false;
		}

		public void NormalForm()
		{
			mIsShow = false;
		}
	}
}
