using System.Runtime.CompilerServices;

namespace Foundation
{
	public struct PoolObjectInfo
	{
		[CompilerGenerated]
		private readonly string _003CName_003Ek__BackingField;

		[CompilerGenerated]
		private readonly bool _003CLocked_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CUseCount_003Ek__BackingField;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _003CName_003Ek__BackingField;
			}
		}

		public bool Locked
		{
			[CompilerGenerated]
			get
			{
				return _003CLocked_003Ek__BackingField;
			}
		}

		public int UseCount
		{
			[CompilerGenerated]
			get
			{
				return _003CUseCount_003Ek__BackingField;
			}
		}

		internal PoolObjectInfo(string name, bool locked, int useCount)
		{
			_003CName_003Ek__BackingField = name;
			_003CLocked_003Ek__BackingField = locked;
			_003CUseCount_003Ek__BackingField = useCount;
		}
	}
}
