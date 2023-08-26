using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using My.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class ScreenPluginsForm : UGUIForm
	{
		private enum PluginAdState
		{
			NONE = -1,
			BEFORE,
			PREPARED,
			STARTED,
			AFTER
		}

		public GameObject m_back;

		public GameObject m_BeforeAdEffect;

		public Image m_BeforeImage;

		public Text m_BeforeText;

		public GameObject m_AfterAdEffect;

		public Image m_AfterImage;

		public Text m_AfterText;

		public GameObject m_RemoveAdBtn;

		public Text m_removeAdPriceTxt;

		public Text m_debugInfo;

		public Animator m_AfterAnimator;

		private int m_currentPluginId = -1;

		private PluginAdData m_PluginAdData;

		private AssetLoadCallbacks m_BeforeImageAssetLoadCallBack;

		private AssetLoadCallbacks m_AfterImageAssetLoadCallBack;

		private List<object> loadedAsserts = new List<object>();

		private bool m_isRelease;

		private const int REMOVE_AD_SHOPID = 1009;

		private float m_autoPlayTime = 5f;

		private float m_autoStopTime = 5f;

		private const int ENTER_DIFFUICTY_LEVEL_ID = 11;

		private const float ad_error_deal_time = 2f;

		private PluginAdState m_currentState = PluginAdState.NONE;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			base.gameObject.SetActive(true);
			m_BeforeImage.gameObject.SetActive(false);
			m_AfterImage.gameObject.SetActive(false);
			m_isRelease = false;
			m_BeforeImageAssetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (m_isRelease)
				{
					Mod.Resource.UnloadAsset(asset);
				}
				else
				{
					m_BeforeImage.gameObject.SetActive(true);
					m_BeforeImage.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					loadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			});
			m_AfterImageAssetLoadCallBack = new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				if (m_isRelease)
				{
					Mod.Resource.UnloadAsset(asset);
				}
				else
				{
					m_AfterImage.gameObject.SetActive(true);
					m_AfterImage.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
					loadedAsserts.Add(asset);
				}
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			});
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			base.gameObject.SetActive(true);
			m_PluginAdData = userData as PluginAdData;
			if (m_PluginAdData == null)
			{
				CloseForm(false);
				return;
			}
			m_BeforeAdEffect.SetActive(false);
			m_AfterAdEffect.SetActive(false);
			m_currentPluginId = m_PluginAdData.PluginId;
			m_currentState = PluginAdState.BEFORE;
			SetShowContentByState();
			ShowCommonContent();
			AddEventHandler();
			StartCoroutine(AutoPlayAd(m_autoPlayTime));
			InfocUtils.Report_rollingsky2_games_pageshow(14, 0, 1);
		}

		private void SetShowContentByState()
		{
			switch (m_currentState)
			{
			case PluginAdState.BEFORE:
				if (m_BeforeAdEffect != null)
				{
					m_BeforeAdEffect.SetActive(true);
				}
				if (m_AfterAdEffect != null)
				{
					m_AfterAdEffect.SetActive(false);
				}
				break;
			case PluginAdState.AFTER:
				if (m_BeforeAdEffect != null)
				{
					m_BeforeAdEffect.SetActive(false);
				}
				if (m_AfterAdEffect != null)
				{
					m_AfterAdEffect.SetActive(true);
					m_AfterAnimator.Play("afterEffect_in", 0, 0f);
				}
				break;
			case PluginAdState.PREPARED:
			case PluginAdState.STARTED:
				break;
			}
		}

		private void ResetAnimator()
		{
			m_AfterAnimator.Play("afterEffect_in", 0, 0f);
			m_AfterAnimator.Update(0f);
		}

		private void ShowCommonContent()
		{
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[m_currentPluginId];
			if (screenPluginsAd_table != null)
			{
				m_autoPlayTime = (float)screenPluginsAd_table.AutoPlayAdTime * 0.001f;
				m_autoStopTime = (float)screenPluginsAd_table.AutoCloseAdTime * 0.001f;
				m_BeforeText.text = Mod.Localization.GetInfoById(screenPluginsAd_table.BeforeText);
				m_AfterText.text = Mod.Localization.GetInfoById(screenPluginsAd_table.AfterText);
				int beforeImage = screenPluginsAd_table.BeforeImage;
				string assetName = string.Format("pluginAd_{0}", beforeImage);
				Mod.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(assetName), m_BeforeImageAssetLoadCallBack);
				beforeImage = screenPluginsAd_table.AfterImage;
				assetName = string.Format("pluginAd_{0}", beforeImage);
				Mod.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(assetName), m_AfterImageAssetLoadCallBack);
				m_debugInfo.gameObject.SetActive(false);
			}
			if (Mod.DataTable.Get<Shops_shopTable>()[1009] != null)
			{
				m_removeAdPriceTxt.text = MonoSingleton<GameTools>.Instacne.GetProductRealPrice(1009);
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			m_currentState = PluginAdState.NONE;
			if (m_PluginAdData.EndHandler != null)
			{
				m_PluginAdData.EndHandler();
			}
			RemoveEventHandler();
			PlayerDataModule.Instance.PluginAdController.GoingCd();
			StopAllCoroutines();
			if (m_currentPluginId != 11)
			{
				Reward(m_currentPluginId);
			}
			else
			{
				PlayerDataModule.Instance.PluginAdController.DelayRewardPluginId = 11;
				PlayerDataModule.Instance.PluginAdController.DelayRewardFunc = Reward;
			}
			if (m_PluginAdData != null && !m_PluginAdData.Tutorial)
			{
				PlayerDataModule.Instance.PlayerRecordData.AddScreenPluginFormOpenTime();
			}
			m_currentPluginId = -1;
			m_PluginAdData = null;
			Mod.Event.Fire(this, UIPopUpFormCloseEvent.Make(UIFormId.ScreenPluginsForm));
		}

		private void AddEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_back);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(BackClickHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_RemoveAdBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RemoveAdHandler));
			Mod.Event.Subscribe(EventArgs<BuySuccessEventArgs>.EventId, BuySuccessHandler);
		}

		private void RemoveEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_back);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(BackClickHandler));
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(m_RemoveAdBtn);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(RemoveAdHandler));
			Mod.Event.Unsubscribe(EventArgs<BuySuccessEventArgs>.EventId, BuySuccessHandler);
		}

		private void BuySuccessHandler(object sender, Foundation.EventArgs e)
		{
			BuySuccessEventArgs buySuccessEventArgs = e as BuySuccessEventArgs;
			if (buySuccessEventArgs != null)
			{
				int shopItemId = buySuccessEventArgs.ShopItemId;
				Shops_shopTable shops_shopTable = Mod.DataTable.Get<Shops_shopTable>()[shopItemId];
				if (shopItemId == 1009)
				{
					InfocUtils.Report_rollingsky2_games_pageshow(14, 14, 2);
				}
			}
		}

		private void RemoveAdHandler(GameObject go)
		{
			MonoSingleton<GameTools>.Instacne.CommonBuyOperate(1009);
		}

		private void BackClickHandler(GameObject go)
		{
			switch (m_currentState)
			{
			case PluginAdState.NONE:
				CloseForm(false);
				break;
			case PluginAdState.BEFORE:
				PlayPluginAd();
				break;
			case PluginAdState.AFTER:
				CloseForm();
				break;
			case PluginAdState.PREPARED:
			case PluginAdState.STARTED:
				break;
			}
		}

		private void PlayPluginAd()
		{
			m_currentState = PluginAdState.PREPARED;
			StartCoroutine(AdPlayErrorDeal());
			My.Core.Singleton<ADHelper>.Instance.ShowInterstitial(ADScene.NONE, delegate(ADCallbackEventArgs x)
			{
				if (Mod.UI.UIFormIsOpen(UIFormId.ScreenPluginsForm))
				{
					if (x.Status == ADStatus.InterstitialClosed)
					{
						OnAdSuccess();
					}
					else if (x.Status == ADStatus.InterstitialOpened)
					{
						OnAdStart();
					}
					else
					{
						OnAdFailed();
					}
				}
			});
		}

		private IEnumerator AdPlayErrorDeal()
		{
			yield return new WaitForSeconds(2f);
			if (m_currentState == PluginAdState.PREPARED)
			{
				OnAdSuccess();
			}
		}

		private IEnumerator AutoStopAd(float time)
		{
			yield return new WaitForSeconds(time);
			CloseForm();
		}

		private IEnumerator AutoPlayAd(float time)
		{
			yield return new WaitForSeconds(time);
			if (m_currentState == PluginAdState.BEFORE)
			{
				PlayPluginAd();
			}
		}

		private void OnAdSuccess()
		{
			m_currentState = PluginAdState.AFTER;
			if (Mod.Procedure.Current is MenuProcedure)
			{
				MonoSingleton<GameTools>.Instacne.AdResumeMusic();
			}
			SetShowContentByState();
			if (base.gameObject.activeSelf)
			{
				StartCoroutine(AutoStopAd(m_autoStopTime));
			}
			Mod.Event.FireNow(this, Mod.Reference.Acquire<AdPlayEventArgs>().Initialize(1, 0));
			InfocUtils.Report_rollingsky2_games_ads(8, m_currentPluginId % 1000, 1, 0, 4, 0);
		}

		private void OnAdStart()
		{
			m_currentState = PluginAdState.STARTED;
			if (Mod.Procedure.Current is MenuProcedure)
			{
				MonoSingleton<GameTools>.Instacne.AdPauseMusic();
			}
			InfocUtils.Report_rollingsky2_games_ads(8, m_currentPluginId % 1000, 1, 0, 3, 0);
		}

		private void OnAdFailed()
		{
			m_currentState = PluginAdState.AFTER;
			if (Mod.Procedure.Current is MenuProcedure)
			{
				MonoSingleton<GameTools>.Instacne.AdResumeMusic();
			}
			StartCoroutine(AutoStopAd(m_autoStopTime));
		}

		private void CloseForm(bool playOutAnimaion = true)
		{
			if (!playOutAnimaion)
			{
				Mod.UI.CloseUIForm(UIFormId.ScreenPluginsForm);
				return;
			}
			m_AfterAnimator.Play("afterEffect_out", 0, 0f);
			StartCoroutine(DelayCloseForm(0.7f));
		}

		private IEnumerator DelayCloseForm(float deltaTime)
		{
			yield return new WaitForSeconds(deltaTime);
			ResetAnimator();
			Mod.UI.CloseUIForm(UIFormId.ScreenPluginsForm);
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
			m_isRelease = true;
		}

		private void Reward(int currentPluginID)
		{
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[currentPluginID];
			if (screenPluginsAd_table != null)
			{
				int num = 0;
				if (PlayerDataModule.Instance.PlayerIsHadSpecialStarAbility(7))
				{
					int starLevelAbilityNum = PlayerDataModule.Instance.GetStarLevelAbilityNum(7);
					num += starLevelAbilityNum;
				}
				Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(screenPluginsAd_table.AwardId).GetEnumerator();
				int num2 = -1;
				while (enumerator.MoveNext())
				{
					num2 = enumerator.Current.Key;
					int value = enumerator.Current.Value;
				}
				if (num2 > 0 && num > 0)
				{
					PlayerDataModule.Instance.ChangePlayerGoodsNum(enumerator.Current.Key, num, AssertChangeType.AD);
					BroadCastData broadCastData = new BroadCastData();
					broadCastData.GoodId = num2;
					broadCastData.Type = BroadCastType.GOODS;
					broadCastData.Info = string.Format("X{0}", num);
					MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
				}
			}
		}
	}
}
