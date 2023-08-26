namespace Foundation
{
	public interface ITaskAgent<T> where T : Task
	{
		T Task { get; }

		void Init();

		void Boot(T task);

		void Tick(float elapseSeconds, float realElapseSeconds);

		void Recycle();

		void Exit();
	}
}
