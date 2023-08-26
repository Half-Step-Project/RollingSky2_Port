using System.Globalization;
using UnityEngine;

namespace Foundation
{
	internal sealed class TimeInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Time Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Time Scale", Time.timeScale.ToString(CultureInfo.InvariantCulture) + " [" + GetTimeScaleDescription(Time.timeScale) + "]");
			DrawItem("Realtime Since Startup", Time.realtimeSinceStartup.ToString(CultureInfo.InvariantCulture));
			DrawItem("Time Since Level Load", Time.timeSinceLevelLoad.ToString(CultureInfo.InvariantCulture));
			DrawItem("Time", Time.time.ToString(CultureInfo.InvariantCulture));
			DrawItem("Fixed Time", Time.fixedTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Unscaled Time", Time.unscaledTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Fixed Unscaled Time", Time.fixedUnscaledTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Delta Time", Time.deltaTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Fixed Delta Time", Time.fixedDeltaTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Unscaled Delta Time", Time.unscaledDeltaTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Fixed Unscaled Delta Time", Time.fixedUnscaledDeltaTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Smooth Delta Time", Time.smoothDeltaTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Maximum Delta Time", Time.maximumDeltaTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Maximum Particle Delta Time", Time.maximumParticleDeltaTime.ToString(CultureInfo.InvariantCulture));
			DrawItem("Frame Count", Time.frameCount.ToString());
			DrawItem("Rendered Frame Count", Time.renderedFrameCount.ToString());
			DrawItem("Capture Framerate", Time.captureFramerate.ToString());
			DrawItem("In Fixed Time Step", Time.inFixedTimeStep.ToString());
			GUILayout.EndVertical();
		}

		private string GetTimeScaleDescription(float timeScale)
		{
			if (timeScale <= 0f)
			{
				return "Pause";
			}
			if (timeScale < 1f)
			{
				return "Slower";
			}
			if (!(timeScale > 1f))
			{
				return "Normal";
			}
			return "Faster";
		}
	}
}
