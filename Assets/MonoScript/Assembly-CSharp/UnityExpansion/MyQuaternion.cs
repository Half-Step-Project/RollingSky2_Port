using System;
using UnityEngine;

namespace UnityExpansion
{
	[Serializable]
	public struct MyQuaternion
	{
		public float x;

		public float y;

		public float z;

		public float w;

		private static readonly MyQuaternion identityQuaternion = new MyQuaternion(0f, 0f, 0f, 1f);

		public const float kEpsilon = 1E-06f;

		public float this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return x;
				case 1:
					return y;
				case 2:
					return z;
				case 3:
					return w;
				default:
					throw new IndexOutOfRangeException("Invalid Quaternion index!");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					x = value;
					break;
				case 1:
					y = value;
					break;
				case 2:
					z = value;
					break;
				case 3:
					w = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Quaternion index!");
				}
			}
		}

		public static MyQuaternion identity
		{
			get
			{
				return identityQuaternion;
			}
		}

		public MyQuaternion(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public void Set(float newX, float newY, float newZ, float newW)
		{
			x = newX;
			y = newY;
			z = newZ;
			w = newW;
		}

		public static MyQuaternion operator *(MyQuaternion lhs, MyQuaternion rhs)
		{
			return new MyQuaternion(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
		}

		public static MyVector3 operator *(MyQuaternion rotation, MyVector3 point)
		{
			float num = rotation.x * 2f;
			float num2 = rotation.y * 2f;
			float num3 = rotation.z * 2f;
			float num4 = rotation.x * num;
			float num5 = rotation.y * num2;
			float num6 = rotation.z * num3;
			float num7 = rotation.x * num2;
			float num8 = rotation.x * num3;
			float num9 = rotation.y * num3;
			float num10 = rotation.w * num;
			float num11 = rotation.w * num2;
			float num12 = rotation.w * num3;
			MyVector3 result = default(MyVector3);
			result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
			result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
			result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
			return result;
		}

		private static bool IsEqualUsingDot(float dot)
		{
			return dot > 0.999999f;
		}

		public static bool operator ==(MyQuaternion lhs, MyQuaternion rhs)
		{
			return IsEqualUsingDot(Dot(lhs, rhs));
		}

		public static bool operator !=(MyQuaternion lhs, MyQuaternion rhs)
		{
			return !(lhs == rhs);
		}

		public static float Dot(MyQuaternion a, MyQuaternion b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		public static float Angle(MyQuaternion a, MyQuaternion b)
		{
			float num = Dot(a, b);
			if (!IsEqualUsingDot(num))
			{
				return Mathf.Acos(Mathf.Min(Mathf.Abs(num), 1f)) * 2f * 57.29578f;
			}
			return 0f;
		}

		private static MyVector3 Internal_MakePositive(MyVector3 euler)
		{
			float num = -0.005729578f;
			float num2 = 360f + num;
			if (euler.x < num)
			{
				euler.x += 360f;
			}
			else if (euler.x > num2)
			{
				euler.x -= 360f;
			}
			if (euler.y < num)
			{
				euler.y += 360f;
			}
			else if (euler.y > num2)
			{
				euler.y -= 360f;
			}
			if (euler.z < num)
			{
				euler.z += 360f;
			}
			else if (euler.z > num2)
			{
				euler.z -= 360f;
			}
			return euler;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
		}

		public override bool Equals(object other)
		{
			if (!(other is MyQuaternion))
			{
				return false;
			}
			MyQuaternion myQuaternion = (MyQuaternion)other;
			if (x.Equals(myQuaternion.x) && y.Equals(myQuaternion.y) && z.Equals(myQuaternion.z))
			{
				return w.Equals(myQuaternion.w);
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", x, y, z, w);
		}

		public string ToString(string format)
		{
			return string.Format("({0}, {1}, {2}, {3})", x.ToString(format), y.ToString(format), z.ToString(format), w.ToString(format));
		}
	}
}
