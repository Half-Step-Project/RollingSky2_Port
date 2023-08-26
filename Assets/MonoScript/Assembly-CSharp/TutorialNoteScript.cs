using Foundation;
using RS2;
using UnityEngine;

public class TutorialNoteScript : MonoBehaviour
{
	public Animator mAnimator;

	private void Start()
	{
		Mod.Event.Subscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, HandleClickGameStart);
	}

	private void OnDestroy()
	{
		Mod.Event.Unsubscribe(EventArgs<ClickGameStartButtonEventArgs>.EventId, HandleClickGameStart);
	}

	public void PlayReady()
	{
		if ((bool)mAnimator)
		{
			mAnimator.SetTrigger("Ready");
		}
	}

	private void PlayReadGo()
	{
		if ((bool)mAnimator)
		{
			mAnimator.SetTrigger("ReadyGo");
		}
	}

	public void AnimationEvent(string info)
	{
		Mod.Event.Fire(this, Mod.Reference.Acquire<GameStartButtonActiveEventArgs>().Initialize(true));
	}

	private void HandleClickGameStart(object sender, EventArgs e)
	{
		if (e is ClickGameStartButtonEventArgs)
		{
			PlayReadGo();
		}
	}
}
