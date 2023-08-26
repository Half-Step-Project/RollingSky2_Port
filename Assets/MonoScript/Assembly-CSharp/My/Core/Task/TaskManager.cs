using System.Collections.Generic;

namespace My.Core.Task
{
	public sealed class TaskManager : MonoSingleton<TaskManager>
	{
		private static List<ITask> tasks = new List<ITask>();

		internal static void AddTask(ITask task)
		{
			tasks.Add(task);
			task.Finished += delegate(TaskFinishedEventArgs args)
			{
				tasks.Remove(args.Task);
			};
		}
	}
}
