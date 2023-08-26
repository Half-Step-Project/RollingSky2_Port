using System.Collections;
using Foundation;
using RS2;
using UnityEngine;

public class TutorialVideoController : MonoBehaviour
{
	public GameObject mResumeButton;

	public GameObject mWindow;

	public Camera mOffscreenCamera;

	public RenderTexture mRenderTexture;

	public GameObject mAnimController;

	public GameObject mAnimSixAsix;

	public GameObject mAnimMouse;

	public GameObject mSwitchButton;

	public GameObject mTitel;

	public GameObject m_UI;

	private bool isStart;

	private bool isOnShow;

	private float mDuration = 4f;

	[SerializeField]
	[Label]
	private float mCurrentTime;

	[SerializeField]
	[Label]
	private bool mIsShowBesumeButton;

	[SerializeField]
	[Label]
	private bool mIsShowWindow;

	private float mShowWindowDuration = 0.25f;

	private Vector3 mFrom = Vector3.zero;

	private Vector3 mTo = Vector3.one;

	private void OnEnable()
	{
		if (mResumeButton == null)
		{
			Transform transform = base.transform.Find("UI/FIRST/TutorialUI/window/resumeBtn");
			if (transform != null)
			{
				mResumeButton = transform.gameObject;
			}
		}
		if (mWindow == null)
		{
			Transform transform2 = base.transform.Find("UI/FIRST/TutorialUI/window");
			if (transform2 != null)
			{
				mWindow = transform2.gameObject;
			}
		}
		if ((bool)mResumeButton)
		{
			mResumeButton.SetActive(false);
			EventTriggerListener.Get(mResumeButton).onClick = OnClickBesumeButton;
		}
		if ((bool)mWindow)
		{
			mWindow.transform.localScale = mFrom;
		}
		if ((bool)mOffscreenCamera)
		{
			mOffscreenCamera.targetTexture = mRenderTexture;
		}
		mCurrentTime = 0f;
		mIsShowBesumeButton = false;
		mIsShowWindow = true;
		mSwitchButton.SetActive(false);
		mAnimMouse.SetActive(true);
		mAnimMouse.GetComponent<Animator>().Play("MouseAnim");
		mTitel.SetActive(true);
		mAnimController.SetActive(false);
		mAnimSixAsix.SetActive(false);
	}

	private void Start()
	{
		isStart = false;
		Mod.Event.Subscribe(EventArgs<TutorialResultEventArgs>.EventId, ShowResult);
	}

	private void ShowResult(object sender, EventArgs e)
	{
		ShowResultUI();
	}

	private void CloseResult()
	{
		CloseResultUI();
	}

	private void Update()
	{
		if (isOnShow && InputService.KeyDown_A && mResumeButton.activeSelf)
		{
			EventTriggerListener.Get(mResumeButton).onClick(null);
		}
		if (isStart)
		{
			return;
		}
		if (mIsShowWindow)
		{
			mCurrentTime += Time.unscaledDeltaTime;
			float num = mCurrentTime / mShowWindowDuration;
			if (num >= 1f)
			{
				num = 1f;
				mCurrentTime = 0f;
				mIsShowWindow = false;
			}
			if (mWindow != null)
			{
				mWindow.transform.localScale = Vector3.Lerp(mFrom, mTo, num);
			}
		}
		if (mIsShowBesumeButton || mIsShowWindow)
		{
			return;
		}
		mCurrentTime += Time.unscaledDeltaTime;
		if (mCurrentTime >= mDuration)
		{
			if ((bool)mResumeButton)
			{
				mResumeButton.SetActive(true);
				isOnShow = true;
			}
			mCurrentTime = 0f;
			mIsShowBesumeButton = true;
		}
	}

	public void OnClickBesumeButton(GameObject obj)
	{
		if (isStart)
		{
			StartCoroutine(TutorialStartGame());
			return;
		}
		isStart = true;
		Mod.Event.FireNow(this, Mod.Reference.Acquire<GameResumeEventArgs>());
		Mod.Event.Fire(this, Mod.Reference.Acquire<OpenTutorialVideoEventArgs>());
		m_UI.SetActive(false);
	}

	private void ShowResultUI()
	{
		m_UI.SetActive(true);
		isOnShow = true;
		mWindow.transform.localScale = Vector3.one;
		mResumeButton.SetActive(true);
		mSwitchButton.SetActive(true);
	}

	private void CloseResultUI()
	{
		m_UI.SetActive(false);
		isOnShow = false;
	}

	public void OnSwitchController(GameObject obj)
	{
		InputController.hasSixAsix = !InputController.hasSixAsix;
		mAnimSixAsix.SetActive(InputController.hasSixAsix);
		mAnimController.SetActive(!InputController.hasSixAsix);
	}

	private IEnumerator TutorialStartGame()
	{
		CloseResultUI();
		yield return new WaitForSeconds(0.1f);
		Mod.Event.FireNow(this, Mod.Reference.Acquire<ChangeRoleTailEffectStateArgs>().Initialize(TailEffectState.IMMEDIATELY_CLOSE));
		Mod.Event.Fire(this, Mod.Reference.Acquire<GameOriginRebirthResetEventArgs>());
		Mod.Event.Fire(this, Mod.Reference.Acquire<PropsRemoveAllEventArgs>().Initialize());
		yield return new WaitForSeconds(0.1f);
		Mod.Event.Fire(this, Mod.Reference.Acquire<ClickGameStartButtonEventArgs>().Initialize());
	}

	public void OnDisable()
	{
		if ((bool)mOffscreenCamera)
		{
			mOffscreenCamera.targetTexture = null;
		}
	}

	public void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<TutorialResultEventArgs>.EventId, ShowResult);
		if ((bool)mOffscreenCamera)
		{
			mOffscreenCamera.targetTexture = null;
		}
		if ((bool)mRenderTexture)
		{
			mRenderTexture.Release();
			mRenderTexture = null;
		}
	}
}
