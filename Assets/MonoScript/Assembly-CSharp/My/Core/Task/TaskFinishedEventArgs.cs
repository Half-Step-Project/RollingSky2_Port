using System;

namespace My.Core.Task
{
	public class TaskFinishedEventArgs : EventArgs
	{
		public ITask Task { get; private set; }

		public bool IsCanceled { get; private set; }

		public TaskFinishedEventArgs(ITask task, bool isCanceled)
		{
			Task = task;
			IsCanceled = isCanceled;
		}
	}
}
