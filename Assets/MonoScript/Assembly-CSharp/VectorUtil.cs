using UnityEngine;

public static class VectorUtil
{
	public static Vector3 GetMulti(this Vector3 val1, Vector3 val2)
	{
		return new Vector3(val1.x * val2.x, val1.y * val2.y, val1.z * val2.z);
	}
}
