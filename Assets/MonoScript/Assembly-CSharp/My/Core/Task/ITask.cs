namespace My.Core.Task
{
	public interface ITask
	{
		bool Living { get; }

		bool Running { get; }

		bool Paused { get; }

		event TaskFinishedHandler Finished;

		bool Start();

		bool Pause(bool pause);

		bool Wait(float keepTime);

		void Cancel();
	}
}
