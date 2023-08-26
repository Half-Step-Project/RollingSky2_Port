using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using RS2;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGameStartManager : MonoBehaviour, ITutorialHandOperate
{
	public enum TutorialState
	{
		Null,
		Tips,
		Start,
		TimeCount,
		Game,
		End
	}

	public static readonly float SWITCH_TIME = 3f;

	public static readonly float BEGIN_ALPHA = 1f;

	public static readonly float DEFAULT_WIDTH = 720f;

	public static readonly float DEFAULT_HEIGHT = 1280f;

	public static readonly float DEFAULT_WIDTH_DIV_HEIGHT = DEFAULT_WIDTH / DEFAULT_HEIGHT;

	public static readonly float HALF_BORDER_SIZE = 280f;

	public static readonly float DEFAULT_HAND_POS_Y = 200f;

	private List<object> loadedAsserts = new List<object>();

	private InsideGameDataModule insideGameModule;

	public GameObject startWidget;

	public GameObject startGameBtn;

	public GameObject backGameBtn;

	public GameObject NS_info;

	private CustomText NStext;

	private GameObject NS_Handle;

	public GameObject imageControl;

	public GameObject imageKeyboard;

	public Text m_progressText;

	public GameObject timeCountWidget;

	public Image timeCountDownSprite;

	public GameObject m_handWidget;

	public Image handLineSp;

	public Image handCircleSp;

	public GameObject m_inEffect;

	public GameObject m_outEffect;

	public GameObject m_handCircleSp2;

	public GameObject m_slideArea;

	private RectTransform m_handArea;

	private bool ifPlayBtnClick;

	private int currentTimeCount;

	public TutorialState currentState;

	private static int ShowTipTimes;

	private float halfBorderSize;

	private float handPosY;

	public AudioSource[] timeCountSounds;

	public TipStartController m_tutorialTipController;

	private bool m_isStartFromOriginPoint;

	private int m_currentLevelId = -1;

	private bool m_isEndHand;

	private IEnumerator m_coroutine;

	private bool gameStart;

	private IEnumerator buttonCoroutine;

	private int m_showAreaState = -1;

	public void Init()
	{
		currentState = TutorialState.Null;
		if (m_progressText != null)
		{
			m_progressText.gameObject.SetActive(false);
		}
		m_handArea = m_slideArea.transform.Find("TestArea") as RectTransform;
		NStext = NS_info.transform.Find("Text").GetComponent<CustomText>();
		NS_Handle = NS_info.transform.Find("NS_Handle").gameObject;
		NS_Handle.transform.GetChild(0).gameObject.SetActive(InputController.hasSixAsix);
		NS_Handle.transform.GetChild(1).gameObject.SetActive(!InputController.hasSixAsix);
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
		insideGameModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		GameController.GAMESTATE m_gameState = GameController.Instance.M_gameState;
		if (m_gameState != GameController.GAMESTATE.Null && m_gameState != 0 && m_gameState != GameController.GAMESTATE.Reset && m_gameState != GameController.GAMESTATE.RebirthReset)
		{
			int num2 = 8;
		}
		if (backGameBtn != null && DeviceManager.Instance.IsNeedSpecialAdapte())
		{
			MonoSingleton<GameTools>.Instacne.AdapteSpecialScreen(backGameBtn.transform as RectTransform);
		}
		if (m_tutorialTipController != null)
		{
			m_tutorialTipController.Init(this);
		}
		startWidget.transform.Find("GameStartLabel/StartButton/Image").gameObject.SetActive(false);
		startWidget.transform.Find("GameStartLabel/BackToMenuButton/Image").gameObject.SetActive(false);
		startWidget.transform.Find("GameStartLabel/StartButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 180f);
		startWidget.transform.Find("GameStartLabel/BackToMenuButton").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 30f);
		float num = new float[2]
		{
			startWidget.transform.Find("GameStartLabel/StartButton/Text").GetComponent<CustomText>().preferredWidth,
			startWidget.transform.Find("GameStartLabel/BackToMenuButton/Text").GetComponent<CustomText>().preferredWidth
		}.Max();
		startWidget.transform.Find("GameStartLabel/StartButton").GetComponent<RectTransform>().sizeDelta = new Vector2(num * 1.3f, 108f);
		startWidget.transform.Find("GameStartLabel/BackToMenuButton").GetComponent<RectTransform>().sizeDelta = new Vector2(num * 1.3f, 108f);
		NS_info.SetActive(false);
		if (InputSystem.Instance.inputState == InputSystem.InputState.Keyboard)
		{
			imageControl.SetActive(false);
		}
		else
		{
			imageControl.SetActive(true);
		}
		if (buttonCoroutine != null)
		{
			StopCoroutine(buttonCoroutine);
		}
		buttonCoroutine = DelayStartButtonEvent(0.5f);
		StartCoroutine(buttonCoroutine);
	}

	public void OnOpen()
	{
		AddEventListener();
		currentTimeCount = 0;
		GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
		m_currentLevelId = dataModule.CurLevelId;
		GameController.GAMESTATE m_gameState = GameController.Instance.M_gameState;
		if (m_gameState == GameController.GAMESTATE.Null || m_gameState == GameController.GAMESTATE.Initialize || m_gameState == GameController.GAMESTATE.Reset || m_gameState == GameController.GAMESTATE.RebirthReset || m_gameState == GameController.GAMESTATE.OriginRebirthReset)
		{
			currentState = TutorialState.Start;
			InsideGameDataModule dataModule2 = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			if (dataModule2.CurrentRebirthBoxData != null || dataModule2.CurrentOriginRebirth != null)
			{
				startWidget.SetActive(true);
				m_progressText.gameObject.SetActive(true);
				m_progressText.text = dataModule2.ProgressPercentage + "%";
				backGameBtn.SetActive(false);
				m_isStartFromOriginPoint = true;
			}
			else
			{
				startWidget.SetActive(true);
				m_progressText.gameObject.SetActive(false);
				backGameBtn.SetActive(true);
				m_isStartFromOriginPoint = false;
			}
			timeCountWidget.SetActive(false);
			string spriteName = string.Format("Tutorial_Label_{0}", TutorialDataModule.MAX_TIME_COUNT - 1);
			Mod.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(spriteName), new AssetLoadCallbacks(delegate(string assetName, object asset, float duration, object data2)
			{
				timeCountDownSprite.sprite = MonoSingleton<GameTools>.Instacne.CreateSpriteByTexture2D(asset as Texture2D);
				timeCountDownSprite.SetNativeSize();
				loadedAsserts.Add(asset);
			}, delegate(string assetName, string errorMessage, object data2)
			{
				Log.Error(string.Format("Can not load sprite '{0}' from '{1}' with error message '{2}'.", spriteName, assetName, errorMessage));
			}));
			timeCountDownSprite.gameObject.SetActive(false);
			timeCountDownSprite.SetNativeSize();
			ifPlayBtnClick = false;
		}
		bool active = MonoSingleton<GameTools>.Instacne.IsCanOperateBackToMenu();
		backGameBtn.SetActive(active);
		if (m_tutorialTipController != null)
		{
			m_tutorialTipController.Open();
		}
	}

	public void OnClose()
	{
		RemoveEventListener();
		ifPlayBtnClick = false;
		if (m_tutorialTipController != null)
		{
			m_tutorialTipController.Reset();
		}
		m_showAreaState = -1;
	}

	private void AddEventListener()
	{
		Mod.Event.Subscribe(EventArgs<GameCutScenePercentEventArgs>.EventId, OnTimeCountDown);
		Mod.Event.Subscribe(EventArgs<ToClickGameStartEventArgs>.EventId, OnToClickGameStart);
		Mod.Event.Subscribe(EventArgs<GameStartButtonActiveEventArgs>.EventId, OnGameStartButtonActiveHandle);
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(startGameBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnPlayBtnClick));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(backGameBtn);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnBackClick));
		EventTriggerListener eventTriggerListener3 = EventTriggerListener.Get(startGameBtn.transform.Find("StartButton").gameObject);
		eventTriggerListener3.onClick = (EventTriggerListener.VoidDelegate)Delegate.Combine(eventTriggerListener3.onClick, new EventTriggerListener.VoidDelegate(OnPlayBtnClick));
	}

	private void OnSwitchControl(object go)
	{
		if (!ifPlayBtnClick)
		{
			bool hasSixAsix = !InputController.hasSixAsix;
			SwitchControlText(hasSixAsix);
			InputController.hasSixAsix = hasSixAsix;
		}
	}

	private void SwitchControlText(bool hasSixAsix)
	{
		NS_Handle.transform.GetChild(0).gameObject.SetActive(hasSixAsix);
		NS_Handle.transform.GetChild(1).gameObject.SetActive(!hasSixAsix);
	}

	public void OnBackClick(GameObject go)
	{
		Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		BackToMenu();
	}

	public void BackToMenu()
	{
		HomeForm.CurrentSeriesId = PlayerDataModule.Instance.LastEndterLevelData.SeriesId;
		Mod.Event.Fire(this, Mod.Reference.Acquire<GameExitEventArgs>());
	}

	private void RemoveEventListener()
	{
		Mod.Event.Unsubscribe(EventArgs<GameCutScenePercentEventArgs>.EventId, OnTimeCountDown);
		Mod.Event.Unsubscribe(EventArgs<ToClickGameStartEventArgs>.EventId, OnToClickGameStart);
		Mod.Event.Unsubscribe(EventArgs<GameStartButtonActiveEventArgs>.EventId, OnGameStartButtonActiveHandle);
		EventTriggerListener eventTriggerListener = EventTriggerListener.Get(startGameBtn);
		eventTriggerListener.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener.onClick, new EventTriggerListener.VoidDelegate(OnPlayBtnClick));
		EventTriggerListener eventTriggerListener2 = EventTriggerListener.Get(backGameBtn);
		eventTriggerListener2.onClick = (EventTriggerListener.VoidDelegate)Delegate.Remove(eventTriggerListener2.onClick, new EventTriggerListener.VoidDelegate(OnBackClick));
	}

	private void OnToClickGameStart(object sender, Foundation.EventArgs e)
	{
		OnPlayBtnClick(null);
		MonoSingleton<GameTools>.Instacne.EnableInput();
	}

	private void OnGameStartButtonActiveHandle(object sender, Foundation.EventArgs e)
	{
		GameStartButtonActiveEventArgs gameStartButtonActiveEventArgs = e as GameStartButtonActiveEventArgs;
		if (gameStartButtonActiveEventArgs != null && startWidget != null)
		{
			startWidget.SetActive(gameStartButtonActiveEventArgs.mActive);
		}
	}

	private bool IsShowTutorialArea()
	{
		LevelDifficulty levelDifficulty = PlayerDataModule.Instance.GetLevelDifficulty(m_currentLevelId);
		bool result = false;
		if (levelDifficulty == LevelDifficulty.NORMAL && (!m_tutorialTipController.PressDown || !MonoSingleton<GameTools>.Instacne.IsClickInItemZone(m_handArea)))
		{
			result = true;
		}
		return result;
	}

	private bool IsShowHanderLine()
	{
		return PlayerDataModule.Instance.GetPlayerFinishLevelCount() <= 3;
	}

	private void ShowTutorialArea()
	{
		int num = (IsShowTutorialArea() ? 1 : 0);
		if (m_showAreaState == num)
		{
			return;
		}
		m_showAreaState = num;
		if (num == 1)
		{
			if (m_coroutine != null)
			{
				StopCoroutine(m_coroutine);
			}
			m_inEffect.SetActive(true);
			m_outEffect.SetActive(false);
			m_handCircleSp2.SetActive(true);
			return;
		}
		m_slideArea.SetActive(false);
		if (!IsShowHanderLine())
		{
			if (m_coroutine != null)
			{
				StopCoroutine(m_coroutine);
			}
			m_coroutine = DelayHideTutorialArea(3f);
			StartCoroutine(m_coroutine);
		}
	}

	private void ShowHanderLine()
	{
		if (IsShowHanderLine())
		{
			m_inEffect.SetActive(true);
			m_outEffect.SetActive(false);
			m_handCircleSp2.SetActive(true);
		}
	}

	private void OnTimeCountDown(object sender, Foundation.EventArgs e)
	{
		GameCutScenePercentEventArgs gameCutScenePercentEventArgs = e as GameCutScenePercentEventArgs;
		if (gameCutScenePercentEventArgs != null)
		{
			float percent = gameCutScenePercentEventArgs.Percent;
			if (percent >= 1f && currentState != TutorialState.Game)
			{
				currentState = TutorialState.Game;
				ShowHanderLine();
			}
		}
	}

	public void OnPlayBtnClick(object param)
	{
		if (!ifPlayBtnClick)
		{
			if (startWidget != null)
			{
				startWidget.SetActive(false);
			}
			if (backGameBtn != null)
			{
				backGameBtn.SetActive(false);
			}
			if (timeCountWidget != null)
			{
				timeCountWidget.SetActive(false);
			}
			Mod.Event.Fire(this, Mod.Reference.Acquire<ClickGameStartButtonEventArgs>().Initialize());
			ifPlayBtnClick = true;
			if (m_isStartFromOriginPoint)
			{
				currentState = TutorialState.Game;
				ShowHanderLine();
			}
		}
	}

	public void OnTick(float elapseSeconds, float realElapseSeconds)
	{
		if (currentState == TutorialState.Game)
		{
			UpdateControlTips();
			ShowTutorialArea();
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

	private void Update()
	{
		if (!CommonDialogUtil.Instance.onShow && startWidget.activeSelf && gameStart)
		{
			if (InputService.KeyDown_A)
			{
				OnPlayBtnClick(null);
			}
			else if (InputService.KeyDown_B)
			{
				BackToMenu();
			}
		}
	}

	private IEnumerator DelayStartButtonEvent(float time)
	{
		gameStart = false;
		yield return new WaitForSeconds(time);
		gameStart = true;
	}

	public void Release()
	{
		for (int i = 0; i < loadedAsserts.Count; i++)
		{
			Mod.Resource.UnloadAsset(loadedAsserts[i]);
		}
		loadedAsserts.Clear();
	}

	public void OnHanderDown()
	{
	}

	public void OnHandUp()
	{
	}

	public void EndHand()
	{
	}

	private IEnumerator DelayHideTutorialArea(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		SetTutorialAreaState(false);
	}

	private void SetTutorialAreaState(bool isShow)
	{
	}

	private RectTransform SetNSButton(RectTransform buttonRoot, float maxWidth)
	{
		RectTransform component = buttonRoot.GetComponentInChildren<CustomText>().GetComponent<RectTransform>();
		float x = buttonRoot.transform.Find("Image").GetComponent<RectTransform>().sizeDelta.x;
		float num = (maxWidth + x) * 1.3f;
		float num2 = (num - (maxWidth + x)) / 3f;
		component.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, component.GetComponent<RectTransform>().sizeDelta.y);
		component.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(num, component.parent.GetComponent<RectTransform>().sizeDelta.y);
		component.sizeDelta = new Vector2(maxWidth, 108f);
		component.anchorMin = new Vector2(0f, 0.5f);
		component.anchorMax = new Vector2(0f, 0.5f);
		component.pivot = new Vector2(0f, 0.5f);
		component.GetComponent<CustomText>().alignment = TextAnchor.MiddleLeft;
		component.anchoredPosition = new Vector2(x + num2 * 2f, component.anchoredPosition.y);
		buttonRoot.transform.Find("Image").GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, 0f);
		return buttonRoot;
	}
}
