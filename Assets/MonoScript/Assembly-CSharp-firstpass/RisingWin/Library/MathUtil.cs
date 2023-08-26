using UnityEngine;

namespace RisingWin.Library
{
	public class MathUtil
	{
		public static float FLOAT_EPSILON = 1E-07f;

		public static bool FloatEqualZero(float val)
		{
			if (Mathf.Abs(val) <= FLOAT_EPSILON)
			{
				return true;
			}
			return false;
		}

		public static bool FloatEqualCmp(float val, float cmp = 1E-07f)
		{
			if (Mathf.Abs(val) <= cmp)
			{
				return true;
			}
			return false;
		}

		public static bool FloatEqual(float val1, float val2)
		{
			if (val2 > val1)
			{
				return val2 - val1 <= FLOAT_EPSILON;
			}
			return val1 - val2 <= FLOAT_EPSILON;
		}

		public static bool FloatEqual3(float val1, float val2, float val3)
		{
			if (val2 > val1)
			{
				return val2 - val1 <= val3;
			}
			return val1 - val2 <= val3;
		}

		public static bool Vector2Zero(Vector2 val)
		{
			if (FloatEqualZero(val.x))
			{
				return FloatEqualZero(val.y);
			}
			return false;
		}

		public static bool Vector2Equal(Vector2 val1, Vector2 val2)
		{
			if (FloatEqual(val1.x, val2.x))
			{
				return FloatEqual(val1.y, val2.y);
			}
			return false;
		}

		public static bool Vector2Equal3(Vector2 val1, Vector2 val2, Vector2 val3)
		{
			if (FloatEqual3(val1.x, val2.x, val3.x))
			{
				return FloatEqual3(val1.y, val2.y, val3.y);
			}
			return false;
		}

		public static bool Vector3EqualZero(Vector3 val)
		{
			if (FloatEqualZero(val.x) && FloatEqualZero(val.y))
			{
				return FloatEqualZero(val.z);
			}
			return false;
		}

		public static bool Vector3Equal(Vector3 val1, Vector3 val2)
		{
			if (FloatEqual(val1.x, val2.x) && FloatEqual(val1.y, val2.y))
			{
				return FloatEqual(val1.z, val2.z);
			}
			return false;
		}

		public static bool Vector3IntEqualZero(Vector3Int val)
		{
			if (val.x == 0 && val.y == 0)
			{
				return val.z == 0;
			}
			return false;
		}

		public static float GetMappingValue(float pOriginalMin, float pOriginalMax, float pCurrentMin, float pCurrentMax, float pOriginalValue)
		{
			return pCurrentMin + (pCurrentMax - pCurrentMin) * ((pOriginalValue - pOriginalMin) / (pOriginalMax - pOriginalMin));
		}

		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		public static int IntClamp(int val, int min, int max)
		{
			if (val <= min)
			{
				return min;
			}
			if (val >= max)
			{
				return max;
			}
			return val;
		}

		public static float FloatClamp(float val, float min, float max)
		{
			if (val <= min)
			{
				return min;
			}
			if (val >= max)
			{
				return max;
			}
			return val;
		}

		public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
		{
			Vector3 lhs = linePoint2 - linePoint1;
			Vector3 rhs = Vector3.Cross(lineVec1, lineVec2);
			Vector3 lhs2 = Vector3.Cross(lhs, lineVec2);
			if (Mathf.Abs(Vector3.Dot(lhs, rhs)) < 0.0001f && rhs.sqrMagnitude > 0.0001f)
			{
				float num = Vector3.Dot(lhs2, rhs) / rhs.sqrMagnitude;
				intersection = linePoint1 + lineVec1 * num;
				return true;
			}
			intersection = Vector3.zero;
			return false;
		}

		public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
		{
			if ((linePoint1 - linePoint2).sqrMagnitude < Mathf.Epsilon)
			{
				closestPointLine1 = linePoint1;
				closestPointLine2 = linePoint2;
				return true;
			}
			closestPointLine1 = Vector3.zero;
			closestPointLine2 = Vector3.zero;
			float num = Vector3.Dot(lineVec1, lineVec1);
			float num2 = Vector3.Dot(lineVec1, lineVec2);
			float num3 = Vector3.Dot(lineVec2, lineVec2);
			float num4 = num * num3 - num2 * num2;
			if (num4 != 0f)
			{
				Vector3 rhs = linePoint1 - linePoint2;
				float num5 = Vector3.Dot(lineVec1, rhs);
				float num6 = Vector3.Dot(lineVec2, rhs);
				float num7 = (num2 * num6 - num5 * num3) / num4;
				float num8 = (num * num6 - num5 * num2) / num4;
				closestPointLine1 = linePoint1 + lineVec1 * num7;
				closestPointLine2 = linePoint2 + lineVec2 * num8;
				return true;
			}
			return false;
		}

		public static int FloatToIntGrid(float f, int gridSize = 1)
		{
			int num = (int)(f / (float)gridSize);
			decimal num2 = (decimal)f;
			if (0m != num2 % (decimal)gridSize)
			{
				num++;
			}
			return num;
		}

		public static bool IsSafeRotation(Vector3 rot)
		{
			if (float.IsNaN(rot.x) || float.IsNaN(rot.y) || float.IsNaN(rot.z))
			{
				return false;
			}
			if (float.IsInfinity(rot.x) || float.IsInfinity(rot.y) || float.IsInfinity(rot.z))
			{
				return false;
			}
			return true;
		}
	}
}
