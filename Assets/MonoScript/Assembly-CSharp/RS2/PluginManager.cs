using System;
using Foundation;
using My.Core;
using Umeng;
using UnityEngine;

namespace RS2
{
	public class PluginManager : MonoSingleton<PluginManager>
	{
		private ProductInfo productInfo;

		public void InitThird()
		{
		}

		private void InitAppsflyer(bool gdpr)
		{
			AppsFlyer.setAppsFlyerKey("JS4D7K6orQauKANLStD2nb");
			MonoSingleton<GameTools>.Instacne.AFReportRetention();
		}

		private void InitUmeng(bool gdpr)
		{
			if (!TempShieldedUmeng())
			{
				string appkey = "5c63b809f1f5560e82000661";
				string channelId = "unknow";
				if (gdpr)
				{
					UMCommon.Init(appkey, channelId);
					Analytics.Start();
				}
				else
				{
					CMPGDPRUtils.Instance.OnGDPRCallback += HandleGDPRCallback;
				}
			}
		}

		private void HandleGDPRCallback(object sender, System.EventArgs e)
		{
			if (((CMPGDPRUtils.GDPREventArgs)e).Agreed)
			{
				Log.Info("restart umeng");
				InitUmeng(true);
			}
		}

		private void InitCloudConfig()
		{
			string cloudLanguage = MonoSingleton<GameTools>.Instacne.GetCloudLanguage(Mod.Localization.Language);
			My.Core.Singleton<CloudConfigHelper>.Instance.PullCloudConfigDataWithLanguage(cloudLanguage);
		}

		private void InitAd()
		{
			My.Core.Singleton<ADHelper>.Instance.Initialize();
			My.Core.Singleton<ADHelper>.Instance.RequestRewardeVideo();
			My.Core.Singleton<ADHelper>.Instance.RequestInterstitial();
		}

		public void InitShop()
		{
			My.Core.Singleton<IAPHelper>.Instance.Initialize();
			InitIProductInfo();
		}

		private void InitIProductInfo()
		{
			string config = EncodeConfig.getConfig(PlayerLocalDatakey.GAMESHOPPRICEDATA);
			if (!string.IsNullOrEmpty(config))
			{
				productInfo = JsonUtility.FromJson<ProductInfo>(config);
				if (productInfo == null)
				{
					return;
				}
				Shops_shopTable[] records = Mod.DataTable.Get<Shops_shopTable>().Records;
				int num = 0;
				for (int i = 0; i < records.Length; i++)
				{
					if (records[i].BuyType == 1)
					{
						num++;
					}
				}
				if (num != productInfo.productinInfoList.Count)
				{
					My.Core.Singleton<IAPHelper>.Instance.RefreshProductInfo();
				}
			}
			else
			{
				My.Core.Singleton<IAPHelper>.Instance.RefreshProductInfo();
			}
		}

		public void RefreshProductionInfo()
		{
			string config = EncodeConfig.getConfig(PlayerLocalDatakey.GAMESHOPPRICEDATA);
			if (!string.IsNullOrEmpty(config))
			{
				productInfo = JsonUtility.FromJson<ProductInfo>(config);
			}
		}

		public ProductInfo GetProductInfo()
		{
			return productInfo;
		}

		private bool TempShieldedUmeng()
		{
			return true;
		}
	}
}
