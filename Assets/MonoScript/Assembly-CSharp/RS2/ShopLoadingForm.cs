using System;
using Foundation;
using UnityEngine.UI;

namespace RS2
{
	public class ShopLoadingForm : UGUIForm
	{
		public class ShopLoadingFormData
		{
			public float m_time;

			public System.Action m_callBack;
		}

		private ShopLoadingFormData m_shopLoadingFormData;

		private float m_currentTime;

		private float m_intervalTime;

		public Image m_image;

		protected override void OnOpen(object userData)
		{
			m_shopLoadingFormData = (ShopLoadingFormData)userData;
			m_intervalTime = ((m_shopLoadingFormData != null) ? m_shopLoadingFormData.m_time : 60f);
			m_currentTime = 0f;
			base.OnOpen(userData);
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			m_currentTime += elapseSeconds;
			m_image.gameObject.transform.Rotate(0f, 0f, elapseSeconds * -100f);
			if (m_currentTime >= m_intervalTime)
			{
				if (m_shopLoadingFormData != null && m_shopLoadingFormData.m_callBack != null)
				{
					m_shopLoadingFormData.m_callBack();
				}
				Mod.UI.CloseUIForm(UIFormId.ShopLoadingForm);
			}
		}
	}
}
