using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foundation
{
	public struct AssetDependencyInfo
	{
		[CompilerGenerated]
		private readonly ResourceName _003CAssetName_003Ek__BackingField;

		[CompilerGenerated]
		private readonly List<ResourceName> _003CDependencyAssetNames_003Ek__BackingField;

		[CompilerGenerated]
		private readonly List<ResourceName> _003CScatteredDependencyAssetNames_003Ek__BackingField;

		public ResourceName AssetName
		{
			[CompilerGenerated]
			get
			{
				return _003CAssetName_003Ek__BackingField;
			}
		}

		public List<ResourceName> DependencyAssetNames
		{
			[CompilerGenerated]
			get
			{
				return _003CDependencyAssetNames_003Ek__BackingField;
			}
		}

		public List<ResourceName> ScatteredDependencyAssetNames
		{
			[CompilerGenerated]
			get
			{
				return _003CScatteredDependencyAssetNames_003Ek__BackingField;
			}
		}

		public AssetDependencyInfo(ResourceName assetName, List<ResourceName> dependencyAssetNames, List<ResourceName> scatteredDependencyAssetNames)
		{
			_003CAssetName_003Ek__BackingField = assetName;
			_003CDependencyAssetNames_003Ek__BackingField = dependencyAssetNames;
			_003CScatteredDependencyAssetNames_003Ek__BackingField = scatteredDependencyAssetNames;
		}
	}
}
