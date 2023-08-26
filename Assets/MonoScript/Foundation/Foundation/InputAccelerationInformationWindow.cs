using System.Globalization;
using UnityEngine;

namespace Foundation
{
	internal sealed class InputAccelerationInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Input Acceleration Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Acceleration:", Input.acceleration.ToString());
			DrawItem("Acceleration Event Count:", Input.accelerationEventCount.ToString());
			DrawItem("Acceleration Events:", GetAccelerationEventsString(Input.accelerationEvents));
			GUILayout.EndVertical();
		}

		private string GetAccelerationEventString(AccelerationEvent accelerationEvent)
		{
			return accelerationEvent.acceleration.ToString() + ", " + accelerationEvent.deltaTime.ToString(CultureInfo.InvariantCulture);
		}

		private string GetAccelerationEventsString(AccelerationEvent[] accelerationEvents)
		{
			string[] array = new string[accelerationEvents.Length];
			for (int i = 0; i < accelerationEvents.Length; i++)
			{
				array[i] = GetAccelerationEventString(accelerationEvents[i]);
			}
			return string.Join("; ", array);
		}
	}
}
