using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public sealed class AboutForm : UGUIForm
	{
		[SerializeField]
		private RectTransform m_Transform;

		[SerializeField]
		private float m_ScrollSpeed = 1f;

		private float m_InitPosition;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			CanvasScaler componentInParent = GetComponentInParent<CanvasScaler>();
			if (componentInParent == null)
			{
				Log.Warning("Can not find CanvasScaler component.");
			}
			else
			{
				m_InitPosition = -0.5f * componentInParent.referenceResolution.x * (float)Screen.height / (float)Screen.width;
			}
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			m_Transform.SetLocalPositionY(m_InitPosition);
			Mod.Sound.PlayMusic(3);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			Mod.Sound.PlayMusic(1);
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			m_Transform.AddLocalPositionY(m_ScrollSpeed * elapseSeconds);
			if (m_Transform.localPosition.y > m_Transform.sizeDelta.y - m_InitPosition)
			{
				m_Transform.SetLocalPositionY(m_InitPosition);
			}
		}
	}
}
