using System.Runtime.CompilerServices;

namespace Foundation
{
	public struct AssetLoadCallbacks
	{
		[CompilerGenerated]
		private readonly AssetLoadSuccess _003CSuccess_003Ek__BackingField;

		[CompilerGenerated]
		private readonly AssetLoadFailure _003CFailure_003Ek__BackingField;

		[CompilerGenerated]
		private readonly AssetLoadUpdate _003CUpdate_003Ek__BackingField;

		[CompilerGenerated]
		private readonly AssetLoadDependency _003CDependency_003Ek__BackingField;

		public AssetLoadSuccess Success
		{
			[CompilerGenerated]
			get
			{
				return _003CSuccess_003Ek__BackingField;
			}
		}

		public AssetLoadFailure Failure
		{
			[CompilerGenerated]
			get
			{
				return _003CFailure_003Ek__BackingField;
			}
		}

		public AssetLoadUpdate Update
		{
			[CompilerGenerated]
			get
			{
				return _003CUpdate_003Ek__BackingField;
			}
		}

		public AssetLoadDependency Dependency
		{
			[CompilerGenerated]
			get
			{
				return _003CDependency_003Ek__BackingField;
			}
		}

		public AssetLoadCallbacks(AssetLoadSuccess success, AssetLoadFailure failure, AssetLoadUpdate update = null, AssetLoadDependency dependency = null)
		{
			_003CSuccess_003Ek__BackingField = success;
			_003CFailure_003Ek__BackingField = failure;
			_003CUpdate_003Ek__BackingField = update;
			_003CDependency_003Ek__BackingField = dependency;
		}
	}
}
