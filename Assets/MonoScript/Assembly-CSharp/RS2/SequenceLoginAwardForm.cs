using System;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class SequenceLoginAwardForm : UGUIForm
	{
		public GameObject m_getBtn;

		public GameObject hadGetTips;

		public GameObject timeTipsRoot;

		public Text timeTips;

		public Button m_getAdBtn;

		public GameObject m_awardItem;

		public GameObject emptyItem;

		public Transform m_container;

		public SnapScrollView snapScrollView;

		public Text desc;

		public GameObject leftPage;

		public GameObject rightPage;

		private List<SequenceLoginAwardItem> m_AwardItemList;

		private const int LeaveFormPluginAdId = 3;

		private static int m_leaveFormTotalTime;

		private bool m_isGotGoods;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			InitUI();
		}

		public void InitUI()
		{
			SequenceLoginAward_table[] records = Mod.DataTable.Get<SequenceLoginAward_table>().Records;
			GameObject gameObject = null;
			if (m_AwardItemList == null)
			{
				m_AwardItemList = new List<SequenceLoginAwardItem>(records.Length);
			}
			UnityEngine.Object.Instantiate(emptyItem).transform.SetParent(m_container, false);
			for (int i = 0; i < records.Length; i++)
			{
				gameObject = UnityEngine.Object.Instantiate(m_awardItem);
				gameObject.name = i.ToString();
				gameObject.transform.SetParent(m_container);
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
				gameObject.transform.localScale = Vector3.one;
				m_AwardItemList.Add(gameObject.GetComponent<SequenceLoginAwardItem>());
				m_AwardItemList[i].Init();
				m_AwardItemList[i].SetData(records[i].Id);
			}
			UnityEngine.Object.Instantiate(emptyItem).transform.SetParent(m_container, false);
			m_awardItem.SetActive(false);
			snapScrollView.Init();
		}

		private IEnumerator CheckForNextDay()
		{
			do
			{
				yield return new WaitForSeconds(1f);
			}
			while (PlayerDataModule.Instance.SequenceLoginData.HadGetToday());
			int num = PlayerDataModule.Instance.SequenceLoginData.NowIndex();
			RefreshButton();
			RefreshDesc(num + 1);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			SnapScrollView obj = snapScrollView;
			obj.PageChangeFinished = (System.Action<int>)Delegate.Combine(obj.PageChangeFinished, new System.Action<int>(OnPageChangeFinished));
			SnapScrollView obj2 = snapScrollView;
			obj2.PageChangeStart = (System.Action)Delegate.Combine(obj2.PageChangeStart, new System.Action(OnPageChangeStart));
			AddEventListener();
			for (int i = 0; i < m_AwardItemList.Count; i++)
			{
				if (m_AwardItemList[i].CanGet())
				{
					InfocUtils.Report_rollingsky2_games_pageshow(6, 0, 1, 0, 0, i + 1);
					break;
				}
			}
			int num = PlayerDataModule.Instance.SequenceLoginData.NowIndex();
			snapScrollView.SetPage(num);
			RefreshDesc(num + 1);
			RefreshButton();
			UpdatePageButton();
			m_AwardItemList[num].ShowSelectGo(true);
			if (PlayerDataModule.Instance.SequenceLoginData.HadGetToday())
			{
				StartCoroutine(CheckForNextDay());
			}
			PlayerDataModule.Instance.PlayerRecordData.AddOpenSequenceLoginAwardFormCount();
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			SnapScrollView obj = snapScrollView;
			obj.PageChangeFinished = (System.Action<int>)Delegate.Remove(obj.PageChangeFinished, new System.Action<int>(OnPageChangeFinished));
			SnapScrollView obj2 = snapScrollView;
			obj2.PageChangeStart = (System.Action)Delegate.Remove(obj2.PageChangeStart, new System.Action(OnPageChangeStart));
			RemoveEventListener();
			CancelInvoke();
			StopAllCoroutines();
			DealLeaveformPluginAd();
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			foreach (SequenceLoginAwardItem awardItem in m_AwardItemList)
			{
				awardItem.Release();
			}
		}

		private void OnPageChangeFinished(int page)
		{
			RefreshDesc(page + 1);
			RefreshButton();
			UpdatePageButton();
			m_AwardItemList[page].ShowSelectGo(true);
		}

		private void OnPageChangeStart()
		{
			foreach (SequenceLoginAwardItem awardItem in m_AwardItemList)
			{
				awardItem.ShowSelectGo(false);
			}
		}

		private void RefreshDesc(int tableId)
		{
			SequenceLoginAward_table sequenceLoginAward_table = Mod.DataTable.Get<SequenceLoginAward_table>()[tableId];
			desc.text = MonoSingleton<GameTools>.Instacne.GetGoodsTeamDesc(sequenceLoginAward_table.GoodTeamIds);
		}

		private void RefreshButton()
		{
			CancelInvoke("RefreshTime");
			int currentPage = snapScrollView.CurrentPage;
			int num = PlayerDataModule.Instance.SequenceLoginData.NowIndex();
			switch (PlayerDataModule.Instance.SequenceLoginData.GetAwardRecord(currentPage))
			{
			case 0:
				if (currentPage < num)
				{
					Debug.LogError("有该领而未领取的登录奖励");
				}
				if (!PlayerDataModule.Instance.SequenceLoginData.HadGetToday() && currentPage == num)
				{
					m_getBtn.SetActive(true);
					m_getAdBtn.gameObject.SetActive(true);
					timeTipsRoot.SetActive(false);
				}
				else
				{
					m_getBtn.SetActive(false);
					m_getAdBtn.gameObject.SetActive(false);
					timeTipsRoot.SetActive(true);
				}
				if (num == currentPage)
				{
					InvokeRepeating("RefreshTime", 0f, 1f);
				}
				else
				{
					timeTips.text = string.Format(Mod.Localization.GetInfoById(142), currentPage - num + 1);
				}
				hadGetTips.SetActive(false);
				break;
			case 1:
				hadGetTips.SetActive(true);
				m_getBtn.SetActive(false);
				m_getAdBtn.gameObject.SetActive(false);
				timeTipsRoot.SetActive(false);
				break;
			}
		}

		private void ToShowGetButton()
		{
			m_getBtn.SetActive(true);
		}

		private void RefreshTime()
		{
			timeTips.text = MonoSingleton<GameTools>.Instacne.GetLeftTimeToday();
		}

		public void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_getBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(GetHandler));
		}

		public void GetAdBtnHandler()
		{
			InfocUtils.Report_rollingsky2_games_ads(13, 0, 1, 0, 3, 0);
			MonoSingleton<GameTools>.Instacne.PlayerVideoAd(ADScene.SequenceLogin, OnAdSuccess);
		}

		private void OnAdSuccess(ADScene adScen = ADScene.SequenceLogin)
		{
			GetGoods(3);
			Mod.UI.CloseUIForm(UIFormId.SequenceLoginAwardForm);
			InfocUtils.Report_rollingsky2_games_ads(13, 0, 1, 0, 4, 0);
		}

		public void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_getBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(GetHandler));
		}

		private void GetHandler(GameObject go)
		{
			GetGoods();
			Mod.UI.CloseUIForm(UIFormId.SequenceLoginAwardForm);
		}

		private void GetGoods(int facotr = 1)
		{
			for (int i = 0; i < m_AwardItemList.Count; i++)
			{
				if (m_AwardItemList[i].CanGet())
				{
					m_AwardItemList[i].GetGoods(facotr);
					break;
				}
			}
			m_isGotGoods = true;
		}

		private void UpdatePageButton()
		{
			if (snapScrollView.IsFirstPage())
			{
				leftPage.SetActive(false);
			}
			else
			{
				leftPage.SetActive(true);
			}
			if (snapScrollView.IsLastPage())
			{
				rightPage.SetActive(false);
			}
			else
			{
				rightPage.SetActive(true);
			}
		}

		public void OnClickClose()
		{
			if (!PlayerDataModule.Instance.SequenceLoginData.HadGetToday())
			{
				GetGoods();
			}
			Mod.UI.CloseUIForm(UIFormId.SequenceLoginAwardForm);
		}

		public void OnClickLeft()
		{
			snapScrollView.PrePage();
			UpdatePageButton();
		}

		public void OnClickRight()
		{
			snapScrollView.NextPage();
			UpdatePageButton();
		}

		private bool DealLeaveformPluginAd()
		{
			m_leaveFormTotalTime++;
			int id = MonoSingleton<GameTools>.Instacne.ComputerScreenPluginAdId(3);
			ScreenPluginsAd_table screenPluginsAd_table = Mod.DataTable.Get<ScreenPluginsAd_table>()[id];
			bool flag = false;
			if (screenPluginsAd_table != null)
			{
				flag |= m_leaveFormTotalTime >= screenPluginsAd_table.TriggerNum;
				flag |= !m_isGotGoods;
				if (flag)
				{
					flag &= PlayerDataModule.Instance.PluginAdController.IsScreenAdRead();
					if (flag)
					{
						PluginAdData pluginAdData = new PluginAdData();
						pluginAdData.PluginId = 3;
						pluginAdData.EndHandler = delegate
						{
							ClearLeaveFormPluginAdData();
						};
						Mod.UI.OpenUIForm(UIFormId.ScreenPluginsForm, pluginAdData);
					}
				}
			}
			m_isGotGoods = false;
			return flag;
		}

		private void ClearLeaveFormPluginAdData()
		{
			m_leaveFormTotalTime = 0;
		}
	}
}
