namespace Foundation
{
	public interface IDebuggerWindowGroup : IDebuggerWindow
	{
		int WindowCount { get; }

		int SelectedIndex { get; set; }

		IDebuggerWindow SelectedWindow { get; }

		string[] WindowNames { get; }

		IDebuggerWindow GetWindow(string path);

		void RegisterWindow(string path, IDebuggerWindow window);
	}
}
