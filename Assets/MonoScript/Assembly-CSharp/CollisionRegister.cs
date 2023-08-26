using UnityEngine;

public class CollisionRegister : MonoBehaviour
{
	public delegate void CallBackCollider(Collider collider);

	public delegate void CallBackCollision(Collision collision);

	public CallBackCollider m_callBackCollider;

	public CallBackCollision m_callBackCollision;

	private bool m_isListen = true;

	public bool IsListen
	{
		get
		{
			return m_isListen;
		}
		set
		{
			m_isListen = value;
		}
	}

	public static CollisionRegister Builder(GameObject targetObj)
	{
		return targetObj.AddComponent<CollisionRegister>();
	}

	public void DispatchEvent(Collider collider)
	{
		if (IsListen && m_callBackCollider != null)
		{
			m_callBackCollider(collider);
		}
	}

	public void DispatchEvent(Collision collision)
	{
		if (IsListen && m_callBackCollision != null)
		{
			m_callBackCollision(collision);
		}
	}
}
