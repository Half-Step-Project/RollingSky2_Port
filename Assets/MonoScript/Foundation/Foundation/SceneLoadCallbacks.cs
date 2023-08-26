using System.Runtime.CompilerServices;

namespace Foundation
{
	public struct SceneLoadCallbacks
	{
		[CompilerGenerated]
		private readonly SceneLoadSuccess _003CSuccess_003Ek__BackingField;

		[CompilerGenerated]
		private readonly SceneLoadFailure _003CFailure_003Ek__BackingField;

		[CompilerGenerated]
		private readonly SceneLoadUpdate _003CUpdate_003Ek__BackingField;

		[CompilerGenerated]
		private readonly SceneLoadDependencyAsset _003CDependency_003Ek__BackingField;

		public SceneLoadSuccess Success
		{
			[CompilerGenerated]
			get
			{
				return _003CSuccess_003Ek__BackingField;
			}
		}

		public SceneLoadFailure Failure
		{
			[CompilerGenerated]
			get
			{
				return _003CFailure_003Ek__BackingField;
			}
		}

		public SceneLoadUpdate Update
		{
			[CompilerGenerated]
			get
			{
				return _003CUpdate_003Ek__BackingField;
			}
		}

		public SceneLoadDependencyAsset Dependency
		{
			[CompilerGenerated]
			get
			{
				return _003CDependency_003Ek__BackingField;
			}
		}

		public SceneLoadCallbacks(SceneLoadSuccess success, SceneLoadFailure failure, SceneLoadUpdate update, SceneLoadDependencyAsset dependency)
		{
			_003CSuccess_003Ek__BackingField = success;
			_003CFailure_003Ek__BackingField = failure;
			_003CUpdate_003Ek__BackingField = update;
			_003CDependency_003Ek__BackingField = dependency;
		}
	}
}
