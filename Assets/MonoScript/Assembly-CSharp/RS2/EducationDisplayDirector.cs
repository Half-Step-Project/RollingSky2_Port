using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Foundation;
using UnityEngine;
using UnityEngine.Events;

namespace RS2
{
	public sealed class EducationDisplayDirector : MonoBehaviour
	{
		public static EducationDisplayDirector Instance;

		public static bool InEducation;

		private static bool _isInitialized;

		[SerializeField]
		private EducationAnimator m_Camera;

		[SerializeField]
		private EducationSpriteAnimator m_Sprite;

		[SerializeField]
		private EducationAnimator m_Series;

		[SerializeField]
		private SlotSet m_SlotSet;

		[SerializeField]
		public List<Renderer> m_MaskRenders;

		public MeshRenderer SeriesMaskMeshRenderer;

		[Header("Environment")]
		[SerializeField]
		private EnvironmentData m_Education;

		[SerializeField]
		private EnvironmentData m_SelectLevel;

		[SerializeField]
		private EnvironmentData m_Tutorial;

		[SerializeField]
		private float m_Duration;

		[Header("AnimSpeed")]
		public float m_instrumentSpeed;

		public float m_spriteSpeed;

		[Header("Effect")]
		public GameObject m_startUpgradeEffect;

		public GameObject m_tutorialEffect;

		public HomeForm _homeForm
		{
			get
			{
				return Mod.UI.GetUIForm(UIFormId.HomeForm) as HomeForm;
			}
		}

		private MenuForm _MenuForm
		{
			get
			{
				return Mod.UI.GetUIForm(UIFormId.MenuViewForm) as MenuForm;
			}
		}

		public EducationAnimator _Camera
		{
			get
			{
				return m_Camera;
			}
		}

		public EducationSpriteAnimator _Sprite
		{
			get
			{
				return m_Sprite;
			}
		}

		public EducationAnimator _Series
		{
			get
			{
				return m_Series;
			}
		}

		public SlotSet _Slots
		{
			get
			{
				return m_SlotSet;
			}
		}

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
		}

		private void Start()
		{
			Initialize();
		}

		private void OnDestroy()
		{
			RemoveEventListener();
		}

		private void Initialize()
		{
			m_tutorialEffect.SetActive(false);
			m_Sprite.Parent = this;
			_Sprite.Initialize();
			_Slots.Initialize();
			_Slots.ShowInstrumentLevel(true);
			StartCoroutine(DelayInitEnvironment());
			InEducation = true;
			AddEventListener();
		}

		public void OnSelectLevel()
		{
			_Camera.PlayAnim("SelectLevel");
			_Sprite.PlayAnim("SelectLevel");
			_Series.PlayAnim("SelectLevel");
			_Slots.PlayAnim("SelectLevel");
			_homeForm.SetState(HomeFormState.SelectLevel);
			_MenuForm.SetState(MenuFormState.SelectLevel);
			ChangeEnvironmentData(m_SelectLevel, m_Duration);
			if ((bool)SeriesMaskMeshRenderer && (bool)SeriesMaskMeshRenderer.material)
			{
				SeriesMaskMeshRenderer.material.SetTexture("_MainTex", _homeForm.GetLevelBgRenderTexture());
			}
			else
			{
				Debug.LogErrorFormat("SeriesMaskMeshRenderer == null ({0}) | SeriesMaskMeshRenderer.material == null ({1})", SeriesMaskMeshRenderer == null, SeriesMaskMeshRenderer.material == null);
			}
			InEducation = false;
		}

		public void OnSelectLevelBack()
		{
			_Camera.PlayAnim("BackToMain");
			_Sprite.PlayAnim("BackToMain");
			_Series.PlayAnim("BackToMain");
			_Slots.PlayAnim("BackToMain");
			_Slots.ShowInstrumentLevel(true);
			_homeForm.SetState(HomeFormState.Education);
			_MenuForm.SetState(MenuFormState.Education);
			ChangeEnvironmentData(m_Education, m_Duration);
			InEducation = true;
		}

		public void OnMusicalInstrumentUpgrade()
		{
			_Camera.PlayAnim("MIUpgrade");
			_Slots.PlayAnim("MIUpgrade");
			_Slots.ShowInstrumentLevel(false);
			InEducation = false;
		}

		public void OnMusicalInstrumentUpgradeBack()
		{
			_Camera.PlayAnim("MIUpgradeBack");
			_Slots.PlayAnim("MIUpgradeBack");
			_Slots.ShowInstrumentLevel(true);
			InEducation = true;
		}

		public void OnPlayerUpgradeFormOpen()
		{
			_Camera.PlayAnim("PlayerUpgradeFormOpen");
			InEducation = false;
		}

		public void OnPlayerUpgradeFormClose()
		{
			_Camera.PlayAnim("PlayerUpgradeFormClose");
			InEducation = true;
		}

		public void OnLevelIn()
		{
			_Sprite.PlayAnim("EnterLevel");
			_Series.PlayAnim("EnterLevel");
			_Camera.PlayAnim("EnterLevel");
		}

		public void OnPlayerUpgrade()
		{
			_Sprite.PlayAnim("LevelUpgrade");
		}

		public void OnPlayerStarUpgrade()
		{
			_Sprite.OnPlayerStarUpgrade();
			_Slots.PlayAnim("StarUpgrade");
			for (int i = 0; i < 7; i++)
			{
				Slot slot = _Slots.GetSlot(i);
				if (!slot.IsEmpty && !slot.MI.Lock)
				{
					slot.MI.PlayAnim(MusicalInstrument.AnimState.StartUpgrade);
				}
			}
			Object.Instantiate(m_startUpgradeEffect);
		}

		private IEnumerator DelayInitEnvironment()
		{
			if (TutorialManager.Instance.GetCurrentStageId() != TutorialStageId.STAGE_HOME_MENU)
			{
				yield return new WaitForEndOfFrame();
				ChangeEnvironmentData(m_Education, 0.01f);
			}
		}

		private void ChangeEnvironmentData(EnvironmentData to, float duration)
		{
			MaterialTool.GetSkyboxMaterial().DOColor(to.m_EducationSkyBoxColor, "_SubColor", duration);
			DOTween.To(() => RenderSettings.fogColor, delegate(Color x)
			{
				RenderSettings.fogColor = x;
			}, to.m_EducationFogColor, duration);
			DOTween.To(delegate(float x)
			{
				RenderSettings.fogStartDistance = x;
			}, RenderSettings.fogStartDistance, to.m_EducationFogStart, duration);
			DOTween.To(delegate(float x)
			{
				RenderSettings.fogEndDistance = x;
			}, RenderSettings.fogEndDistance, to.m_EducationFogEnd, duration);
		}

		private void AddEventListener()
		{
			Mod.Event.Subscribe(EventArgs<TrigerTutorialStepEventArgs>.EventId, OnTutorialTrigerHandler);
		}

		private void RemoveEventListener()
		{
			Mod.Event.Unsubscribe(EventArgs<TrigerTutorialStepEventArgs>.EventId, OnTutorialTrigerHandler);
		}

		private void OnTutorialTrigerHandler(object sender, EventArgs args)
		{
			TrigerTutorialStepEventArgs trigerTutorialStepEventArgs = args as TrigerTutorialStepEventArgs;
			if (trigerTutorialStepEventArgs != null && trigerTutorialStepEventArgs.StageStepId == TutorialStepId.STAGE_HOME_MENU_STEP_0)
			{
				DealWithTutorial();
			}
		}

		public void StartTutorial()
		{
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_HOME_MENU)
			{
				m_Sprite.PlayAnim("RoleIn");
				_Slots.gameObject.SetActive(false);
				TutorialManager.Instance.HideUIGroup(0, 2);
				m_tutorialEffect.SetActive(true);
				MaterialTool.GetSkyboxMaterial().SetColor("_SubColor", Color.black);
				StartCoroutine(DelayOperate(1f, delegate
				{
					ChangeEnvironmentData(m_Tutorial, 3.5f);
				}));
				StartCoroutine(DelayOperate(6f, delegate
				{
					m_tutorialEffect.SetActive(false);
				}));
			}
		}

		private IEnumerator DelayOperate(float delay, UnityAction callBack)
		{
			yield return new WaitForSeconds(delay);
			callBack();
		}

		private void DealWithTutorial()
		{
			if (TutorialManager.Instance.GetCurrentStageId() == TutorialStageId.STAGE_HOME_MENU)
			{
				CommonTutorialData commonTutorialData = new CommonTutorialData(-1);
				CommonTutorialStepData commonTutorialStepData = new CommonTutorialStepData();
				commonTutorialStepData.showContent = true;
				commonTutorialStepData.needBlock = false;
				commonTutorialStepData.changeRect = false;
				commonTutorialStepData.stepType = TutorialStepType.ONLY_CONTENT;
				commonTutorialStepData.tutorialContent = Mod.Localization.GetInfoById(321);
				commonTutorialStepData.finishAction = delegate
				{
					Mod.Event.Fire(this, Mod.Reference.Acquire<TrigerTutorialStepEventArgs>().Initialize(TutorialStepId.STAGE_HOME_MENU_STEP_1));
					TutorialManager.Instance.ShowUIGroup(2);
				};
				commonTutorialData.AddStep(commonTutorialStepData);
				BuildinTutorialForm.Form.StartTutorial(commonTutorialData);
			}
		}
	}
}
