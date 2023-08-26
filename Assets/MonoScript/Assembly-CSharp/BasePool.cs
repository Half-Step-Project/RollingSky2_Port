using System.Collections.Generic;
using UnityEngine;

public abstract class BasePool<T> where T : Object
{
	public Dictionary<int, Queue<T>> m_opens = new Dictionary<int, Queue<T>>();

	public Dictionary<int, List<T>> m_closes = new Dictionary<int, List<T>>();

	public virtual T Acquire(int key)
	{
		T val = null;
		Queue<T> value = null;
		if (m_opens.TryGetValue(key, out value) && value != null && value.Count > 0)
		{
			val = value.Dequeue();
			List<T> value2 = null;
			if (m_closes.TryGetValue(key, out value2))
			{
				if (value2 == null)
				{
					value2 = new List<T>();
				}
				value2.Add(val);
				m_closes[key] = value2;
			}
			else
			{
				value2 = new List<T>();
				value2.Add(val);
				m_closes[key] = value2;
			}
		}
		return val;
	}

	public virtual void Retrieve(int key, T obj)
	{
		if ((Object)obj == (Object)null)
		{
			return;
		}
		List<T> value = null;
		if (m_closes.TryGetValue(key, out value) && value != null && value.Count > 0)
		{
			value.Remove(obj);
		}
		Queue<T> value2 = null;
		if (m_opens.TryGetValue(key, out value2))
		{
			if (value2 == null)
			{
				value2 = new Queue<T>();
			}
			value2.Enqueue(obj);
			m_opens[key] = value2;
		}
		else
		{
			value2 = new Queue<T>();
			value2.Enqueue(obj);
			m_opens[key] = value2;
		}
	}

	public virtual void RetrieveAll()
	{
		if (m_closes == null || m_closes.Count == 0)
		{
			return;
		}
		Dictionary<int, List<T>>.Enumerator enumerator = m_closes.GetEnumerator();
		Queue<T> value = new Queue<T>();
		while (enumerator.MoveNext())
		{
			value.Clear();
			m_opens.TryGetValue(enumerator.Current.Key, out value);
			if (value == null)
			{
				value = new Queue<T>();
			}
			for (int i = 0; i < enumerator.Current.Value.Count; i++)
			{
				value.Enqueue(enumerator.Current.Value[i]);
			}
			m_opens[enumerator.Current.Key] = value;
			enumerator.Current.Value.Clear();
		}
		m_closes.Clear();
	}

	public virtual void Clear()
	{
		RetrieveAll();
		Dictionary<int, Queue<T>>.Enumerator enumerator = m_opens.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Queue<T>.Enumerator enumerator2 = enumerator.Current.Value.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				Object.Destroy(enumerator2.Current);
			}
			enumerator.Current.Value.Clear();
		}
		m_opens.Clear();
	}
}
