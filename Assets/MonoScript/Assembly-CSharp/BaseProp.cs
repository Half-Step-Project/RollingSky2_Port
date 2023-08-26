using UnityEngine;

public abstract class BaseProp : MonoBehaviour
{
	public delegate void OnUpdate();

	public PropData m_propData;

	public OnUpdate m_update;

	public bool IsTriggering { get; set; }

	public abstract void OnInstance();

	public abstract void Add();

	public abstract void Remove();

	public abstract void OnTrigger();

	public void DestroyLocal()
	{
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		if (m_update != null)
		{
			m_update();
		}
	}
}
