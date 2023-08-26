using System.Collections.Generic;
using Foundation;
using My.Core;
using UnityEngine.Events;

namespace RS2
{
	public class ScreenPluginAdController
	{
		private long m_protectedEndTimeStamp;

		private bool m_isCdEnd;

		private float m_cdGoingTime;

		private bool m_updateCd;

		private uint timerId;

		private UnityAction<int> m_DelayRewardFunc;

		private bool m_isInited;

		private int m_DelayRewardPluginId = -1;

		private List<UIFormId> m_CommonBuyRecord = new List<UIFormId>();

		public int DelayRewardPluginId
		{
			get
			{
				return m_DelayRewardPluginId;
			}
			set
			{
				m_DelayRewardPluginId = value;
			}
		}

		public UnityAction<int> DelayRewardFunc
		{
			get
			{
				return m_DelayRewardFunc;
			}
			set
			{
				m_DelayRewardFunc = value;
			}
		}

		public List<UIFormId> CommonBuyRecord
		{
			get
			{
				return m_CommonBuyRecord;
			}
			set
			{
				m_CommonBuyRecord = value;
			}
		}

		public void Init()
		{
			if (!m_isInited)
			{
				m_isInited = true;
				if ((int)PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.REMOVE_AD) <= 0)
				{
					timerId = TimerHeap.AddTimer(0u, 100u, TimerHandler);
					Mod.Event.Subscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
					Mod.Event.Subscribe(EventArgs<BuySuccessEventArgs>.EventId, BuySuccessHandler);
					Mod.Event.Subscribe(EventArgs<AdPlayEventArgs>.EventId, PlayAdEventHandler);
					SetProtectTime(GameCommon.screenPluginAdCDTime * 1000);
					GoingCd();
				}
			}
		}

		private void SetProtectTime(int protectTime)
		{
			m_protectedEndTimeStamp = PlayerDataModule.Instance.ServerTime + protectTime;
		}

		private bool IsInProtectTimeStage()
		{
			return m_protectedEndTimeStamp > PlayerDataModule.Instance.ServerTime;
		}

		private bool IsInRemoveAdBufferStage()
		{
			return PlayerDataModule.Instance.BufferIsEnable(GameCommon.REMOVE_AD_TIME);
		}

		private void TimerHandler()
		{
			if (m_updateCd)
			{
				m_cdGoingTime -= 0.1f;
				if (m_cdGoingTime <= 0f)
				{
					m_updateCd = false;
					m_isCdEnd = true;
				}
			}
		}

		public void GoingCd()
		{
			m_cdGoingTime = GameCommon.screenPluginAdCDTime;
			m_updateCd = true;
			m_isCdEnd = false;
		}

		public bool IsScreenAdRead()
		{
			if (GameCommon.isOpenScreenPlugin < 1)
			{
				return false;
			}
			if ((int)PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.REMOVE_AD) > 0)
			{
				return false;
			}
			if (!m_isCdEnd)
			{
				return false;
			}
			if (PlayerDataModule.Instance.IsInNewPlayerProtectedStage())
			{
				return false;
			}
			if (IsInProtectTimeStage())
			{
				return false;
			}
			if (IsInRemoveAdBufferStage())
			{
				return false;
			}
			return PluginAdNativeRead();
		}

		public bool IsToturialScreenRead()
		{
			if ((int)PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.REMOVE_AD) > 0)
			{
				return false;
			}
			return PluginAdNativeRead();
		}

		private bool PluginAdNativeRead()
		{
			InfocUtils.ReportInterstitialAdShowChance();
			bool num = My.Core.Singleton<ADHelper>.Instance.InterstitialCanShow(ADScene.NONE);
			if (!num)
			{
				My.Core.Singleton<ADHelper>.Instance.RequestInterstitial();
			}
			return num;
		}

		private void OnPlayerAssetChange(object sender, EventArgs e)
		{
			GameGoodsNumChangeEventArgs gameGoodsNumChangeEventArgs = e as GameGoodsNumChangeEventArgs;
			if (gameGoodsNumChangeEventArgs == null)
			{
				return;
			}
			if (gameGoodsNumChangeEventArgs.GoodsId == GameCommon.REMOVE_AD)
			{
				if (PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.REMOVE_AD) > 0.0)
				{
					Clear();
				}
			}
			else if (gameGoodsNumChangeEventArgs.GoodsId == 2 && gameGoodsNumChangeEventArgs.ChangeNum < 0.0)
			{
				SetProtectTime(GameCommon.screenPluginAdProtectedTime * 1000);
			}
			else if (gameGoodsNumChangeEventArgs.GoodsId == 6 && gameGoodsNumChangeEventArgs.ChangeNum < 0.0)
			{
				SetProtectTime(GameCommon.screenPluginAdProtectedTime * 1000);
			}
		}

		private void BuySuccessHandler(object sender, EventArgs e)
		{
			BuySuccessEventArgs buySuccessEventArgs = e as BuySuccessEventArgs;
			if (buySuccessEventArgs != null)
			{
				int shopItemId = buySuccessEventArgs.ShopItemId;
				if (Mod.DataTable.Get<Shops_shopTable>()[shopItemId] != null)
				{
					SetProtectTime(GameCommon.screenPluginAdProtectedTime * 1000);
				}
			}
		}

		private void PlayAdEventHandler(object sender, EventArgs args)
		{
			AdPlayEventArgs adPlayEventArgs = args as AdPlayEventArgs;
			if (adPlayEventArgs != null && adPlayEventArgs.AdState == 1)
			{
				GoingCd();
			}
		}

		public void Clear()
		{
			TimerHeap.DelTimer(timerId);
			Mod.Event.Unsubscribe(EventArgs<GameGoodsNumChangeEventArgs>.EventId, OnPlayerAssetChange);
		}

		public void ClearDelayRewardOperate()
		{
			m_DelayRewardPluginId = -1;
			m_DelayRewardFunc = null;
		}
	}
}
