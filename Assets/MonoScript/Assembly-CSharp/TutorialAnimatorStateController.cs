using Foundation;
using RS2;
using UnityEngine;

public class TutorialAnimatorStateController : StateMachineBehaviour
{
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.IsName("Role_ToIn"))
		{
			Mod.Event.Fire(this, Mod.Reference.Acquire<TrigerTutorialStepEventArgs>().Initialize(TutorialStepId.STAGE_HOME_MENU_STEP_0));
		}
	}
}
