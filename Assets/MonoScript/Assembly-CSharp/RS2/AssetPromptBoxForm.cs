using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class AssetPromptBoxForm : UGUIForm
	{
		public AssetPromptBoxUINode m_rightUpNode;

		public AssetPromptBoxUINode m_rightDownNode;

		public AssetPromptBoxUINode m_leftUpNode;

		public AssetPromptBoxUINode m_leftDownNode;

		public GameObject m_background;

		private AssetPromptBoxUINode m_currentNode;

		private AssetPromptBoxFormData m_data;

		public AssetPromptBoxFormData CurrentData
		{
			get
			{
				return m_data;
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
		}

		protected override void OnOpen(object userData)
		{
			m_rightUpNode.gameObject.SetActive(false);
			m_rightDownNode.gameObject.SetActive(false);
			m_leftUpNode.gameObject.SetActive(false);
			m_leftDownNode.gameObject.SetActive(false);
			base.OnOpen(userData);
			m_data = userData as AssetPromptBoxFormData;
			if (m_data != null)
			{
				m_currentNode = null;
				Vector2 vector = Mod.UI.UICamera.WorldToScreenPoint(m_data.m_target.position);
				float num = (float)Screen.width - vector.x;
				float num2 = (float)Screen.height - vector.y;
				if (num >= vector.x && num2 >= vector.y)
				{
					m_currentNode = m_rightUpNode;
				}
				else if (num >= vector.x && num2 < vector.y)
				{
					m_currentNode = m_rightDownNode;
				}
				else if (num < vector.x && num2 >= vector.y)
				{
					m_currentNode = m_leftUpNode;
				}
				else if (num < vector.x && num2 < vector.y)
				{
					m_currentNode = m_leftDownNode;
				}
				else
				{
					m_currentNode = m_rightDownNode;
				}
				if ((bool)m_currentNode)
				{
					m_currentNode.gameObject.SetActive(true);
				}
				m_currentNode.transform.position = m_data.m_target.position;
				Goods_goodsTable goodData = Mod.DataTable.Get<Goods_goodsTable>().Get(m_data.m_goodID);
				m_currentNode.OnOpen(goodData);
				EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_background);
				eventTriggerListener.onDown = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onDown, new EventTriggerListener.VoidDelegate(OnClickBackground));
			}
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if (Input.GetMouseButtonDown(0) && m_currentNode != null)
			{
				Mod.UI.CloseUIForm(UIFormId.AssetPromptBoxForm);
			}
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			m_currentNode = null;
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_background);
			eventTriggerListener.onDown = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onDown, new EventTriggerListener.VoidDelegate(OnClickBackground));
			if (m_currentNode != null)
			{
				m_currentNode.OnClose();
			}
		}

		protected override void OnUnload()
		{
			base.OnUnload();
		}

		private void OnClickBackground(GameObject obj)
		{
			Mod.UI.CloseUIForm(UIFormId.AssetPromptBoxForm);
		}
	}
}
