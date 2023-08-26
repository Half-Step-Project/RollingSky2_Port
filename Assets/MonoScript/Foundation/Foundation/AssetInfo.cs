using System.Runtime.CompilerServices;

namespace Foundation
{
	public struct AssetInfo
	{
		[CompilerGenerated]
		private readonly ResourceName _003CAssetName_003Ek__BackingField;

		[CompilerGenerated]
		private readonly BundleName _003CBundleName_003Ek__BackingField;

		public ResourceName AssetName
		{
			[CompilerGenerated]
			get
			{
				return _003CAssetName_003Ek__BackingField;
			}
		}

		public BundleName BundleName
		{
			[CompilerGenerated]
			get
			{
				return _003CBundleName_003Ek__BackingField;
			}
		}

		public AssetInfo(ResourceName assetName, BundleName bundleName)
		{
			_003CAssetName_003Ek__BackingField = assetName;
			_003CBundleName_003Ek__BackingField = bundleName;
		}
	}
}
