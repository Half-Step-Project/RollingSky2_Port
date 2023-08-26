using System.Runtime.CompilerServices;

namespace Foundation
{
	public struct BundleInfo
	{
		[CompilerGenerated]
		private readonly BundleName _003CBundleName_003Ek__BackingField;

		[CompilerGenerated]
		private readonly int _003CLength_003Ek__BackingField;

		[CompilerGenerated]
		private readonly uint _003CCrc_003Ek__BackingField;

		[CompilerGenerated]
		private readonly bool _003CStorageInReadOnly_003Ek__BackingField;

		public BundleName BundleName
		{
			[CompilerGenerated]
			get
			{
				return _003CBundleName_003Ek__BackingField;
			}
		}

		public int Length
		{
			[CompilerGenerated]
			get
			{
				return _003CLength_003Ek__BackingField;
			}
		}

		public uint Crc
		{
			[CompilerGenerated]
			get
			{
				return _003CCrc_003Ek__BackingField;
			}
		}

		public bool StorageInReadOnly
		{
			[CompilerGenerated]
			get
			{
				return _003CStorageInReadOnly_003Ek__BackingField;
			}
		}

		public BundleInfo(BundleName bundleName, int length, uint crc, bool storageInReadOnly)
		{
			_003CBundleName_003Ek__BackingField = bundleName;
			_003CLength_003Ek__BackingField = length;
			_003CCrc_003Ek__BackingField = crc;
			_003CStorageInReadOnly_003Ek__BackingField = storageInReadOnly;
		}
	}
}
