using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class CommonAlertItemAdController : CommonAlertItemController
{
	public CommonAlertData.AlertShopType m_showType = CommonAlertData.AlertShopType.AD;

	private CommonAlertData m_AlertData;

	public GameObject adBtn;

	public Text rewardContentTxt;

	public Image rewardIcon;

	public Text adTxt;

	public SetUIGrey m_adGray;

	private List<object> loadedAsserts = new List<object>();

	private uint adTimerId;

	public override void Init(CommonAlertData alertData)
	{
		AddEventHandler();
		m_AlertData = alertData;
		rewardIcon.gameObject.SetActive(false);
		adTxt.text = alertData.adContent;
		rewardContentTxt.text = alertData.alertContent;
		if (alertData.iconid > 0)
		{
			string rewardspriteName = alertData.iconid.ToString();
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(rewardspriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				rewardIcon.gameObject.SetActive(true);
				rewardIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				StartCoroutine(DynamicLayout());
				loadedAsserts.Add(asset);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", rewardspriteName, assetName, errorMessage));
			}));
		}
		SetAdButtonState();
		adTimerId = TimerHeap.AddTimer((uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), (uint)(GameCommon.COMMON_AD_REFRESHTIME * 1000f), SetAdButtonState);
	}

	private void AddEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(adBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(AdBtnClickHandler));
	}

	private void RemoveEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(adBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(AdBtnClickHandler));
	}

	private void AdBtnClickHandler(GameObject obj)
	{
		if (!MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE) || m_AlertData == null)
		{
			return;
		}
		int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
		string info = string.Format("scenes=5&scenes_source=0&ad_source=1&action=3&source=0&level={0}", curLevelId);
		InfocUtils.ReportInfoc(InfocTable.rollingsky2_games_ads, info);
		MonoSingleton<GameTools>.Instacne.PlayerVideoAd(m_AlertData.adScene, delegate
		{
			m_AlertData.callBackFunc();
		}, null, null, delegate
		{
			if (m_AlertData.startADCallBackFunc != null)
			{
				m_AlertData.startADCallBackFunc();
			}
		});
	}

	private IEnumerator DynamicLayout()
	{
		yield return new WaitForEndOfFrame();
		rewardIcon.rectTransform.localPosition = new Vector2((rewardContentTxt.rectTransform.rect.width + rewardIcon.rectTransform.rect.width) * 0.5f, 0f);
		yield return null;
	}

	public override void Release()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
		TimerHeap.DelTimer(adTimerId);
	}

	public override void Reset()
	{
		RemoveEventHandler();
	}

	public override CommonAlertData.AlertShopType GetAlertType()
	{
		return m_showType;
	}

	private void SetAdButtonState()
	{
		if (MonoSingleton<GameTools>.Instacne.CanShowAd(ADScene.NONE))
		{
			m_adGray.SetGrey(false);
			TimerHeap.DelTimer(adTimerId);
			adTxt.text = m_AlertData.adContent;
		}
		else
		{
			m_adGray.SetGrey(true);
			adTxt.text = Mod.Localization.GetInfoById(211);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(adTxt.transform.parent as RectTransform);
	}
}
