using UnityEngine;

namespace RS2
{
	public class AnimationEventReceiver : MonoBehaviour
	{
		public void OnUpgrade1Finish()
		{
			base.transform.parent.parent.GetComponent<EducationSpriteAnimator>().OnUpgrade1Finish();
		}
	}
}
