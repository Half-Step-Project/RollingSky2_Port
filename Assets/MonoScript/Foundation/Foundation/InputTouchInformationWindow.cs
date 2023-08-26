using System.Globalization;
using UnityEngine;

namespace Foundation
{
	internal sealed class InputTouchInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Input Touch Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Touch Supported:", Input.touchSupported.ToString());
			DrawItem("Touch Pressure Supported:", Input.touchPressureSupported.ToString());
			DrawItem("Stylus Touch Supported:", Input.stylusTouchSupported.ToString());
			DrawItem("Simulate Mouse With Touches:", Input.simulateMouseWithTouches.ToString());
			DrawItem("Multi Touch Enabled:", Input.multiTouchEnabled.ToString());
			DrawItem("Touch Count:", Input.touchCount.ToString());
			DrawItem("Touches:", GetTouchesString(Input.touches));
			GUILayout.EndVertical();
		}

		private string GetTouchString(Touch touch)
		{
			return touch.position.ToString() + ", " + touch.deltaPosition.ToString() + ", " + touch.rawPosition.ToString() + ", " + touch.pressure.ToString(CultureInfo.InvariantCulture) + ", " + touch.phase;
		}

		private string GetTouchesString(Touch[] touches)
		{
			string[] array = new string[touches.Length];
			for (int i = 0; i < touches.Length; i++)
			{
				array[i] = GetTouchString(touches[i]);
			}
			return string.Join("; ", array);
		}
	}
}
