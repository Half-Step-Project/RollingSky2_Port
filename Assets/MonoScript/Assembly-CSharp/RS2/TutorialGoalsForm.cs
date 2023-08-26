using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class TutorialGoalsForm : UGUIForm
	{
		public GameObject m_closeBtn;

		public GameObject m_BeforeAdEffect;

		public Image m_BeforeImage;

		public Text m_BeforeText;

		public DOTweenAnimation m_tweenAnimationIn;

		public GameObject m_Effect;

		private bool m_TweenInFinished;

		private AssetLoadCallbacks m_BeforeImageAssetLoadCallBack;

		private List<object> loadedAsserts = new List<object>();

		private bool m_isRelease;

		private TutorialGoalsData m_goalData;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			base.gameObject.SetActive(true);
			m_BeforeImage.gameObject.SetActive(false);
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
					PlayInAnimation();
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
			m_goalData = userData as TutorialGoalsData;
			if (m_goalData == null)
			{
				CloseForm(false);
				return;
			}
			AddEventHandler();
			m_TweenInFinished = false;
			string assetName = string.Format("pluginAd_{0}", 1);
			Mod.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(assetName), m_BeforeImageAssetLoadCallBack);
		}

		private void PlayInAnimation()
		{
			if (!(m_tweenAnimationIn != null))
			{
				return;
			}
			List<Tween> tweens = m_tweenAnimationIn.GetTweens();
			for (int i = 0; i < tweens.Count; i++)
			{
				if (i != 0)
				{
					continue;
				}
				tweens[i].OnComplete(delegate
				{
					m_TweenInFinished = true;
					if (m_Effect != null)
					{
						m_Effect.SetActive(true);
					}
				});
			}
			m_tweenAnimationIn.DORestart();
			m_tweenAnimationIn.DOPlayForward();
		}

		private void PlayOutAnimation()
		{
			if (m_BeforeAdEffect != null)
			{
				m_BeforeAdEffect.transform.DOLocalMoveY(-1280f, 0.5f).SetDelay(0.05f).OnComplete(delegate
				{
					CloseForm(false);
				});
			}
		}

		private void ResetAnimator()
		{
		}

		protected override void OnClose(object userData)
		{
			base.OnClose(userData);
			RemoveEventHandler();
			StopAllCoroutines();
			if (m_tweenAnimationIn != null)
			{
				m_tweenAnimationIn.DOKill();
			}
			if (m_goalData != null)
			{
				m_goalData.EndCallBack();
			}
			m_goalData = null;
		}

		private void AddEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseClickHandler));
		}

		private void RemoveEventHandler()
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(m_closeBtn);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(CloseClickHandler));
		}

		private void CloseClickHandler(GameObject go)
		{
			if (m_TweenInFinished)
			{
				PlayOutAnimation();
			}
		}

		private void CloseForm(bool playOutAnimaion = true)
		{
			if (!playOutAnimaion)
			{
				Mod.UI.CloseUIForm(UIFormId.TutorialGoalsForm);
			}
			else
			{
				StartCoroutine(DelayCloseForm(0.4f));
			}
		}

		private IEnumerator DelayCloseForm(float deltaTime)
		{
			yield return new WaitForSeconds(deltaTime);
			ResetAnimator();
			Mod.UI.CloseUIForm(UIFormId.TutorialGoalsForm);
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
	}
}
