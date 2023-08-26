using System.Runtime.CompilerServices;

namespace Foundation
{
	public struct SceneUnloadCallbacks
	{
		[CompilerGenerated]
		private readonly SceneUnloadSuccess _003CSuccess_003Ek__BackingField;

		[CompilerGenerated]
		private readonly SceneUnloadFailure _003CFailure_003Ek__BackingField;

		public SceneUnloadSuccess Success
		{
			[CompilerGenerated]
			get
			{
				return _003CSuccess_003Ek__BackingField;
			}
		}

		public SceneUnloadFailure Failure
		{
			[CompilerGenerated]
			get
			{
				return _003CFailure_003Ek__BackingField;
			}
		}

		public SceneUnloadCallbacks(SceneUnloadSuccess success, SceneUnloadFailure failure)
		{
			_003CSuccess_003Ek__BackingField = success;
			_003CFailure_003Ek__BackingField = failure;
		}
	}
}
