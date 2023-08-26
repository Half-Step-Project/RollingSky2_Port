using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[Serializable]
public class KeyedPriorityQueue<K, V, P> where V : class
{
	[Serializable]
	private struct HeapNode<KK, VV, PP>
	{
		public KK Key;

		public VV Value;

		public PP Priority;

		public HeapNode(KK key, VV value, PP priority)
		{
			Key = key;
			Value = value;
			Priority = priority;
		}
	}

	private List<HeapNode<K, V, P>> heap = new List<HeapNode<K, V, P>>();

	private int size;

	private Comparer<P> priorityComparer = Comparer<P>.Default;

	private HeapNode<K, V, P> placeHolder;

	public int Count
	{
		get
		{
			return size;
		}
	}

	public ReadOnlyCollection<V> Values
	{
		get
		{
			List<V> list = new List<V>();
			for (int i = 1; i <= size; i++)
			{
				list.Add(heap[i].Value);
			}
			return new ReadOnlyCollection<V>(list);
		}
	}

	public ReadOnlyCollection<K> Keys
	{
		get
		{
			List<K> list = new List<K>();
			for (int i = 1; i <= size; i++)
			{
				list.Add(heap[i].Key);
			}
			return new ReadOnlyCollection<K>(list);
		}
	}

	public event EventHandler<KeyedPriorityQueueHeadChangedEventArgs<V>> FirstElementChanged;

	public KeyedPriorityQueue()
	{
		heap.Add(default(HeapNode<K, V, P>));
	}

	public void Enqueue(K key, V value, P priority)
	{
		V val = ((size > 0) ? heap[1].Value : null);
		int num = ++size;
		int num2 = num / 2;
		if (num == heap.Count)
		{
			heap.Add(placeHolder);
		}
		while (num > 1 && IsHigher(priority, heap[num2].Priority))
		{
			heap[num] = heap[num2];
			num = num2;
			num2 = num / 2;
		}
		heap[num] = new HeapNode<K, V, P>(key, value, priority);
		V value2 = heap[1].Value;
		if (!value2.Equals(val))
		{
			RaiseHeadChangedEvent(val, value2);
		}
	}

	public V Dequeue()
	{
		V result = ((size < 1) ? null : DequeueImpl());
		V newHead = ((size < 1) ? null : heap[1].Value);
		RaiseHeadChangedEvent(null, newHead);
		return result;
	}

	private V DequeueImpl()
	{
		V value = heap[1].Value;
		heap[1] = heap[size];
		heap[size--] = placeHolder;
		Heapify(1);
		return value;
	}

	public V Remove(K key)
	{
		if (size < 1)
		{
			return null;
		}
		V value = heap[1].Value;
		for (int i = 1; i <= size; i++)
		{
			if (heap[i].Key.Equals(key))
			{
				V value2 = heap[i].Value;
				Swap(i, size);
				heap[size--] = placeHolder;
				Heapify(i);
				V value3 = heap[1].Value;
				if (!value.Equals(value3))
				{
					RaiseHeadChangedEvent(value, value3);
				}
				return value2;
			}
		}
		return null;
	}

	public V Get(K key)
	{
		if (size < 1)
		{
			return null;
		}
		HeapNode<K, V, P> heapNode = heap[1];
		for (int i = 1; i <= size; i++)
		{
			if (heap[i].Key.Equals(key))
			{
				return heap[i].Value;
			}
		}
		return null;
	}

	public V Peek()
	{
		if (size >= 1)
		{
			return heap[1].Value;
		}
		return null;
	}

	public V FindByPriority(P priority, Predicate<V> match)
	{
		if (size >= 1)
		{
			return Search(priority, 1, match);
		}
		return null;
	}

	public void Clear()
	{
		heap.Clear();
		size = 0;
	}

	private void RaiseHeadChangedEvent(V oldHead, V newHead)
	{
		if (oldHead != newHead)
		{
			EventHandler<KeyedPriorityQueueHeadChangedEventArgs<V>> firstElementChanged = this.FirstElementChanged;
			if (firstElementChanged != null)
			{
				firstElementChanged(this, new KeyedPriorityQueueHeadChangedEventArgs<V>(oldHead, newHead));
			}
		}
	}

	private V Search(P priority, int i, Predicate<V> match)
	{
		V val = null;
		if (IsHigher(heap[i].Priority, priority))
		{
			if (match(heap[i].Value))
			{
				val = heap[i].Value;
			}
			int num = 2 * i;
			int num2 = num + 1;
			if (val == null && num <= size)
			{
				val = Search(priority, num, match);
			}
			if (val == null && num2 <= size)
			{
				val = Search(priority, num2, match);
			}
		}
		return val;
	}

	private void Heapify(int i)
	{
		int num = 2 * i;
		int num2 = num + 1;
		int num3 = i;
		if (num <= size && IsHigher(heap[num].Priority, heap[i].Priority))
		{
			num3 = num;
		}
		if (num2 <= size && IsHigher(heap[num2].Priority, heap[num3].Priority))
		{
			num3 = num2;
		}
		if (num3 != i)
		{
			Swap(i, num3);
			Heapify(num3);
		}
	}

	private void Swap(int i, int j)
	{
		HeapNode<K, V, P> value = heap[i];
		heap[i] = heap[j];
		heap[j] = value;
	}

	protected virtual bool IsHigher(P p1, P p2)
	{
		return priorityComparer.Compare(p1, p2) < 1;
	}
}
