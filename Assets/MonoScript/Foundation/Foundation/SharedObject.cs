using System.Runtime.CompilerServices;

namespace Foundation
{
	public abstract class SharedObject
	{
		[CompilerGenerated]
		private readonly string _003CName_003Ek__BackingField;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _003CName_003Ek__BackingField;
			}
		}

		public object Target { get; private set; }

		public bool Locked { get; set; }

		public int UseCount { get; private set; }

		public bool IsInUsing
		{
			get
			{
				return UseCount > 0;
			}
		}

		protected SharedObject(string name, object target)
		{
			if (target == null)
			{
				Log.Error("Target '" + name + "' is invalid.");
				return;
			}
			_003CName_003Ek__BackingField = name;
			Target = target;
		}

		protected internal virtual void OnRegister()
		{
		}

		protected internal virtual void OnSpawn()
		{
			UseCount++;
		}

		protected internal virtual void OnRecycle()
		{
			UseCount--;
			if (UseCount < 0)
			{
				Log.Error("Object " + Name + " recycle error, because the using count has been less then 0.");
			}
		}

		protected internal virtual void OnUnload(bool force = false)
		{
			if (!force && UseCount > 0)
			{
				Log.Warning("Unload object " + Name + " that have been using.");
			}
			Target = null;
		}
	}
}
