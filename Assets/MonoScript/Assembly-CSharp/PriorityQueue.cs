using System;
using System.Collections.Generic;

internal class PriorityQueue<T>
{
	private IComparer<T> comparer;

	private T[] heap;

	public int Count { get; private set; }

	public PriorityQueue()
		: this((IComparer<T>)null)
	{
	}

	public PriorityQueue(int capacity)
		: this(capacity, (IComparer<T>)null)
	{
	}

	public PriorityQueue(IComparer<T> comparer)
		: this(16, comparer)
	{
	}

	public PriorityQueue(int capacity, IComparer<T> comparer)
	{
		IComparer<T> obj;
		if (comparer != null)
		{
			obj = comparer;
		}
		else
		{
			IComparer<T> @default = Comparer<T>.Default;
			obj = @default;
		}
		this.comparer = obj;
		heap = new T[capacity];
	}

	public void Push(T v)
	{
		if (Count >= heap.Length)
		{
			Array.Resize(ref heap, Count * 2);
		}
		heap[Count] = v;
		SiftUp(Count++);
	}

	public T Pop()
	{
		if (Count == 0)
		{
			return default(T);
		}
		T result = Top();
		heap[0] = heap[--Count];
		if (Count > 0)
		{
			SiftDown(0);
		}
		return result;
	}

	public T Top()
	{
		if (Count > 0)
		{
			return heap[0];
		}
		return default(T);
	}

	private void SiftUp(int n)
	{
		T val = heap[n];
		int num = n / 2;
		while (n > 0 && comparer.Compare(val, heap[num]) > 0)
		{
			heap[n] = heap[num];
			n = num;
			num /= 2;
		}
		heap[n] = val;
	}

	private void SiftDown(int n)
	{
		T val = heap[n];
		for (int num = n * 2; num < Count; num *= 2)
		{
			if (num + 1 < Count && comparer.Compare(heap[num + 1], heap[num]) > 0)
			{
				num++;
			}
			if (comparer.Compare(val, heap[num]) >= 0)
			{
				break;
			}
			heap[n] = heap[num];
			n = num;
		}
		heap[n] = val;
	}
}
