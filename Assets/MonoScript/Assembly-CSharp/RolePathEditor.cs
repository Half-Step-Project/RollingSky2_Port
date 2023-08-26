using System.Collections.Generic;
using UnityEngine;

public class RolePathEditor : MonoBehaviour
{
	public RolePathTable PathTable;

	public int ShowSize = 100;

	public Color[] PathColors = new Color[10];

	private void OnDrawGizmos()
	{
		if (!(PathTable != null))
		{
			return;
		}
		List<RolePathNode> pathData = PathTable.PathData;
		Vector3 size = new Vector3(0.1f, 0.5f, 0.1f);
		int num = 0;
		if (pathData == null)
		{
			return;
		}
		for (int i = 0; i < pathData.Count; i++)
		{
			Vector3 position = pathData[i].Position;
			if (i < pathData.Count - 1)
			{
				Vector3 position2 = pathData[i + 1].Position;
				if (num % 2 == 0)
				{
					if (position2.z < position.z)
					{
						num++;
					}
				}
				else if (num % 2 == 1 && position.z < position2.z)
				{
					num++;
				}
			}
			Gizmos.color = PathColors[num];
			Gizmos.DrawCube(position, size);
		}
	}
}
