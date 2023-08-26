using System.Collections.Generic;

namespace Foundation
{
	public sealed class ReferencePool
	{
		private readonly List<IReference> _recycles = new List<IReference>();

		public int UnusedCount
		{
			get
			{
				return _recycles.Count;
			}
		}

		public int UsingCount { get; private set; }

		public int AcquiredCount { get; private set; }

		public int RecycledCount { get; private set; }

		public int AddedCount { get; private set; }

		public int RemovedCount { get; private set; }

		public T Acquire<T>() where T : class, IReference, new()
		{
			UsingCount++;
			AcquiredCount++;
			if (_recycles.Count > 0)
			{
				return (T)_recycles.RemoveLast();
			}
			AddedCount++;
			return new T();
		}

		public void Recycle<T>(T reference) where T : class, IReference
		{
			if (reference != null && !_recycles.Contains(reference))
			{
				reference.OnRecycle();
				_recycles.Add(reference);
				RecycledCount++;
				UsingCount--;
			}
		}

		public void Add<T>(int count) where T : class, IReference, new()
		{
			if (count > 0)
			{
				AddedCount += count;
				while (count-- > 0)
				{
					_recycles.Add(new T());
				}
			}
		}

		public void Remove(int count)
		{
			if (count > 0)
			{
				if (count > _recycles.Count)
				{
					count = _recycles.Count;
				}
				RemovedCount += count;
				while (count-- > 0)
				{
					_recycles.RemoveLast();
				}
			}
		}

		public void RemoveAll()
		{
			RemovedCount += _recycles.Count;
			_recycles.Clear();
		}
	}
}
