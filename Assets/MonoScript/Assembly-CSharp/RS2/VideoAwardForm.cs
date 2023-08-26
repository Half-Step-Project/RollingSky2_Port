using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class VideoAwardForm : UGUIForm
	{
		private const int VideoAwardNoAds = 1;

		public const int ShowRateThreshold = 5;

		public Text multi1;

		public GameObject closeButton;

		public Button aDsButton;

		public Image awardImg;

		public SetUIGrey setUIGrey;

		private bool m_release;

		private List<object> loadedAsserts = new List<object>();

		private double awardCount;

		protected override bool EnableInputAfterOpen()
		{
			return true;
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			base.gameObject.SetActive(true);
			m_release = false;
			int num = 0;
			if (userData != null)
			{
				num = (int)userData;
			}
			if (num > 0)
			{
				closeButton.SetActive(false);
				Invoke("ShowCloseButton", num);
			}
			VideoAward_videoAwardTable videoAwardTable = PlayerDataModule.Instance.PlayerGiftPackageData.VideoAwardTable;
			Goods_goodsTable goodsTable = Mod.DataTable.Get<Goods_goodsTable>().Get(videoAwardTable.GoodsId);
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(goodsTable.IconId.ToString()), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (m_release)
				{
					Mod.Resource.UnloadAsset(asset);
				}
				else
				{
					if (awardImg != null)
					{
						awardImg.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					}
					loadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", goodsTable.IconId.ToString(), assetName, errorMessage));
			}));
			awardCount = videoAwardTable.Count;
			if (videoAwardTable.GoodsId == 3)
			{
				awardCount *= PlayerDataModule.Instance.GetOffLineProductionGoldByTime(60000L);
			}
			else if (videoAwardTable.GoodsId == GameCommon.REPUTATION_ID)
			{
				awardCount *= PlayerDataModule.Instance.ComputerProductReputationByTime(60);
			}
			if (multi1 != null)
			{
				multi1.text = "X" + MonoSingleton<GameTools>.Instacne.DoubleToFormatString(awardCount);
			}
			InvokeRepeating("CheckAds", 0f, GameCommon.COMMON_AD_REFRESHTIME);
			InfocUtils.Report_rollingsky2_games_pageshow(13, 0, 1);
		}

		private void CheckAds()
		{
			bool flag = MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.MainView);
			aDsButton.interactable = flag;
			setUIGrey.SetGrey(!flag);
		}

		private void ShowCloseButton()
		{
			closeButton.SetActive(true);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			m_release = true;
			CancelInvoke();
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
		}

		public void OnClickAdsButton()
		{
			DoClickAdsButton();
		}

		private void DoClickAdsButton()
		{
			InfocUtils.Report_rollingsky2_games_ads(7, 0, 1, 0, 3, 0);
			MonoSingleton<GameTools>.Instacne.PlayVideoAdAndDisableInput(ADScene.MainView, delegate(ADScene adScen)
			{
				OnAdSuccess(adScen);
				MonoSingleton<GameTools>.Instacne.EnableInput();
				InfocUtils.Report_rollingsky2_games_ads(7, 0, 1, 0, 4, 0);
			});
		}

		private void OnAdSuccess(ADScene adScen = ADScene.NONE)
		{
			VideoAward_videoAwardTable videoAwardTable = PlayerDataModule.Instance.PlayerGiftPackageData.VideoAwardTable;
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.GoodsId = videoAwardTable.GoodsId;
			getGoodsData.GoodsNum = awardCount;
			getGoodsData.NeedMoveEffect = true;
			getGoodsData.moveEffectFinishedCallback = delegate
			{
				PlayerDataModule.Instance.ChangePlayerGoodsNum(videoAwardTable.GoodsId, awardCount, AssertChangeType.AD);
			};
			Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
			PlayerDataModule.Instance.PlayerLocalVideoAwardData.GetAward();
			Mod.Event.Fire(this, Mod.Reference.Acquire<GetVideoAwardEvent>());
			PlayerDataModule.Instance.PlayerGiftPackageData.VideoAwardTableId = -1;
			InfocUtils.Report_rollingsky2_games_currency(videoAwardTable.GoodsId, 1, 13, 0, (int)awardCount);
		}

		public void OnClickNoAdsButton()
		{
			PlayerDataModule.Instance.PlayerLocalVideoAwardData.GetAward();
			Mod.Event.Fire(this, Mod.Reference.Acquire<GetVideoAwardEvent>());
			PlayerDataModule.Instance.PlayerGiftPackageData.VideoAwardTableId = -1;
		}
	}
}
