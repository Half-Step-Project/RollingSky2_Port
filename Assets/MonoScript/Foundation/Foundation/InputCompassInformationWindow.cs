using System.Globalization;
using UnityEngine;

namespace Foundation
{
	internal sealed class InputCompassInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Input Compass Information</b>");
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Enable", GUILayout.Height(30f)))
			{
				Input.compass.enabled = true;
			}
			if (GUILayout.Button("Disable", GUILayout.Height(30f)))
			{
				Input.compass.enabled = false;
			}
			GUILayout.EndHorizontal();
			DrawItem("Enabled:", Input.compass.enabled.ToString());
			DrawItem("Heading Accuracy:", Input.compass.headingAccuracy.ToString(CultureInfo.InvariantCulture));
			DrawItem("Magnetic Heading:", Input.compass.magneticHeading.ToString(CultureInfo.InvariantCulture));
			DrawItem("Raw Vector:", Input.compass.rawVector.ToString());
			DrawItem("Timestamp:", Input.compass.timestamp.ToString(CultureInfo.InvariantCulture));
			DrawItem("True Heading:", Input.compass.trueHeading.ToString(CultureInfo.InvariantCulture));
			GUILayout.EndVertical();
		}
	}
}
