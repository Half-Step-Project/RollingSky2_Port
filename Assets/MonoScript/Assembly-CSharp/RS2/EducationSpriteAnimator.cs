using System.Collections.Generic;
using System.Linq;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class EducationSpriteAnimator : EducationAnimator
	{
		private bool m_speedUp;

		private GameObject m_CurrentRole;

		private List<object> m_cacheObj = new List<object>();

		private EducationDisplayDirector m_Parent;

		private int flag;

		public EducationDisplayDirector Parent
		{
			get
			{
				return m_Parent;
			}
			set
			{
				m_Parent = value;
			}
		}

		public void Initialize()
		{
			string playerAsset = AssetUtility.GetPlayerAsset(PlayerDataModule.Instance.GetPlayerStageAvaterName());
			Mod.Resource.LoadAsset(playerAsset, new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				SetRole(asset as GameObject);
				m_cacheObj.Add(asset);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			}));
		}

		public void SetStateSpeed(string param, float speed)
		{
			m_Animator.SetFloat(param, speed);
		}

		private void RefreshSpeed()
		{
			m_speedUp = PlayerDataModule.Instance.IsProductSpeedUpGoing();
			SetStateSpeed("Multi", m_speedUp ? EducationDisplayDirector.Instance.m_spriteSpeed : 1f);
		}

		private void OnMouseUpAsButton()
		{
		}

		private void SetRole(GameObject obj)
		{
			GameObject gameObject = (m_CurrentRole = Object.Instantiate(obj));
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.SetActive(true);
			m_Animator = gameObject.GetComponentInChildren<Animator>();
			InvokeRepeating("RefreshSpeed", 0f, 3f);
			flag = 1;
		}

		private void OnRenderObject()
		{
			if (flag != 2 && Parent != null && flag == 1)
			{
				Parent.StartTutorial();
				flag = 2;
			}
		}

		public void OnPlayerStarUpgrade()
		{
			string playerAsset = AssetUtility.GetPlayerAsset(PlayerDataModule.Instance.GetPlayerStageAvaterName());
			Mod.Resource.LoadAsset(playerAsset, new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				PlayAnim("StarUpgrade1");
				m_cacheObj.Add(asset);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load item '{0}' from '{1}' with error message.", assetName, errorMessage));
			}));
		}

		public void OnUpgrade1Finish()
		{
			Object.Destroy(m_CurrentRole);
			SetRole(m_cacheObj.Last() as GameObject);
			PlayAnim("StarUpgrade2");
		}

		private void OnDestroy()
		{
			foreach (object item in m_cacheObj)
			{
				Mod.Resource.UnloadAsset(item);
			}
			CancelInvoke("RefreshSpeed");
			m_cacheObj.Clear();
			m_cacheObj = null;
		}
	}
}
