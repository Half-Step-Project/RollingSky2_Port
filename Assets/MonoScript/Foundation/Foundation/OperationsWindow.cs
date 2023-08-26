using UnityEngine;

namespace Foundation
{
	internal sealed class OperationsWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Operations</b>");
			GUILayout.BeginVertical("box");
			if (Mod.ObjectPool != null && GUILayout.Button("Object Pool Release", GUILayout.Height(30f)))
			{
				Mod.ObjectPool.Unload();
			}
			if (Mod.Resource != null)
			{
				if (GUILayout.Button("Unload Unused Assets", GUILayout.Height(30f)))
				{
					Mod.Resource.UnloadUnusedAssets();
				}
				if (GUILayout.Button("Unload Unused Assets and Garbage Collect", GUILayout.Height(30f)))
				{
					Mod.Resource.UnloadUnusedAssets(true);
				}
			}
			if (GUILayout.Button("Shutdown (Restart)", GUILayout.Height(30f)))
			{
				Mod.Reboot();
			}
			if (GUILayout.Button("Shutdown (Quit)", GUILayout.Height(30f)))
			{
				Mod.Shutdown();
			}
			GUILayout.BeginVertical("box");
			float speed = Mod.Core.Speed;
			GUILayout.Label("TimeScale:" + speed, GUILayout.Width(200f));
			speed = GUILayout.HorizontalSlider(speed, 0f, 8f);
			Mod.Core.Speed = speed;
			GUILayout.EndVertical();
			GUILayout.EndVertical();
		}
	}
}
