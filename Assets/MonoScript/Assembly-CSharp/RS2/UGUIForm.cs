using System;
using System.Collections;
using Foundation;
using RisingWin.Library;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public abstract class UGUIForm : UIFormLogic, IJoystickListener
	{
		public const int DepthFactor = 100;

		private Canvas m_CachedCanvas;

		private CanvasGroup m_CanvasGroup;

		[Header("close animatorStateName = close")]
		[Header("close Method = OnCloseUIFormByAnim")]
		public Animator closeAnim;

		public float closeTime;

		public int OriginalDepth { get; private set; }

		public int Depth
		{
			get
			{
				return m_CachedCanvas.sortingOrder;
			}
		}

		public event System.Action DepthChangedEvent;

		public void Close(bool isShutdown = false)
		{
			Mod.UI.CloseUIForm(base.UIForm);
		}

		public void PlayUISound(int uiSoundId, float delty = 0f)
		{
			StartCoroutine(DeltyPlaySound(uiSoundId, delty));
		}

		private IEnumerator DeltyPlaySound(int soundId, float delty)
		{
			if (delty > 0f)
			{
				yield return new WaitForSeconds(delty);
			}
			Mod.Sound.PlayUISound(soundId);
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			m_CachedCanvas = base.gameObject.GetOrAddComponent<Canvas>();
			m_CachedCanvas.overrideSorting = true;
			OriginalDepth = m_CachedCanvas.sortingOrder;
			m_CanvasGroup = base.gameObject.GetOrAddComponent<CanvasGroup>();
			RectTransform component = GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.anchoredPosition = Vector2.zero;
			component.sizeDelta = Vector2.zero;
			base.gameObject.GetOrAddComponent<GraphicRaycaster>();
		}

		protected virtual bool EnableInputAfterOpen()
		{
			return false;
		}

		protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
		{
			int depth = Depth;
			base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
			int num = 10000 * uiGroupDepth + 100 * depthInUIGroup - depth + OriginalDepth;
			Canvas[] componentsInChildren = GetComponentsInChildren<Canvas>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].sortingOrder += num;
			}
			if (this.DepthChangedEvent != null)
			{
				this.DepthChangedEvent();
			}
		}

		private IEnumerator CloseCo(float duration)
		{
			yield return m_CanvasGroup.FadeToAlpha(0f, duration);
			Mod.UI.CloseUIForm(base.UIForm);
		}

		private void OnDestroy()
		{
			CancelInvoke();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
		}

		public virtual void OnDirectionTrigger(JoystickDirection pDirection, float pValue)
		{
		}

		public virtual void OnConsequentDirectionTrigger(JoystickDirection pDirection, float pValue)
		{
		}

		public virtual void OnButtonTrigger(JoystickButton pButton)
		{
		}
	}
}
