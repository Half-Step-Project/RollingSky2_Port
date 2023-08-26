using UnityEngine;

public sealed class BezierMover
{
	private Vector3[] bezierPoints;

	private int currentIndex;

	private float cachedDistance;

	public BezierMover()
	{
	}

	public BezierMover(Vector3[] bezierPs)
	{
		bezierPoints = bezierPs;
		currentIndex = 0;
		cachedDistance = 0f;
	}

	public bool MoveForwardByDis(float distance, Vector3 beginPos, ref Vector3 targetPos, ref Vector3 moveDir, bool ifRecycle = false)
	{
		int num = bezierPoints.Length;
		int num2 = currentIndex;
		cachedDistance += distance;
		while (currentIndex < num - 1)
		{
			for (int i = currentIndex + 1; i <= num - 1; i++)
			{
				Vector3 vector = bezierPoints[currentIndex];
				Vector3 vector2 = bezierPoints[i];
				float magnitude = (vector2 - vector).magnitude;
				float t = cachedDistance / magnitude;
				if (cachedDistance <= magnitude)
				{
					targetPos = Vector3.Lerp(vector, vector2, t);
					moveDir = targetPos - beginPos;
					return false;
				}
				cachedDistance -= magnitude;
				currentIndex = i;
			}
		}
		targetPos = bezierPoints[currentIndex];
		float magnitude2 = (targetPos - bezierPoints[num2]).magnitude;
		cachedDistance -= magnitude2;
		moveDir = (targetPos - beginPos).normalized;
		if (ifRecycle)
		{
			currentIndex %= num - 1;
		}
		return true;
	}

	public bool MoveForwardByZ(float disLocalZ, Transform parentTrans, Vector3 beginLocPos, ref Vector3 targetLocPos, ref Vector3 moveLocDir)
	{
		int num = bezierPoints.Length;
		while (currentIndex < num - 1)
		{
			Vector3 vector = bezierPoints[currentIndex];
			if ((bool)parentTrans)
			{
				vector = parentTrans.InverseTransformPoint(vector);
			}
			float num2 = vector.z - beginLocPos.z;
			if (Mathf.Abs(num2 / disLocalZ) < 1f)
			{
				currentIndex++;
				continue;
			}
			targetLocPos = Vector3.Lerp(beginLocPos, vector, Mathf.Abs(disLocalZ / num2));
			moveLocDir = (targetLocPos - beginLocPos).normalized;
			return false;
		}
		Vector3 vector2 = bezierPoints[currentIndex];
		if (parentTrans != null)
		{
			targetLocPos = parentTrans.InverseTransformPoint(vector2);
		}
		else
		{
			targetLocPos = vector2;
		}
		moveLocDir = (targetLocPos - beginLocPos).normalized;
		return true;
	}

	public bool MoveToPosZ(ref Vector3 targetPos, ref Vector3 moveDir)
	{
		float z = targetPos.z;
		float z2 = bezierPoints[currentIndex].z;
		if (z < bezierPoints[0].z)
		{
			return false;
		}
		int num = bezierPoints.Length;
		if (moveDir.z > 0f)
		{
			if (z < z2)
			{
				currentIndex = 0;
			}
			while (currentIndex < num - 1)
			{
				Vector3 vector = bezierPoints[currentIndex];
				Vector3 vector2 = bezierPoints[currentIndex + 1];
				float num2 = (z - vector.z) / (vector2.z - vector.z);
				if (num2 >= 0f && num2 <= 1f)
				{
					targetPos = Vector3.Lerp(vector, vector2, num2);
					if (num2 == 0f)
					{
						moveDir = (vector2 - vector).normalized;
					}
					else
					{
						moveDir = (targetPos - vector).normalized;
					}
					return false;
				}
				if (num2 > 1f)
				{
					currentIndex++;
					continue;
				}
				Debug.LogWarningFormat("CALL percent:{0} is smaller 0", num2);
				return false;
			}
			moveDir = (bezierPoints[currentIndex] - bezierPoints[currentIndex - 1]).normalized;
			return true;
		}
		if (moveDir.z < 0f)
		{
			if (z > z2)
			{
				currentIndex = num - 1;
			}
			while (currentIndex > 0)
			{
				Vector3 vector3 = bezierPoints[currentIndex];
				Vector3 vector4 = bezierPoints[currentIndex - 1];
				float num3 = (z - vector3.z) / (vector4.z - vector3.z);
				if (num3 >= 0f && num3 <= 1f)
				{
					targetPos = Vector3.Lerp(vector3, vector4, num3);
					if (num3 == 0f)
					{
						moveDir = (vector4 - vector3).normalized;
					}
					else
					{
						moveDir = (targetPos - vector3).normalized;
					}
					return false;
				}
				if (num3 > 1f)
				{
					currentIndex--;
					continue;
				}
				Debug.LogWarningFormat("CALL percent:{0} is smaller 0", num3);
				break;
			}
			moveDir = (bezierPoints[0] - bezierPoints[1]).normalized;
			return false;
		}
		Debug.LogError("MoveDir.z should not be 0!!!");
		return false;
	}

	public void ResetData(Vector3[] bezierPs = null)
	{
		if (bezierPs != null)
		{
			bezierPoints = bezierPs;
		}
		currentIndex = 0;
		cachedDistance = 0f;
	}

	public int GetIndex()
	{
		return currentIndex;
	}

	public void SetIndex(int index)
	{
		currentIndex = index;
	}

	public float GetDistance()
	{
		return cachedDistance;
	}

	public void SetDistance(float distance)
	{
		cachedDistance = distance;
	}

	public float GetTotalDistance()
	{
		if (bezierPoints.Length > 2)
		{
			float num = 0f;
			for (int i = 0; i < bezierPoints.Length - 1; i++)
			{
				num += (bezierPoints[i + 1] - bezierPoints[i]).magnitude;
			}
			return num;
		}
		return -1f;
	}

	public float GetTotalDistanceZ()
	{
		if (bezierPoints.Length > 2)
		{
			return bezierPoints[bezierPoints.Length - 1].z - bezierPoints[0].z;
		}
		return -1f;
	}
}
