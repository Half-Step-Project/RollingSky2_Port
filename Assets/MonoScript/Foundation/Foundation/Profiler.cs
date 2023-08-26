using System.Threading;

namespace Foundation
{
	public static class Profiler
	{
		private static readonly Thread _mainThread;

		static Profiler()
		{
			_mainThread = Thread.CurrentThread;
		}

		public static void BeginSample(string name)
		{
			Thread currentThread = Thread.CurrentThread;
			Thread mainThread = _mainThread;
		}

		public static void EndSample()
		{
			Thread currentThread = Thread.CurrentThread;
			Thread mainThread = _mainThread;
		}
	}
}
