using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class CoolPlayGiftForm : UGUIForm
	{
		public GameObject m_NoramlContainer;

		public GameObject m_closeButton;

		public GameObject m_BuyContainer;

		public GameObject m_InfoContainer;

		public Text m_InfoText;

		public Text m_normalPriceTxt;

		public GoodsItemController m_NormalItemController;

		public RectTransform m_normalItemContainer;

		public GameObject m_AlertContainer;

		public Text m_alertPrceTx;

		public GoodsItemController m_AlertItemController;

		public RectTransform m_AlertItemContainer;

		private List<GoodsItemController> m_NormalGoodsList = new List<GoodsItemController>();

		private List<GoodsItemController> m_AlertGoodsList = new List<GoodsItemController>();

		private MonoTimer m_timer;

		private CoolPlayData m_Data;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			base.gameObject.SetActive(true);
			m_Data = userData as CoolPlayData;
			if (m_Data == null)
			{
				Mod.UI.CloseUIForm(UIFormId.CoolPlayGiftForm);
				return;
			}
			Mod.Event.Subscribe(EventArgs<BuySuccessEventArgs>.EventId, BuySuccessHandler);
			m_closeButton.SetActive(false);
			Invoke("ShowCloseButton", 1.5f);
			m_AlertContainer.SetActive(false);
			m_NoramlContainer.SetActive(true);
			if (m_Data.Type == CoolPlayData.OpenType.BUY)
			{
				m_BuyContainer.SetActive(true);
				m_InfoContainer.SetActive(false);
			}
			else if (m_Data.Type == CoolPlayData.OpenType.INFO)
			{
				m_BuyContainer.SetActive(false);
				m_InfoContainer.SetActive(true);
				int num = (int)((float)PlayerDataModule.Instance.CoolPlayPagageData.LeftTime() * 0.001f);
				m_InfoText.text = MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(num);
				m_timer = new MonoTimer(1f, true);
				m_timer.Elapsed += delegate
				{
					int num2 = (int)((float)PlayerDataModule.Instance.CoolPlayPagageData.LeftTime() * 0.001f);
					if (num2 <= 0)
					{
						m_timer.Stop();
					}
					else
					{
						m_InfoText.text = MonoSingleton<GameTools>.Instacne.TimeFormat_HH_MM_SS(num2);
					}
				};
				m_timer.FireElapsedOnStop = false;
				m_timer.Start();
			}
			m_normalPriceTxt.text = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(m_Data.ShopId);
			m_alertPrceTx.text = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(m_Data.ShopId);
			InitContent();
		}

		private void InitContent()
		{
			GameObject gameObject = null;
			GoodsItemController goodsItemController = null;
			m_NormalGoodsList.Clear();
			int goodsTeamid = Mod.DataTable.Get<Shops_shopTable>()[m_Data.ShopId].GoodsTeamid;
			Dictionary<int, int> dictionary = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(goodsTeamid);
			Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator();
			int i = 0;
			for (int childCount = m_normalItemContainer.transform.childCount; i < childCount; i++)
			{
				Object.Destroy(m_normalItemContainer.transform.GetChild(i).gameObject);
			}
			while (enumerator.MoveNext())
			{
				gameObject = Object.Instantiate(m_NormalItemController.gameObject);
				goodsItemController = gameObject.GetComponent<GoodsItemController>();
				gameObject.SetActive(true);
				m_NormalGoodsList.Add(goodsItemController);
				gameObject.transform.SetParent(m_normalItemContainer.transform);
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
				gameObject.transform.localScale = Vector3.one;
				goodsItemController.Init();
				goodsItemController.SetGoodsId(enumerator.Current.Key, 0, MonoSingleton<GameTools>.Instacne.GetGoodsDesc(enumerator.Current.Key), false);
			}
			int j = 0;
			for (int childCount2 = m_AlertItemContainer.transform.childCount; j < childCount2; j++)
			{
				Object.Destroy(m_AlertItemContainer.transform.GetChild(j).gameObject);
			}
			enumerator = dictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				gameObject = Object.Instantiate(m_AlertItemController.gameObject);
				goodsItemController = gameObject.GetComponent<GoodsItemController>();
				gameObject.SetActive(true);
				m_AlertGoodsList.Add(goodsItemController);
				gameObject.transform.SetParent(m_AlertItemContainer.transform);
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
				gameObject.transform.localScale = Vector3.one;
				goodsItemController.Init();
				goodsItemController.SetGoodsId(enumerator.Current.Key, 0, "", true);
			}
		}

		private void ShowCloseButton()
		{
			m_closeButton.SetActive(true);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			Mod.Event.Unsubscribe(EventArgs<BuySuccessEventArgs>.EventId, BuySuccessHandler);
			PlayerDataModule.Instance.CoolPlayPagageData.Record();
			if (m_Data.CallBack != null)
			{
				m_Data.CallBack();
			}
			if (m_timer != null)
			{
				m_timer.Stop();
			}
		}

		private void BuySuccessHandler(object sender, EventArgs e)
		{
			if (e is BuySuccessEventArgs)
			{
				Mod.UI.CloseUIForm(UIFormId.CoolPlayGiftForm);
			}
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < m_AlertGoodsList.Count; i++)
			{
				m_AlertGoodsList[i].Release();
			}
			for (int j = 0; j < m_NormalGoodsList.Count; j++)
			{
				m_NormalGoodsList[j].Release();
			}
		}

		public void OnQutiBtnClickHandler()
		{
			Mod.UI.CloseUIForm(UIFormId.CoolPlayGiftForm);
		}

		public void OnBuyClickHandler()
		{
			MonoSingleton<GameTools>.Instacne.CommonBuyOperate(m_Data.ShopId);
		}

		public void OnBuyCloseBtnClickHandler()
		{
			if (m_Data.Type == CoolPlayData.OpenType.BUY)
			{
				m_AlertContainer.SetActive(true);
				m_NoramlContainer.SetActive(false);
			}
		}

		public void OnInfoCloseBtnClickHandler()
		{
			Mod.UI.CloseUIForm(UIFormId.CoolPlayGiftForm);
		}

		public void AlertCloseHandler()
		{
			if (m_Data.Type == CoolPlayData.OpenType.BUY)
			{
				m_AlertContainer.SetActive(false);
				m_NoramlContainer.SetActive(true);
			}
		}
	}
}
