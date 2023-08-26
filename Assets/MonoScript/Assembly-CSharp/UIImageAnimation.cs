using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageAnimation : MonoBehaviour
{
	[SerializeField]
	protected Image mImageRoot;

	[SerializeField]
	protected int mFPS = 30;

	[SerializeField]
	protected bool mLoop = true;

	[SerializeField]
	protected bool mSnap = true;

	protected float mDelta;

	protected int mIndex;

	protected bool mActive = true;

	[SerializeField]
	public List<Sprite> mSpriteNames = new List<Sprite>();

	public int frames
	{
		get
		{
			return mSpriteNames.Count;
		}
	}

	public int framesPerSecond
	{
		get
		{
			return mFPS;
		}
		set
		{
			mFPS = value;
		}
	}

	public bool loop
	{
		get
		{
			return mLoop;
		}
		set
		{
			mLoop = value;
		}
	}

	public bool isPlaying
	{
		get
		{
			return mActive;
		}
	}

	protected virtual void Start()
	{
	}

	private void Update()
	{
		if (!mActive || mSpriteNames.Count <= 1 || !Application.isPlaying || mFPS <= 0)
		{
			return;
		}
		mDelta += Time.unscaledDeltaTime;
		float num = 1f / (float)mFPS;
		if (num < mDelta)
		{
			mDelta = ((num > 0f) ? (mDelta - num) : 0f);
			mIndex++;
			if (mIndex >= mSpriteNames.Count)
			{
				mIndex = 0;
				mActive = mLoop;
			}
			if (mActive)
			{
				mImageRoot.sprite = mSpriteNames[mIndex];
				mDelta = 0f;
			}
		}
	}
}
