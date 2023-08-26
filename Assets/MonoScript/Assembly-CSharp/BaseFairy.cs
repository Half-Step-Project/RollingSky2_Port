using UnityEngine;

public abstract class BaseFairy : MonoBehaviour
{
	[Label]
	public FairyState mState;

	public FairyData mData;

	public void OnInstantiate()
	{
		OnInst();
	}

	public void SwitchState(FairyState state)
	{
		mState = state;
		OnSwitchState();
	}

	public void UpdateState()
	{
		OnUpdateState();
	}

	public abstract void PlaySkill(bool active, params FairySkillsName[] skillsName);

	public abstract bool IsSkillPlaying();

	protected abstract void OnInst();

	protected abstract void OnSwitchState();

	protected abstract void OnUpdateState();

	public virtual void DestroyLocal()
	{
		Object.Destroy(base.gameObject);
	}
}
