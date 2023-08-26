using UnityEngine;

public class AnimationEventListen : MonoBehaviour
{
	public delegate void StringDelegate(GameObject obj, string info);

	public GameObject m_obj;

	public StringDelegate m_onStringDelegate;

	public void AnimationEvent(string info)
	{
		if (m_onStringDelegate != null)
		{
			m_onStringDelegate(base.gameObject, info);
		}
	}

	public static AnimationEventListen Get(GameObject obj)
	{
		AnimationEventListen animationEventListen = obj.GetComponent<AnimationEventListen>();
		if (animationEventListen == null)
		{
			animationEventListen = obj.AddComponent<AnimationEventListen>();
		}
		return animationEventListen;
	}
}
