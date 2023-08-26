using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CoroutineManager
{
	private static GameObject m_target = null;

	private static Dictionary<int, CoroutineScript> m_dic = new Dictionary<int, CoroutineScript>();

	public static void CreateCoroutine(CoroutineManagerType index, IEnumerator enumerator)
	{
		if (m_target == null)
		{
			m_target = new GameObject("CoroutineManager");
			Object.DontDestroyOnLoad(m_target);
		}
		CoroutineScript value;
		if (!m_dic.TryGetValue((int)index, out value))
		{
			CoroutineScript coroutineScript = m_target.gameObject.AddComponent<CoroutineScript>();
			coroutineScript.transform.parent = m_target.transform;
			m_dic.Add((int)index, coroutineScript);
			value = coroutineScript;
		}
		value.CreateCoroutine(index, enumerator);
	}

	public static void DestroyCoroutine(CoroutineManagerType index)
	{
		CoroutineScript value = null;
		if (m_dic.TryGetValue((int)index, out value))
		{
			value.DestroyCoroutine();
			m_dic.Remove((int)index);
		}
	}

	public static void DestroyCoroutine(CoroutineManagerType[] indexs)
	{
		for (int i = 0; i < indexs.Length; i++)
		{
			DestroyCoroutine(indexs[i]);
		}
	}

	public static void DestroyAllCoroutine()
	{
		Dictionary<int, CoroutineScript>.Enumerator enumerator = m_dic.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.DestroyCoroutine();
		}
		m_dic.Clear();
	}
}
