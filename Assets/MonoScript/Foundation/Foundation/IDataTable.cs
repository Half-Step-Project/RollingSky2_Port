using System;

namespace Foundation
{
	public interface IDataTable
	{
		Type RecordType { get; }

		IRecord[] Records { get; }

		int Count { get; }

		bool Contains(int id);

		int Add(byte[] bytes, int position);

		void RemoveAll();
	}
	public interface IDataTable<T> : IDataTable where T : IRecord
	{
		T this[int id] { get; }

		T Min { get; }

		T Max { get; }

		new T[] Records { get; }

		bool Contains(Predicate<T> condition);

		T Get(int id);

		T Get(Predicate<T> condition);

		T[] Filter(Predicate<T> condition);

		T[] Filter(Predicate<T> condition, Comparison<T> comparison);

		T[] Sort(Comparison<T> comparison);
	}
}
