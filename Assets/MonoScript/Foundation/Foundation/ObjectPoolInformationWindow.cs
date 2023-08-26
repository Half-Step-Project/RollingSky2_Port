using UnityEngine;

namespace Foundation
{
	internal sealed class ObjectPoolInformationWindow : ScrollableDebuggerWindowBase
	{
		protected override void OnDrawScrollableWindow()
		{
			GUILayout.Label("<b>Object Pool Information</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Object Pool Count", Mod.ObjectPool.Count.ToString());
			GUILayout.EndVertical();
			IObjectPool[] objectPools = Mod.ObjectPool.ObjectPools;
			for (int i = 0; i < objectPools.Length; i++)
			{
				DrawObjectPool(objectPools[i]);
			}
		}

		private void DrawObjectPool(IObjectPool objectPool)
		{
			GUILayout.Label("<b>Object Pool: " + (string.IsNullOrEmpty(objectPool.Name) ? "<Unnamed>" : objectPool.Name) + "</b>");
			GUILayout.BeginVertical("box");
			DrawItem("Type", objectPool.ObjectType.FullName);
			DrawItem("Used Count", objectPool.Count.ToString());
			DrawItem("Can Release Count", objectPool.CanUnloadCount.ToString());
			PoolObjectInfo[] objectInfos = objectPool.ObjectInfos;
			GUILayout.BeginHorizontal();
			GUILayout.Label("<b>Name</b>");
			GUILayout.Label("<b>Locked</b>", GUILayout.Width(60f));
			GUILayout.Label("<b>Use Count</b>", GUILayout.Width(60f));
			GUILayout.Label("<b>Priority</b>", GUILayout.Width(60f));
			GUILayout.Label("<b>Last Use Time</b>", GUILayout.Width(120f));
			GUILayout.EndHorizontal();
			if (objectInfos.Length != 0)
			{
				for (int i = 0; i < objectInfos.Length; i++)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(objectInfos[i].Name);
					GUILayout.Label(objectInfos[i].Locked.ToString(), GUILayout.Width(60f));
					GUILayout.Label(objectInfos[i].UseCount.ToString(), GUILayout.Width(60f));
					GUILayout.EndHorizontal();
				}
			}
			else
			{
				GUILayout.Label("<i>Object Pool is Empty ...</i>");
			}
			GUILayout.EndVertical();
		}
	}
}
