using System.Runtime.CompilerServices;

namespace Foundation
{
	internal sealed class UIFormOpenCacheData
	{
		[CompilerGenerated]
		private readonly UIForm _003CForm_003Ek__BackingField;

		[CompilerGenerated]
		private readonly object _003CUserData_003Ek__BackingField;

		[CompilerGenerated]
		private readonly float _003CDuration_003Ek__BackingField;

		public UIForm Form
		{
			[CompilerGenerated]
			get
			{
				return _003CForm_003Ek__BackingField;
			}
		}

		public object UserData
		{
			[CompilerGenerated]
			get
			{
				return _003CUserData_003Ek__BackingField;
			}
		}

		public float Duration
		{
			[CompilerGenerated]
			get
			{
				return _003CDuration_003Ek__BackingField;
			}
		}

		public UIFormOpenCacheData(UIForm uiForm, float duration, object userData)
		{
			_003CForm_003Ek__BackingField = uiForm;
			_003CDuration_003Ek__BackingField = duration;
			_003CUserData_003Ek__BackingField = userData;
		}
	}
}
