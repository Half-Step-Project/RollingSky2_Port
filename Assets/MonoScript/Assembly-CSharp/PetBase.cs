using UnityEngine;

public abstract class PetBase : MonoBehaviour
{
	public Animator m_petAnimator;

	public PetSkeleton m_petSkeleton;

	public PetData m_petData;

	public PetState m_currentPetState;

	public PetUpdate m_onUpdate;

	public abstract void OnGameInitialization();

	public void SwitchPetState(PetState petState)
	{
		m_currentPetState = petState;
		OnSwitchPetState(petState);
	}

	protected abstract void OnSwitchPetState(PetState petState);

	public virtual void DestroyLocal()
	{
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		if (m_onUpdate != null)
		{
			m_onUpdate();
		}
	}

	public void OnEnable()
	{
		RegisterListener();
	}

	public void OnDisable()
	{
		UnregisterListener();
	}

	protected virtual void RegisterListener()
	{
	}

	protected virtual void UnregisterListener()
	{
	}

	public void TriggerUpMounts(UpMountsTrigger.TriggerData data)
	{
		if (this is IPetMounts)
		{
			((IPetMounts)this).UpMountsDuration = data.m_duration;
			SwitchPetState(PetState.UpMounts);
		}
	}

	public void TriggerDownMounts(DownMountsTrigger.TriggerData data)
	{
		if (this is IPetMounts)
		{
			((IPetMounts)this).DownMountsDuration = data.m_duration;
			SwitchPetState(PetState.DownMounts);
		}
	}

	public void TriggerPathToMove(PathToMoveByPetTrigger.PathToMoveByPetTriggerData data)
	{
		if (this is IPetPathToMove)
		{
			((IPetPathToMove)this).PathtoMoveData = data;
			SwitchPetState(PetState.PathToMove);
		}
	}
}
