using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class CommonAlertItemLockController : CommonAlertItemController
{
	public CommonAlertData.AlertShopType m_showType = CommonAlertData.AlertShopType.UNLOCK;

	private CommonAlertData m_AlertData;

	public Text lockContentTxt;

	public Text lockPriceTxt;

	public Image lockMoneyIcon;

	public GameObject m_unLockBtn;

	public Text m_okTxt;

	private List<object> loadedAsserts = new List<object>();

	public override void Init(CommonAlertData alertData)
	{
		AddEventHandler();
		m_AlertData = alertData;
		m_okTxt.text = (string.IsNullOrEmpty(alertData.lableContent) ? "Ok" : alertData.lableContent);
		if (alertData.iconid > 0)
		{
			lockPriceTxt.text = string.Format(Mod.Localization.GetInfoById(69), alertData.goodsNum);
			lockContentTxt.text = string.Format(Mod.Localization.GetInfoById(68));
			string moneyspriteName = alertData.iconid.ToString();
			Mod.Resource.LoadAsset(AssetUtility.GetGameIconAsset(moneyspriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				lockMoneyIcon.gameObject.SetActive(true);
				lockMoneyIcon.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				loadedAsserts.Add(asset);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", moneyspriteName, assetName, errorMessage));
			}));
		}
	}

	private void AddEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_unLockBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OkBtnClickHandler));
	}

	private void RemoveEventHandler()
	{
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_unLockBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OkBtnClickHandler));
	}

	private void OkBtnClickHandler(GameObject obj)
	{
		if (m_AlertData != null)
		{
			m_AlertData.callBackFunc();
		}
	}

	private IEnumerator DynamicLayout()
	{
		yield return new WaitForEndOfFrame();
		lockMoneyIcon.rectTransform.localPosition = new Vector2(lockPriceTxt.rectTransform.localPosition.x + (lockPriceTxt.rectTransform.rect.width + lockMoneyIcon.rectTransform.rect.width) * 0.5f, 0f);
		lockContentTxt.rectTransform.localPosition = new Vector2(lockMoneyIcon.rectTransform.localPosition.x + lockMoneyIcon.rectTransform.rect.width * 0.5f, 0f);
		yield return null;
	}

	public override void Release()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}

	public override void Reset()
	{
		RemoveEventHandler();
	}

	public override CommonAlertData.AlertShopType GetAlertType()
	{
		return m_showType;
	}
}
