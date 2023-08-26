using Foundation;
using UnityEngine;

namespace RS2
{
	public class EducationAnimator : MonoBehaviour
	{
		public Animator m_Animator;

		private void Awake()
		{
			if (base.name == "Series")
			{
				base.gameObject.transform.GetChild(0).gameObject.SetActive(false);
			}
		}

		public void PlayAnim(string trigger)
		{
			if (base.name != "Series")
			{
				m_Animator.SetTrigger(trigger);
				Log.Info(string.Format("**********{0}----->{1}", base.name, trigger.ToString()));
			}
		}
	}
}
