using System.Collections;
using UnityEngine;

public sealed class CoroutineScript : MonoBehaviour
{
	public CoroutineManagerType m_type;

	public Coroutine CreateCoroutine(CoroutineManagerType type, IEnumerator routine)
	{
		m_type = type;
		return StartCoroutine(routine);
	}

	public void DestroyCoroutine()
	{
		StopAllCoroutines();
		Object.Destroy(this);
	}
}
