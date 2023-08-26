using UnityEngine;

namespace Foundation
{
	internal sealed class ScreenInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Screen Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Current Resolution", GetResolutionString(Screen.currentResolution));
			DrawItem("Screen Width", string.Format("{0} px / {1:F2} in / {2:F2} cm", Screen.width.ToString(), ConvertUtility.GetInchesFromPixels(Screen.width), ConvertUtility.GetCentimetersFromPixels(Screen.width)));
			DrawItem("Screen Height", string.Format("{0} px / {1:F2} in / {2:F2} cm", Screen.height.ToString(), ConvertUtility.GetInchesFromPixels(Screen.height), ConvertUtility.GetCentimetersFromPixels(Screen.height)));
			DrawItem("Screen DPI", Screen.dpi.ToString("F2"));
			DrawItem("Screen Orientation", Screen.orientation.ToString());
			DrawItem("Is Full Screen", Screen.fullScreen.ToString());
			DrawItem("Sleep Timeout", GetSleepTimeoutDescription(Screen.sleepTimeout));
			DrawItem("Cursor Visible", Cursor.visible.ToString());
			DrawItem("Cursor Lock State", Cursor.lockState.ToString());
			DrawItem("Auto Landscape Left", Screen.autorotateToLandscapeLeft.ToString());
			DrawItem("Auto Landscape Right", Screen.autorotateToLandscapeRight.ToString());
			DrawItem("Auto Portrait", Screen.autorotateToPortrait.ToString());
			DrawItem("Auto Portrait Upside Down", Screen.autorotateToPortraitUpsideDown.ToString());
			DrawItem("Support Resolutions", GetResolutionsString(Screen.resolutions));
			GUILayout.EndVertical();
		}

		private string GetSleepTimeoutDescription(int sleepTimeout)
		{
			switch (sleepTimeout)
			{
			case -1:
				return "Never Sleep";
			case -2:
				return "System Setting";
			default:
				return sleepTimeout.ToString();
			}
		}

		private string GetResolutionString(Resolution resolution)
		{
			return resolution.width + " x " + resolution.height + " @ " + resolution.refreshRate + "Hz";
		}

		private string GetResolutionsString(Resolution[] resolutions)
		{
			string[] array = new string[resolutions.Length];
			for (int i = 0; i < resolutions.Length; i++)
			{
				array[i] = GetResolutionString(resolutions[i]);
			}
			return string.Join("; ", array);
		}
	}
}
