using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallAutoPathTest : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			list.Add(base.transform.GetChild(i));
		}
		if (list != null && list.Count > 0)
		{
			Vector3[] pathByPositions = Bezier.GetPathByPositions((from c in list.Where((Transform c) => c != null).ToArray()
				select c.transform.position).ToArray());
			for (int j = 0; j < pathByPositions.Length - 1; j++)
			{
				Gizmos.DrawLine(pathByPositions[j], pathByPositions[j + 1]);
			}
		}
	}
}
