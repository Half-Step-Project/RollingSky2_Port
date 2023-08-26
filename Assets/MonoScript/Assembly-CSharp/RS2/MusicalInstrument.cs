using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RS2
{
	public sealed class MusicalInstrument : MonoBehaviour
	{
		public enum AnimState
		{
			Idel,
			Click,
			Rest,
			LevelUp,
			StartUpgrade
		}

		[SerializeField]
		private TextMesh m_levelTex;

		[SerializeField]
		private GameObject m_lock;

		[SerializeField]
		private GameObject m_starEffect;

		[SerializeField]
		private Animation m_RiseTextAnim;

		[SerializeField]
		private TextMesh m_RiseText;

		[SerializeField]
		private ParticleSystem[] m_productEffects;

		private List<List<int>> m_soundIDs;

		private float m_currentRate;

		private int? m_cacheSoundID;

		private bool m_needPlyaUpgrade;

		private double m_product_speed;

		private System.Random random;

		private Animator m_MusicalInstrument;

		private MaterialsSwitcher m_MaterialSwitcher;

		private Tweener m_levelTextFade;

		private float m_clickDelayFade;

		private int randomIndex = -1;

		private int soundIndex;

		private float mTime;

		private float mRandomDuration = 3f;

		public int m_ID { get; private set; }

		public bool Lock
		{
			get
			{
				return m_islock;
			}
		}

		public PlayerLocalInstrumentData m_data { get; private set; }

		private bool m_islock
		{
			get
			{
				return (int)m_data.LockState == 1;
			}
		}

		public void Initialize(PlayerLocalInstrumentData data)
		{
			if (m_MusicalInstrument == null)
			{
				m_MusicalInstrument = base.transform.Find("Animation").GetComponent<Animator>();
			}
			if (m_MaterialSwitcher == null)
			{
				m_MaterialSwitcher = base.transform.GetComponent<MaterialsSwitcher>();
			}
			m_data = data;
			m_ID = data.m_Id;
			m_soundIDs = data.SoundId();
			random = new System.Random(m_ID);
			m_clickDelayFade = GameCommon.instrumentLevelClickShowTime;
			RefreshState();
			if (!m_islock)
			{
				PlayEffect(true);
				StartCoroutine(DelayStartEffect());
			}
			Mod.Event.Subscribe(EventArgs<InstrumentPropertyChangeEventArgs>.EventId, OnInstrumentPropertyChange);
		}

		public void RefreshState()
		{
			m_lock.SetActive(false);
			m_levelTex.gameObject.SetActive(false);
			m_MaterialSwitcher.SwitchTo(0);
			if (!m_needPlyaUpgrade)
			{
				m_needPlyaUpgrade = !m_islock;
			}
		}

		public void PlayAnim(AnimState trigger)
		{
			m_MusicalInstrument.SetTrigger(trigger.ToString());
		}

		public void OnUpgrade()
		{
			PlayAnim(AnimState.LevelUp);
			RefreshState();
		}

		public void RefreshLock()
		{
			RefreshState();
			StopAllCoroutines();
			CancelInvoke("RepeatEffect");
			PlayEffect(!m_islock);
			RefreshAnimSpeed();
			if (!m_islock)
			{
				StartCoroutine(DelayStartEffect());
			}
		}

		private void PlayEffect(bool play)
		{
			if (m_productEffects == null || m_productEffects.Length == 0)
			{
				return;
			}
			ParticleSystem[] productEffects = m_productEffects;
			foreach (ParticleSystem particleSystem in productEffects)
			{
				if (play)
				{
					particleSystem.Play();
				}
				else
				{
					particleSystem.Stop();
				}
			}
		}

		private void RefreshEffectSpeed()
		{
			bool flag = PlayerDataModule.Instance.IsProductSpeedUpGoing();
			if (m_productEffects != null && m_productEffects.Length != 0)
			{
				ParticleSystem[] productEffects = m_productEffects;
				for (int i = 0; i < productEffects.Length; i++)
				{
					ParticleSystem.MainModule main = productEffects[i].main;
					main.simulationSpeed = ((!flag) ? 1 : 3);
				}
			}
		}

		public void PlaySound()
		{
			if (randomIndex == -1 || Time.time - mTime >= mRandomDuration)
			{
				randomIndex = random.Next(0, m_soundIDs.Count);
				soundIndex = 0;
				m_cacheSoundID = null;
			}
			if (m_cacheSoundID.HasValue)
			{
				Mod.Sound.StopSound(m_cacheSoundID.Value);
			}
			int count = m_soundIDs[randomIndex].Count;
			if (soundIndex >= count)
			{
				soundIndex = 0;
				m_cacheSoundID = null;
			}
			m_cacheSoundID = Mod.Sound.PlayUISound(m_soundIDs[randomIndex][soundIndex], SoundGroupName.MusicalInstrument);
			mTime = Time.time;
			soundIndex++;
		}

		private void OnMouseUpAsButton()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			if (list.Count > 0)
			{
				foreach (RaycastResult item in list)
				{
					if (!item.module.name.Equals("HomeForm(Clone)"))
					{
						return;
					}
				}
			}
			DoMouseUpAsButton();
		}

		public void DoMouseUpAsButton()
		{
			double productCount = 0.0;
			switch (PlayerDataModule.Instance.PlayInstrument(m_ID, out productCount))
			{
			case PlayInstrumentResultState.SUCCESS:
				PlayAnim(AnimState.Click);
				PlaySound();
				LevelTextFadeOut(m_clickDelayFade);
				Mod.Event.Fire(this, Mod.Reference.Acquire<MusicalInstrumentClickEventArgs>().Initialize(m_ID, base.transform.position, 1, productCount));
				break;
			case PlayInstrumentResultState.NONE:
			case PlayInstrumentResultState.IS_GOING_CD:
				break;
			}
		}

		private void ShowLockInfo()
		{
			if (!Mod.UI.UIFormIsOpen(UIFormId.BroadCastForm))
			{
				BroadCastData broadCastData = new BroadCastData();
				broadCastData.Type = BroadCastType.INFO;
				int playerStarLevel = PlayerDataModule.Instance.GetPlayerStarLevel();
				int num = m_data.UnlockStar();
				int num2 = m_data.UnlockLevel();
				string arg = ((playerStarLevel < num) ? string.Format(Mod.Localization.GetInfoById(277), num, num2) : string.Format(Mod.Localization.GetInfoById(278), num2));
				broadCastData.Info = string.Format("<size=30>{0}</size>", arg);
				MonoSingleton<BroadCastManager>.Instacne.BroadCast(broadCastData);
			}
		}

		private void OnInstrumentPropertyChange(object sender, Foundation.EventArgs args)
		{
			InstrumentPropertyChangeEventArgs instrumentPropertyChangeEventArgs = args as InstrumentPropertyChangeEventArgs;
			if (instrumentPropertyChangeEventArgs == null)
			{
				return;
			}
			switch (instrumentPropertyChangeEventArgs.ChangeType)
			{
			case InstrumentPropertyType.LEVEL:
				if (instrumentPropertyChangeEventArgs.InstrumentIds.Contains(m_ID))
				{
					OnUpgrade();
				}
				break;
			case InstrumentPropertyType.LOCK_STATE:
				RefreshLock();
				break;
			case InstrumentPropertyType.PRODUCT_SPEED:
				m_product_speed = m_data.ComputerProductNumByTime(1, PlayerDataModule.Instance.GetPlayerStarLevel(), m_data.m_Level);
				break;
			case InstrumentPropertyType.NONE:
			case InstrumentPropertyType.PLAY_COUNT:
			case InstrumentPropertyType.PUTON_STATE:
				break;
			}
		}

		private void RepeatEffect()
		{
			float effectRate = GetEffectRate();
			double num = m_product_speed;
			if (PlayerDataModule.Instance.IsProductSpeedUpGoing())
			{
				num /= (double)GameCommon.instrumentAdProductFactor;
			}
			Mod.Event.Fire(this, Mod.Reference.Acquire<MusicalInstrumentClickEventArgs>().Initialize(m_ID, base.transform.position, 2, num));
			if (m_currentRate != effectRate)
			{
				CancelInvoke("RepeatEffect");
				InvokeRepeating("RepeatEffect", 0f, effectRate);
				RefreshEffectSpeed();
				RefreshAnimSpeed();
				m_currentRate = effectRate;
			}
		}

		private IEnumerator DelayStartEffect()
		{
			m_product_speed = m_data.ComputerProductNumByTime(1, PlayerDataModule.Instance.GetPlayerStarLevel(), m_data.m_Level);
			float seconds = (float)random.Next(0, 10) / 10f;
			yield return new WaitForSeconds(seconds);
			RefreshEffectSpeed();
			RefreshAnimSpeed();
			m_currentRate = GetEffectRate();
			InvokeRepeating("RepeatEffect", 0f, m_currentRate);
		}

		private float GetEffectRate()
		{
			if (PlayerDataModule.Instance.IsProductSpeedUpGoing())
			{
				return GameCommon.instrumenSpeedUpBroadFrequency;
			}
			return GameCommon.instrumenCommonBroadFrequency;
		}

		private void RefreshAnimSpeed()
		{
			float value = ((PlayerDataModule.Instance.IsProductSpeedUpGoing() && !m_islock) ? EducationDisplayDirector.Instance.m_instrumentSpeed : 1f);
			m_MusicalInstrument.SetFloat("Multi", value);
		}

		public void LevelTextFadeOut(float delay)
		{
			ShowLevelText();
			m_levelTextFade = DOTween.To(delegate(float x)
			{
				m_levelTex.color = new Color(m_levelTex.color.r, m_levelTex.color.g, m_levelTex.color.b, x);
			}, m_levelTex.color.a, 0f, 0.5f).SetDelay(delay);
		}

		public void ShowLevelText()
		{
			if (m_levelTextFade != null)
			{
				m_levelTextFade.Kill();
			}
			m_levelTex.color = new Color(m_levelTex.color.r, m_levelTex.color.g, m_levelTex.color.b, 1f);
		}

		public void Release()
		{
			m_MaterialSwitcher = null;
			random = null;
			m_MusicalInstrument = null;
			m_soundIDs = null;
			m_data = null;
			StopAllCoroutines();
			CancelInvoke("RepeatEffect");
			Mod.Event.Unsubscribe(EventArgs<InstrumentPropertyChangeEventArgs>.EventId, OnInstrumentPropertyChange);
		}
	}
}
