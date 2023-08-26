using Foundation;
using RS2;
using UnityEngine;

public class HomeAnimationController : MonoBehaviour
{
	public Animator m_gloableAnimator;

	private AnimatorStateInfo m_currentAnimatorState;

	private int m_nextState = -1;

	private string m_nextTrigger = "";

	private const float SWITCH_THRESHOLD = 0.95f;

	private void Update()
	{
	}

	public void PlayAnimation(int levelId, bool isUnLock)
	{
		PlayerLocalLevelData playerLevelData = PlayerDataModule.Instance.GetPlayerLevelData(levelId);
		if (isUnLock)
		{
			PlayUnLockAnimationById(levelId);
		}
		else if (playerLevelData.LockState > 0)
		{
			PlayLockAnimation();
		}
		else
		{
			PlayIdleAnimationById(levelId);
		}
	}

	private void PlayLockAnimation()
	{
		string trigger = GenerateTrigerByState(0);
		m_gloableAnimator.SetTrigger(trigger);
		m_nextState = -1;
		m_nextTrigger = "";
	}

	private void PlayIdleAnimationById(int levelId)
	{
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		string text = GenerateTrigerByState(levels_levelTable.MusicAnimationID);
		m_currentAnimatorState = m_gloableAnimator.GetCurrentAnimatorStateInfo(0);
		if (m_currentAnimatorState.shortNameHash == GenerateAnimationStateName(0))
		{
			m_gloableAnimator.SetTrigger(text);
			m_nextState = -1;
		}
		else
		{
			m_nextState = GenerateAnimationStateName(levels_levelTable.MusicAnimationID);
			m_nextTrigger = text;
		}
	}

	private void PlayUnLockAnimationById(int levelId)
	{
		string trigger = GenerateTrigerByState(-1);
		m_gloableAnimator.SetTrigger(trigger);
		Levels_levelTable levels_levelTable = Mod.DataTable.Get<Levels_levelTable>()[levelId];
		string nextTrigger = GenerateTrigerByState(levels_levelTable.MusicAnimationID);
		m_nextState = GenerateAnimationStateName(levels_levelTable.MusicAnimationID);
		m_nextTrigger = nextTrigger;
	}

	private string GenerateTrigerByState(int state)
	{
		return string.Format("State_{0}", state);
	}

	private int GenerateAnimationStateName(int state)
	{
		switch (state)
		{
		case 0:
			return Animator.StringToHash("UI_Lock");
		case -1:
			return Animator.StringToHash("UI_UnLock");
		default:
			return Animator.StringToHash(string.Format("UI_Idle_{0}", state));
		}
	}
}
