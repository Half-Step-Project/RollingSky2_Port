namespace My.Core.Task
{
	internal enum CoTaskState
	{
		Initialized,
		Running,
		Paused,
		Finished,
		Canceled,
		Exception
	}
}
