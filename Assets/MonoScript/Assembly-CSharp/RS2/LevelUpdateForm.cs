using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class LevelUpdateForm : UGUIForm
	{
		private LevelUpdateData m_updateData;

		[SerializeField]
		private GameObject m_closeBtn;

		[SerializeField]
		private Text m_freeTex;

		[SerializeField]
		private Text m_lockInfoTex;

		[SerializeField]
		private Text m_downloadInfo;

		[SerializeField]
		private Text m_percentageInforTex;

		[SerializeField]
		private Image m_progressFore;

		private string m_downloadingSizeFormat;

		private bool m_isLevelLocked;

		private Dictionary<string, int> m_downloadLengthDic = new Dictionary<string, int>();

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			m_freeTex.text = Mod.Localization.GetInfoById(118);
			m_lockInfoTex.text = Mod.Localization.GetInfoById(119);
			m_downloadInfo.text = Mod.Localization.GetInfoById(83);
			m_downloadingSizeFormat = Mod.Localization.GetInfoById(120);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			m_updateData = (LevelUpdateData)userData;
			m_progressFore.fillAmount = 0f;
			m_isLevelLocked = true;
			PlayerLocalLevelData playerLevelData = PlayerDataModule.Instance.GetPlayerLevelData(m_updateData.Level);
			if (playerLevelData != null)
			{
				m_isLevelLocked = playerLevelData.LockState > 0;
			}
			if (!m_isLevelLocked)
			{
				m_freeTex.gameObject.SetActive(false);
				m_lockInfoTex.gameObject.SetActive(false);
			}
			m_downloadLengthDic.Clear();
			AddEventHandler();
		}

		protected override void OnClose(object userData)
		{
			Clear();
			RemoveEventHandler();
			base.OnClose(userData);
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			int num = 0;
			foreach (KeyValuePair<string, int> item in m_downloadLengthDic)
			{
				num += item.Value;
			}
			float fillAmount = (float)num / (float)m_updateData.UpdateTotalLength;
			m_progressFore.fillAmount = fillAmount;
			string arg = MonoSingleton<GameTools>.Instacne.CountSize(m_updateData.UpdateTotalLength);
			string arg2 = MonoSingleton<GameTools>.Instacne.CountSize(num);
			m_percentageInforTex.text = string.Format(m_downloadingSizeFormat, arg2, arg);
		}

		private void Clear()
		{
			m_downloadLengthDic.Clear();
		}

		private void AddEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseBtnClickHandler));
			Mod.Event.Subscribe(EventArgs<BundleUpdateStartEventArgs>.EventId, OnLevelUpdateStart);
			Mod.Event.Subscribe(EventArgs<BundleUpdateFailureEventArgs>.EventId, OnLevelUpdateFailure);
			Mod.Event.Subscribe(EventArgs<BundleUpdateChangedEventArgs>.EventId, OnLevelUpdateChanged);
			Mod.Event.Subscribe(EventArgs<BundleUpdateSuccessEventArgs>.EventId, OnLevelUpdateSuccess);
			Mod.Event.Subscribe(EventArgs<BunldeUpdateAllCompleteEventArgs>.EventId, OnLevelUpdateAllComplete);
		}

		private void RemoveEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseBtnClickHandler));
			Mod.Event.Unsubscribe(EventArgs<BundleUpdateStartEventArgs>.EventId, OnLevelUpdateStart);
			Mod.Event.Unsubscribe(EventArgs<BundleUpdateFailureEventArgs>.EventId, OnLevelUpdateFailure);
			Mod.Event.Unsubscribe(EventArgs<BundleUpdateChangedEventArgs>.EventId, OnLevelUpdateChanged);
			Mod.Event.Unsubscribe(EventArgs<BundleUpdateSuccessEventArgs>.EventId, OnLevelUpdateSuccess);
			Mod.Event.Unsubscribe(EventArgs<BunldeUpdateAllCompleteEventArgs>.EventId, OnLevelUpdateAllComplete);
		}

		private void CloseBtnClickHandler(GameObject obj)
		{
			CommonAlertData commonAlertData = new CommonAlertData();
			commonAlertData.showType = CommonAlertData.AlertShopType.COMMON;
			commonAlertData.alertContent = Mod.Localization.GetInfoById(84);
			commonAlertData.callBackFunc = delegate
			{
				Mod.Resource.StopUpdateLevelBundle();
				Mod.UI.CloseUIForm(UIFormId.LevelUpdateForm);
				Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
				Mod.Resource.LevelBundleCheck(m_updateData.Level);
				Clear();
			};
			Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
		}

		private void OnLevelUpdateStart(object sender, Foundation.EventArgs e)
		{
			BundleUpdateStartEventArgs bundleUpdateStartEventArgs = (BundleUpdateStartEventArgs)e;
			if (bundleUpdateStartEventArgs != null && !m_downloadLengthDic.ContainsKey(bundleUpdateStartEventArgs.Name))
			{
				m_downloadLengthDic.Add(bundleUpdateStartEventArgs.Name, 0);
			}
		}

		private void OnLevelUpdateFailure(object sender, Foundation.EventArgs e)
		{
			BundleUpdateFailureEventArgs bundleUpdateFailureEventArgs = (BundleUpdateFailureEventArgs)e;
			if (bundleUpdateFailureEventArgs != null)
			{
				Log.Error(string.Format("Level Assetbundle {0} Update Fail With Error Message {1}", bundleUpdateFailureEventArgs.Uri, bundleUpdateFailureEventArgs.Message));
				Mod.Resource.StopUpdateLevelBundle();
				CommonAlertData commonAlertData = new CommonAlertData();
				commonAlertData.showType = CommonAlertData.AlertShopType.COMMON;
				commonAlertData.alertContent = Mod.Localization.GetInfoById(105);
				commonAlertData.callBackFunc = delegate
				{
					Mod.UI.CloseUIForm(UIFormId.CommonAlertForm);
					Mod.UI.CloseUIForm(UIFormId.LevelUpdateForm);
				};
				Mod.UI.OpenUIForm(UIFormId.CommonAlertForm, commonAlertData);
				Mod.Resource.LevelBundleCheck(m_updateData.Level);
			}
		}

		private void OnLevelUpdateAllComplete(object sender, Foundation.EventArgs e)
		{
			Clear();
			Mod.UI.CloseUIForm(UIFormId.LevelUpdateForm);
		}

		private void OnLevelUpdateSuccess(object sender, Foundation.EventArgs e)
		{
			BundleUpdateSuccessEventArgs bundleUpdateSuccessEventArgs = (BundleUpdateSuccessEventArgs)e;
			if (bundleUpdateSuccessEventArgs != null)
			{
				m_downloadLengthDic[bundleUpdateSuccessEventArgs.Name] = bundleUpdateSuccessEventArgs.ZipLength;
			}
		}

		private void OnLevelUpdateChanged(object sender, Foundation.EventArgs e)
		{
			BundleUpdateChangedEventArgs bundleUpdateChangedEventArgs = (BundleUpdateChangedEventArgs)e;
			if (bundleUpdateChangedEventArgs == null)
			{
				return;
			}
			int value;
			if (m_downloadLengthDic.TryGetValue(bundleUpdateChangedEventArgs.Name, out value))
			{
				if (value < bundleUpdateChangedEventArgs.Length)
				{
					m_downloadLengthDic[bundleUpdateChangedEventArgs.Name] = bundleUpdateChangedEventArgs.Length;
				}
			}
			else
			{
				m_downloadLengthDic.Add(bundleUpdateChangedEventArgs.Name, bundleUpdateChangedEventArgs.Length);
			}
		}
	}
}
