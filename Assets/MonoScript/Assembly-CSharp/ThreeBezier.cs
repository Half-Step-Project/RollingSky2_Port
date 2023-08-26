using UnityEngine;

public class ThreeBezier
{
	public static Vector3[] GetPathByPositions(Vector3[] positions, int smooth = 50)
	{
		int num = positions.Length / 3;
		Vector3[] array = new Vector3[num * smooth + 1];
		if (positions.Length != 0)
		{
			array[0] = positions[0];
		}
		int num2 = 1;
		Vector3 vector = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < num; i++)
		{
			for (int j = 1; j <= smooth; j++)
			{
				float t = (float)j / (float)smooth;
				int num3 = i * 3;
				vector = (array[num2] = CalculateCubicBezierPoint(t, positions[num3], positions[num3 + 1], positions[num3 + 2], positions[num3 + 3]));
				num2++;
			}
		}
		return array;
	}

	public static PathToMoveTransform[] GetPathByPathToMoveTransform(PathToMoveTransform[] transforms, int smooth = 50)
	{
		int num = transforms.Length / 3;
		PathToMoveTransform[] array = new PathToMoveTransform[num * smooth + 1];
		if (transforms.Length != 0)
		{
			array[0] = transforms[0];
		}
		int num2 = 1;
		for (int i = 0; i < num; i++)
		{
			for (int j = 1; j <= smooth; j++)
			{
				PathToMoveTransform pathToMoveTransform = new PathToMoveTransform();
				float t = (float)j / (float)smooth;
				int num3 = i * 3;
				Vector3 vector = (pathToMoveTransform.m_position = CalculateCubicBezierPoint(t, transforms[num3].m_position, transforms[num3 + 1].m_position, transforms[num3 + 2].m_position, transforms[num3 + 3].m_position));
				pathToMoveTransform.m_rotation = Quaternion.Lerp(transforms[num3].m_rotation, transforms[num3 + 3].m_rotation, t);
				pathToMoveTransform.m_size = Vector3.Lerp(transforms[num3].m_size, transforms[num3 + 3].m_size, t);
				array[num2] = pathToMoveTransform;
				num2++;
			}
		}
		return array;
	}

	public static Vector3 PutOnPath(Vector3[] path, float percent)
	{
		int num = Mathf.CeilToInt((float)(path.Length - 1) * Mathf.Clamp(percent, 0f, 1f));
		int num2 = ((num - 1 > 0) ? (num - 1) : 0);
		float num3 = (float)num * 1f / (float)(path.Length - 1);
		float num4 = (float)num2 * 1f / (float)(path.Length - 1);
		float t = (percent - num4) / ((num3 - num4 == 0f) ? 1f : (num3 - num4));
		return Vector3.Lerp(path[num2], path[num], t);
	}

	public static float GetDistanceByPosition(Vector3[] paths)
	{
		float num = 0f;
		for (int i = 0; i < paths.Length; i++)
		{
			if (i > 0)
			{
				num += Vector3.Distance(paths[i], paths[i - 1]);
			}
		}
		return num;
	}

	private static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float num = 1f - t;
		float num2 = t * t;
		float num3 = num * num;
		float num4 = num3 * num;
		float num5 = num2 * t;
		return num4 * p0 + 3f * num3 * t * p1 + 3f * num * num2 * p2 + num5 * p3;
	}

	public static Vector3[] DrawGizmos(GameObject target, Vector3[] positions, int smooth)
	{
		if (positions == null || positions.Length < 4 || (positions.Length - 1) % 3 != 0)
		{
			return null;
		}
		Vector3[] array = new Vector3[positions.Length];
		for (int i = 0; i < positions.Length; i++)
		{
			array[i] = target.transform.TransformPoint(positions[i]);
		}
		Vector3[] pathByPositions = GetPathByPositions(array, 20);
		Gizmos.color = Color.yellow;
		for (int j = 0; j < pathByPositions.Length - 1; j++)
		{
			Gizmos.DrawLine(pathByPositions[j], pathByPositions[j + 1]);
		}
		return pathByPositions;
	}

	public static void DrawGizmos(GameObject target, PathToMoveTransform[] transforms, int smooth, bool isShowNormal = false)
	{
		if (transforms == null || transforms.Length < 4 || (transforms.Length - 1) % 3 != 0)
		{
			return;
		}
		Vector3[] array = new Vector3[transforms.Length];
		for (int i = 0; i < transforms.Length; i++)
		{
			array[i] = target.transform.TransformPoint(transforms[i].m_position);
		}
		PathToMoveTransform[] pathByPathToMoveTransform = GetPathByPathToMoveTransform(transforms, 20);
		Gizmos.color = Color.yellow;
		for (int j = 0; j < pathByPathToMoveTransform.Length; j++)
		{
			Vector3 vector = target.transform.TransformPoint(pathByPathToMoveTransform[j].m_position);
			if (j < pathByPathToMoveTransform.Length - 1)
			{
				Vector3 to = target.transform.TransformPoint(pathByPathToMoveTransform[j + 1].m_position);
				Gizmos.DrawLine(vector, to);
			}
			if (isShowNormal && j % 2 == 0)
			{
				Gizmos.DrawLine(vector, pathByPathToMoveTransform[j].m_rotation * new Vector3(0f, 3f, 0f) + vector);
			}
		}
	}
}
