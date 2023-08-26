using System;
using System.Collections.Generic;
using Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace RS2
{
	public class SceneTutorialForm : UGUIForm, ITutorialHandOperate
	{
		private enum TutorialStage
		{
			NONE,
			PREPARE,
			START,
			PAUSE,
			RESUME,
			FINISH
		}

		public enum FormType
		{
			Start,
			Tutorial
		}

		public static readonly float SWITCH_TIME = 3f;

		public static readonly float BEGIN_ALPHA = 1f;

		public static readonly float DEFAULT_WIDTH = 720f;

		public static readonly float DEFAULT_HEIGHT = 1280f;

		public static readonly float DEFAULT_WIDTH_DIV_HEIGHT = DEFAULT_WIDTH / DEFAULT_HEIGHT;

		public static readonly float HALF_BORDER_SIZE = 280f;

		public static readonly float DEFAULT_HAND_POS_Y = 200f;

		public static readonly int MAX_SHOW_TIMETIPS = 3;

		private List<object> loadedAsserts = new List<object>();

		private InsideGameDataModule insideGameModule;

		public GameObject timeCountWidget;

		public Image timeCountDownSprite;

		public Image startTipsTutorial;

		public GameObject handWidget;

		public Image handLineSp;

		public Image handCircleSp;

		private WaitForSeconds m_waitForSecond;

		public GameObject m_inEffect;

		public GameObject m_outEffect;

		public GameObject m_handCircleSp2;

		public GameObject m_PauseTxt;

		public Text m_StartText;

		private bool ifFinishGame;

		private float halfBorderSize;

		private float handPosY;

		public TipStartController m_tutorialController;

		public List<ChangeParticleLayers> particle = new List<ChangeParticleLayers>();

		public AudioSource[] timeCountSounds;

		public GameObject m_slideArea;

		public GameObject mStartWidget;

		public GameObject mStartButton;

		[SerializeField]
		private TutorialStage m_stage;

		[SerializeField]
		private RectTransform m_handArea;

		[Label]
		public FormType mType;

		private bool mIsToFormType;

		private int mFrameCount = 2;

		private int mCurrentFrame;

		private bool mIsPause;

		private float mSoundTime;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			Init();
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);
			ChangeParticelLevel();
			m_tutorialController.Open();
			AddEventListener();
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(mStartButton);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickStartButton));
			Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule).GuideLine = true;
			Mod.Event.FireNow(this, Mod.Reference.Acquire<PropsAddEventArgs>().Initialize(PropsName.PATHGUIDE));
			SwitchFormType(FormType.Start);
			TutorialNoteScript tutorialNoteScript = UnityEngine.Object.FindObjectOfType<TutorialNoteScript>();
			if ((bool)tutorialNoteScript)
			{
				tutorialNoteScript.PlayReady();
			}
			ifFinishGame = false;
			RectTransform component = mStartButton.transform.Find("startIcon/StartButton").GetComponent<RectTransform>();
			float preferredWidth = component.GetComponentInChildren<CustomText>().preferredWidth;
			mStartButton.transform.Find("startIcon/StartButton/Image").gameObject.SetActive(false);
			component.sizeDelta = new Vector2(preferredWidth * 1.8f, component.sizeDelta.y);
			EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(mStartButton.transform.Find("startIcon/StartButton").gameObject);
			eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnClickStartButton));
		}

		public void OnClickStartButton(GameObject go)
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<ClickGameStartButtonEventArgs>().Initialize());
			mStartWidget.SetActive(false);
		}

		private void SwitchFormType(FormType formType)
		{
			mType = formType;
			switch (formType)
			{
			case FormType.Start:
				mStartWidget.SetActive(false);
				handWidget.SetActive(false);
				timeCountWidget.SetActive(false);
				m_outEffect.SetActive(false);
				m_inEffect.SetActive(false);
				m_handCircleSp2.SetActive(false);
				handCircleSp.enabled = false;
				m_PauseTxt.gameObject.SetActive(false);
				m_slideArea.SetActive(false);
				mIsToFormType = false;
				mCurrentFrame = 0;
				break;
			case FormType.Tutorial:
				mIsToFormType = true;
				mCurrentFrame = 0;
				OnPauseTime();
				break;
			}
		}

		private void ToTuorial()
		{
			mStartWidget.SetActive(false);
			m_stage = TutorialStage.PAUSE;
			timeCountWidget.SetActive(false);
			handCircleSp.enabled = false;
			m_inEffect.SetActive(true);
			m_outEffect.SetActive(false);
			m_handCircleSp2.SetActive(true);
			PauseHandler();
		}

		protected override void OnClose(object userData)
		{
			EventTriggerListener eventTriggerListener = EventTriggerListener.Get(mStartButton);
			eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnClickStartButton));
			base.OnClose(userData);
			OnClose();
			ifFinishGame = false;
			OnResumeTime();
		}

		protected override void OnUnload()
		{
			base.OnUnload();
			Release();
		}

		protected override void OnPause()
		{
			base.OnPause();
			UGUIForm uIForm = Mod.UI.GetUIForm(UIFormId.PauseForm);
			if ((!uIForm || !uIForm.IsAvailable) && mType != 0)
			{
				OnResumeTime();
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			if (mType != 0)
			{
				OnPauseTime();
				m_stage = TutorialStage.PAUSE;
				PauseHandler();
			}
		}

		private void OnPauseTime()
		{
		}

		private void OnResumeTime()
		{
			if (Mod.Core.Speed != 1f)
			{
				mIsPause = false;
				Debug.Log(1);
				Mod.Core.Speed = 1f;
				if (Mod.Sound.GetMusicTime() >= 1f && mSoundTime >= 1f)
				{
					Singleton<MenuMusicController>.Instance.ResumeGameMusic(mSoundTime);
				}
				else
				{
					Singleton<MenuMusicController>.Instance.ResumeGameMusic();
				}
			}
		}

		public void Init()
		{
			if ((float)Screen.width / (float)Screen.height >= DEFAULT_WIDTH_DIV_HEIGHT)
			{
				halfBorderSize = HALF_BORDER_SIZE * (float)Screen.height / DEFAULT_HEIGHT;
				handPosY = DEFAULT_HAND_POS_Y * (float)Screen.height / DEFAULT_HEIGHT;
			}
			else
			{
				halfBorderSize = HALF_BORDER_SIZE * (float)Screen.width / DEFAULT_WIDTH;
				handPosY = DEFAULT_HAND_POS_Y * (float)Screen.width / DEFAULT_WIDTH;
			}
			Dictionary<string, GameObject> dictionary = ViewTools.CollectAllGameObjects(base.gameObject);
			timeCountSounds = new AudioSource[4];
			timeCountSounds[0] = dictionary["Sound_Go"].GetComponent<AudioSource>();
			timeCountSounds[1] = dictionary["Sound_1"].GetComponent<AudioSource>();
			timeCountSounds[2] = dictionary["Sound_2"].GetComponent<AudioSource>();
			timeCountSounds[3] = dictionary["Sound_3"].GetComponent<AudioSource>();
			m_tutorialController.Init(this);
			insideGameModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			GameController.GAMESTATE m_gameState = GameController.Instance.M_gameState;
			timeCountWidget.SetActive(false);
			timeCountDownSprite.gameObject.SetActive(false);
		}

		protected override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			base.OnTick(elapseSeconds, realElapseSeconds);
			if (mIsPause)
			{
				mSoundTime += elapseSeconds;
			}
			if (mIsToFormType)
			{
				mCurrentFrame++;
				if (mCurrentFrame >= mFrameCount)
				{
					mCurrentFrame = 0;
					ToTuorial();
					mIsToFormType = false;
				}
			}
			else
			{
				if (mType == FormType.Start)
				{
					return;
				}
				if (m_stage == TutorialStage.START || m_stage == TutorialStage.PAUSE)
				{
					UpdateControlTips();
					if (m_stage == TutorialStage.START && !IsShowTutorialArea())
					{
						m_slideArea.SetActive(false);
					}
				}
				if (m_stage != TutorialStage.PAUSE)
				{
					return;
				}
				if (GameController.Instance.M_gameState == GameController.GAMESTATE.End)
				{
					OnResumeTime();
				}
				else if (GameController.Instance.M_gameState == GameController.GAMESTATE.Runing)
				{
					if ((bool)BaseRole.theBall)
					{
						ifFinishGame = BaseRole.theBall.IfWinBeforeFinish || ifFinishGame;
					}
					if (ifFinishGame)
					{
						OnResumeTime();
					}
					else
					{
						OnPauseTime();
					}
				}
			}
		}

		public void OnClose()
		{
			RemoveEventListener();
			m_tutorialController.Reset();
		}

		private void ChangeParticelLevel()
		{
			Canvas component = base.gameObject.GetComponent<Canvas>();
			if (component != null)
			{
				for (int i = 0; i < particle.Count; i++)
				{
					particle[i].ChangeLayer(component.sortingOrder);
				}
			}
		}

		private void UpdateControlTips()
		{
			Vector3 validScreenPos = MonoSingleton<GameTools>.Instacne.GetValidScreenPos(Input.mousePosition);
			int num = Screen.width / 2;
			int num2 = num - (int)halfBorderSize;
			int num3 = num + (int)halfBorderSize;
			validScreenPos.x = Mathf.Max(num2, Mathf.Min(num3, validScreenPos.x));
			Vector3 position = Vector3.zero;
			position = Mod.UI.UICamera.ScreenToWorldPoint(validScreenPos);
			position.y = handCircleSp.transform.position.y;
			handCircleSp.transform.position = position;
			Vector3 localPosition = handCircleSp.transform.localPosition;
			localPosition.z = 0f;
			handCircleSp.transform.localPosition = localPosition;
			Vector3 position2 = handLineSp.transform.position;
			position2.y = position.y;
			handLineSp.transform.position = position2;
		}

		private bool IsOptimalSlideZone()
		{
			Vector3 validScreenPos = MonoSingleton<GameTools>.Instacne.GetValidScreenPos(Input.mousePosition);
			Vector3 position = Mod.UI.UICamera.ScreenToWorldPoint(validScreenPos);
			Vector3 point = m_slideArea.transform.InverseTransformPoint(position);
			return (m_slideArea.transform as RectTransform).rect.Contains(point);
		}

		private void PlayNotInOptiamlSlideZoneAlert()
		{
		}

		private void StopNotInOptiamlSlideZoneAlert()
		{
		}

		private bool IsShowTutorialArea()
		{
			bool result = false;
			if (!MonoSingleton<GameTools>.Instacne.IsClickInItemZone(m_handArea))
			{
				result = true;
			}
			return result;
		}

		private void AddEventListener()
		{
			Mod.Event.Subscribe(EventArgs<OpenTutorialVideoEventArgs>.EventId, OnOpenTutorialVideo);
			Mod.Event.Subscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
			Mod.Event.Subscribe(EventArgs<GameStartButtonActiveEventArgs>.EventId, OnGameStartButtonActiveHandle);
		}

		private void ShowAnimationSprite(int count)
		{
			timeCountDownSprite.gameObject.SetActive(false);
			string spriteName = string.Format("Tutorial_Label_{0}", count - 1);
			Mod.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(spriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				timeCountDownSprite.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				timeCountDownSprite.SetNativeSize();
				timeCountDownSprite.gameObject.SetActive(true);
				loadedAsserts.Add(asset);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", spriteName, assetName, errorMessage));
			}));
		}

		private void PrepareGame()
		{
			timeCountWidget.SetActive(true);
			m_stage = TutorialStage.PREPARE;
			PlayGoAnimation(StartGame);
		}

		private void PlayGoAnimation(Foundation.Action endCallBack)
		{
			if (endCallBack != null)
			{
				endCallBack();
			}
			timeCountDownSprite.gameObject.SetActive(false);
		}

		private void StartGame()
		{
			m_stage = TutorialStage.START;
			Mod.Event.Fire(this, Mod.Reference.Acquire<ClickGameStartButtonEventArgs>().Initialize());
			timeCountWidget.SetActive(false);
			handCircleSp.enabled = false;
			m_inEffect.SetActive(true);
			m_outEffect.SetActive(false);
			m_handCircleSp2.SetActive(true);
		}

		private void ReStartGame()
		{
			timeCountWidget.SetActive(true);
			timeCountDownSprite.gameObject.SetActive(false);
			StopAllCoroutines();
			m_stage = TutorialStage.NONE;
			handCircleSp.enabled = true;
			m_handCircleSp2.SetActive(false);
			m_inEffect.SetActive(false);
			m_outEffect.SetActive(true);
		}

		public void OnHandUp()
		{
			if (m_stage == TutorialStage.PREPARE)
			{
				ReStartGame();
			}
			else if (m_stage == TutorialStage.START)
			{
				m_stage = TutorialStage.PAUSE;
				if (ifFinishGame)
				{
					OnResumeTime();
					return;
				}
				PauseHandler();
				OnPauseTime();
			}
		}

		private void PauseHandler()
		{
			OnPauseTime();
			m_slideArea.SetActive(false);
		}

		public void OnHanderDown()
		{
			if (m_stage == TutorialStage.PAUSE && !ifFinishGame)
			{
				OnResumeTime();
				m_stage = TutorialStage.START;
				m_slideArea.SetActive(false);
			}
		}

		private void EndTutorial()
		{
			OnResumeTime();
			ifFinishGame = true;
			Mod.UI.CloseUIForm(UIFormId.SceneTutorialForm);
			TutorialManager.Instance.EndCurrentStage();
		}

		private void RemoveEventListener()
		{
			Mod.Event.Unsubscribe(EventArgs<OpenTutorialVideoEventArgs>.EventId, OnOpenTutorialVideo);
			Mod.Event.Unsubscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
			Mod.Event.Unsubscribe(EventArgs<GameStartButtonActiveEventArgs>.EventId, OnGameStartButtonActiveHandle);
		}

		public void Release()
		{
			for (int i = 0; i < loadedAsserts.Count; i++)
			{
				Mod.Resource.UnloadAsset(loadedAsserts[i]);
			}
			loadedAsserts.Clear();
		}

		private void OnOpenTutorialVideo(object sender, Foundation.EventArgs e)
		{
			if (e is OpenTutorialVideoEventArgs && Mod.UI.UIFormIsOpen(UIFormId.SceneTutorialForm))
			{
				SwitchFormType(FormType.Tutorial);
			}
		}

		private void OnGameStart(object sender, Foundation.EventArgs e)
		{
			GameStartEventArgs gameStartEventArgs = e as GameStartEventArgs;
			if (gameStartEventArgs != null && Mod.UI.UIFormIsOpen(UIFormId.SceneTutorialForm) && gameStartEventArgs.StartType == GameStartEventArgs.GameStartType.ForceRun && (bool)InputController.instance)
			{
				InputController.instance.EnableInput(false);
			}
		}

		private void OnGameStartButtonActiveHandle(object sender, Foundation.EventArgs e)
		{
			GameStartButtonActiveEventArgs gameStartButtonActiveEventArgs = e as GameStartButtonActiveEventArgs;
			if (gameStartButtonActiveEventArgs != null && mStartWidget != null)
			{
				mStartWidget.SetActive(gameStartButtonActiveEventArgs.mActive);
			}
		}

		public void EndHand()
		{
			EndTutorial();
		}
	}
}
