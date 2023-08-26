using UnityEngine;
using UnityEngine.Profiling;

namespace Foundation
{
	internal sealed class ProfilerInformationWindow : ScrollableDebuggerWindowBase
	{
		private const int MbSize = 1048576;

		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Profiler Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Supported:", UnityEngine.Profiling.Profiler.supported.ToString());
			DrawItem("Enabled:", UnityEngine.Profiling.Profiler.enabled.ToString());
			DrawItem("Enable Binary Log:", UnityEngine.Profiling.Profiler.enableBinaryLog ? ("True, " + UnityEngine.Profiling.Profiler.logFile) : "False");
			DrawItem("Mono Used Size:", string.Format("{0:F3} MB", (float)UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() / 1048576f));
			DrawItem("Mono Heap Size:", string.Format("{0:F3} MB", (float)UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong() / 1048576f));
			DrawItem("Used Heap Size:", string.Format("{0:F3} MB", (float)UnityEngine.Profiling.Profiler.usedHeapSizeLong / 1048576f));
			DrawItem("Total Allocated Memory:", string.Format("{0:F3} MB", (float)UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1048576f));
			DrawItem("Total Reserved Memory:", string.Format("{0:F3} MB", (float)UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1048576f));
			DrawItem("Total Unused Reserved Memory:", string.Format("{0:F3} MB", (float)UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() / 1048576f));
			DrawItem("Temp Allocator Size:", string.Format("{0:F3} MB", (float)UnityEngine.Profiling.Profiler.GetTempAllocatorSize() / 1048576f));
			GUILayout.EndVertical();
		}
	}
}
