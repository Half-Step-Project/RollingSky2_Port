using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : IPoolObj, new()
{
	private readonly Stack<T> m_Stack = new Stack<T>();

	private static ObjectPool<T> instance;

	public int AllCount { get; private set; }

	public static ObjectPool<T> Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new ObjectPool<T>();
			}
			return instance;
		}
	}

	public int InActiveCount
	{
		get
		{
			return m_Stack.Count;
		}
	}

	public int ActiveCount
	{
		get
		{
			return AllCount - InActiveCount;
		}
	}

	private ObjectPool()
	{
	}

	public T Get()
	{
		T result;
		if (m_Stack.Count <= 0)
		{
			result = new T();
			AllCount++;
		}
		else
		{
			result = m_Stack.Pop();
		}
		result.Init();
		return result;
	}

	public void Release(T t)
	{
		if (m_Stack.Count > 0 && (object)m_Stack.Peek() == (object)t)
		{
			Debug.LogError("Try to release Obj that had been released!");
		}
		if (t != null)
		{
			t.Release();
		}
		m_Stack.Push(t);
	}

	public void Clear()
	{
		m_Stack.Clear();
	}
}
