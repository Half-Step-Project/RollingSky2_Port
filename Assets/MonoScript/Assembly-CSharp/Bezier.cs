using System;
using UnityEngine;

public class Bezier
{
	public static Vector3[] GetPathByPositions(Vector3[] path, int smooth = 20)
	{
		int num = path.Length;
		Vector3[] array = new Vector3[smooth * num + 1];
		Vector3[] pts = PathControlPointGenerator(path);
		Vector3 vector = Interp(pts, 0f);
		int num2 = path.Length * smooth;
		for (int i = 1; i <= num2 + 1; i++)
		{
			float t = (float)i / (float)num2;
			Vector3 vector2 = Interp(pts, t);
			array[i - 1] = vector;
			vector = vector2;
		}
		return array;
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

	public static Vector3 PutOnPath(Vector3[] path, float percent)
	{
		return Interp(PathControlPointGenerator(path), percent);
	}

	private static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		int num = 2;
		Vector3[] array = new Vector3[path.Length + num];
		Array.Copy(path, 0, array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = new Vector3[array2.Length];
			Array.Copy(array2, array, array2.Length);
		}
		return array;
	}

	private static Vector3 Interp(Vector3[] pts, float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 vector = pts[num2];
		Vector3 vector2 = pts[num2 + 1];
		Vector3 vector3 = pts[num2 + 2];
		Vector3 vector4 = pts[num2 + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
	}
}
