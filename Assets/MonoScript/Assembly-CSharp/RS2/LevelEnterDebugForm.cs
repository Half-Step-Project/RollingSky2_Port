using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class LevelEnterDebugForm : UGUIForm
	{
		public GameObject m_closeBtn;

		public UILoopList m_loopList;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			AddEventListener();
			SetData();
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventListener();
		}

		private void SetData()
		{
			LevelSeries_table[] records = Mod.DataTable.Get<LevelSeries_table>().Records;
			List<int> list = new List<int>();
			for (int i = 0; i < records.Length; i++)
			{
				list.Add(records[i].Id);
			}
			m_loopList.Data(list);
		}

		private void AddEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
		}

		private void RemoveEventListener()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickCloseButton));
		}

		private void OnClickCloseButton(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.LevelEnterDebugForm);
		}
	}
}
