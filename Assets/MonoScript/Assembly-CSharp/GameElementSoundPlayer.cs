using UnityEngine;

public class GameElementSoundPlayer : MonoBehaviour
{
	public static readonly string PlaySoundFunctionName = "PlaySound";

	public BaseElement gameElement;

	public void RegistAudioEvent(AnimationClip animClip, float time)
	{
		if ((bool)animClip && (animClip.events == null || animClip.events.Length == 0))
		{
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.functionName = PlaySoundFunctionName;
			animationEvent.time = time;
			animClip.AddEvent(animationEvent);
		}
	}

	public void UnRegistAudioEvent(AnimationClip animClip)
	{
		if ((bool)animClip)
		{
			animClip.events = null;
		}
	}

	public void PlaySound()
	{
		if ((bool)gameElement)
		{
			gameElement.PlaySoundEffect();
		}
		else
		{
			Debug.LogError("Can't find the gameElement");
		}
	}
}
