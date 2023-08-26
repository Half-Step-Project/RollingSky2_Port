using System.Runtime.CompilerServices;

namespace Foundation
{
	public abstract class Task
	{
		public const int InvalidId = -1;

		private static int _nextId;

		[CompilerGenerated]
		private readonly int _003CId_003Ek__BackingField;

		public int Id
		{
			[CompilerGenerated]
			get
			{
				return _003CId_003Ek__BackingField;
			}
		}

		public TaskStatus Status { get; internal set; }

		public bool IsDone
		{
			get
			{
				if (Status != TaskStatus.Done)
				{
					return Status == TaskStatus.Error;
				}
				return true;
			}
		}

		protected Task()
		{
			_003CId_003Ek__BackingField = _nextId++;
			Status = TaskStatus.Init;
		}
	}
}
