using UnityEngine;
using UnityEngine.UI;

public class RedPointBehaviour : MonoBehaviour
{
	public delegate bool IsNeedHandler();

	public GameObject mTarget;

	public Text mText;

	public float mDuration = 1f;

	public bool mTimeScale = true;

	private float mTime;

	public IsNeedHandler mIsNeedHandler;

	public bool mCurrentState;

	private void Start()
	{
		OnRefresh();
	}

	private void Update()
	{
		if (!(mTarget == null) && mIsNeedHandler != null)
		{
			mTime += (mTimeScale ? Time.deltaTime : Time.unscaledDeltaTime);
			if (mTime >= mDuration)
			{
				OnRefresh();
				mTime = 0f;
			}
		}
	}

	public void OnRefresh()
	{
		if (mIsNeedHandler != null)
		{
			bool active = (mCurrentState = mIsNeedHandler());
			if (mTarget != null)
			{
				mTarget.SetActive(active);
			}
		}
		else if (mTarget != null)
		{
			mTarget.SetActive(false);
		}
	}

	public void SetRedPointTxt(string info)
	{
		if ((bool)mText)
		{
			mText.text = info;
		}
	}
}
