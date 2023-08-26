using UnityEngine;

public class CollisionListen : MonoBehaviour
{
	public delegate void OnCollisionEnterEvent(Collision collision);

	public delegate void OnTrightEnterEvent(Collider collider);

	public OnCollisionEnterEvent m_onCollisionEnter;

	public OnTrightEnterEvent m_onTriggerEnter;

	public static CollisionListen Builder(GameObject obj)
	{
		CollisionListen collisionListen = obj.GetComponent<CollisionListen>();
		if (collisionListen == null)
		{
			collisionListen = obj.AddComponent<CollisionListen>();
		}
		return collisionListen;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (m_onCollisionEnter != null)
		{
			m_onCollisionEnter(collision);
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (m_onTriggerEnter != null)
		{
			m_onTriggerEnter(collider);
		}
	}

	public void DestroyLocal(float time = 0f)
	{
		Object.Destroy(this, time);
	}
}
