using System;
using System.Runtime.CompilerServices;

namespace Foundation
{
	public struct ReferenceInfo
	{
		[CompilerGenerated]
		private readonly Type _003CType_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CUnusedCount_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CUsingCount_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CAcquiredCount_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CRecycledCount_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CAddedCount_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CRemovedCount_003Ek__BackingField;

		public Type Type
		{
			[CompilerGenerated]
			get
			{
				return _003CType_003Ek__BackingField;
			}
		}

		public int UnusedCount
		{
			[CompilerGenerated]
			get
			{
				return _003CUnusedCount_003Ek__BackingField;
			}
		}

		public int UsingCount
		{
			[CompilerGenerated]
			get
			{
				return _003CUsingCount_003Ek__BackingField;
			}
		}

		public int AcquiredCount
		{
			[CompilerGenerated]
			get
			{
				return _003CAcquiredCount_003Ek__BackingField;
			}
		}

		public int RecycledCount
		{
			[CompilerGenerated]
			get
			{
				return _003CRecycledCount_003Ek__BackingField;
			}
		}

		public int AddedCount
		{
			[CompilerGenerated]
			get
			{
				return _003CAddedCount_003Ek__BackingField;
			}
		}

		public int RemovedCount
		{
			[CompilerGenerated]
			get
			{
				return _003CRemovedCount_003Ek__BackingField;
			}
		}

		internal ReferenceInfo(Type type, int unusedCount, int usingCount, int acquiredCount, int recycledCount, int addedCount, int removedCount)
		{
			_003CType_003Ek__BackingField = type;
			_003CUnusedCount_003Ek__BackingField = unusedCount;
			_003CUsingCount_003Ek__BackingField = usingCount;
			_003CAcquiredCount_003Ek__BackingField = acquiredCount;
			_003CRecycledCount_003Ek__BackingField = recycledCount;
			_003CAddedCount_003Ek__BackingField = addedCount;
			_003CRemovedCount_003Ek__BackingField = removedCount;
		}
	}
}
