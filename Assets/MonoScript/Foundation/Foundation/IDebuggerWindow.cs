namespace Foundation
{
	public interface IDebuggerWindow
	{
		void OnInit(params object[] args);

		void OnExit();

		void OnEnter();

		void OnLeave();

		void OnTick(float elapseSeconds, float realElapseSeconds);

		void OnDraw();
	}
}
