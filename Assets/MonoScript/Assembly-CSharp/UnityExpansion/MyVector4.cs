using System;
using UnityEngine;

namespace UnityExpansion
{
	[Serializable]
	public struct MyVector4
	{
		public const float kEpsilon = 1E-05f;

		public float x;

		public float y;

		public float z;

		public float w;

		private static readonly MyVector4 zeroVector = new MyVector4(0f, 0f, 0f, 0f);

		private static readonly MyVector4 oneVector = new MyVector4(1f, 1f, 1f, 1f);

		private static readonly MyVector4 positiveInfinityVector = new MyVector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		private static readonly MyVector4 negativeInfinityVector = new MyVector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

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
					throw new IndexOutOfRangeException("Invalid Vector4 index!");
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
					throw new IndexOutOfRangeException("Invalid Vector4 index!");
				}
			}
		}

		public MyVector4 normalized
		{
			get
			{
				return Normalize(this);
			}
		}

		public float magnitude
		{
			get
			{
				return Mathf.Sqrt(Dot(this, this));
			}
		}

		public float sqrMagnitude
		{
			get
			{
				return Dot(this, this);
			}
		}

		public static MyVector4 zero
		{
			get
			{
				return zeroVector;
			}
		}

		public static MyVector4 one
		{
			get
			{
				return oneVector;
			}
		}

		public static MyVector4 positiveInfinity
		{
			get
			{
				return positiveInfinityVector;
			}
		}

		public static MyVector4 negativeInfinity
		{
			get
			{
				return negativeInfinityVector;
			}
		}

		public MyVector4(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public MyVector4(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			w = 0f;
		}

		public MyVector4(float x, float y)
		{
			this.x = x;
			this.y = y;
			z = 0f;
			w = 0f;
		}

		public void Set(float newX, float newY, float newZ, float newW)
		{
			x = newX;
			y = newY;
			z = newZ;
			w = newW;
		}

		public static MyVector4 Lerp(MyVector4 a, MyVector4 b, float t)
		{
			t = Mathf.Clamp01(t);
			return new MyVector4(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
		}

		public static MyVector4 LerpUnclamped(MyVector4 a, MyVector4 b, float t)
		{
			return new MyVector4(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t, a.w + (b.w - a.w) * t);
		}

		public static MyVector4 MoveTowards(MyVector4 current, MyVector4 target, float maxDistanceDelta)
		{
			MyVector4 myVector = target - current;
			float num = myVector.magnitude;
			if (num <= maxDistanceDelta || num == 0f)
			{
				return target;
			}
			return current + myVector / num * maxDistanceDelta;
		}

		public static MyVector4 Scale(MyVector4 a, MyVector4 b)
		{
			return new MyVector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
		}

		public void Scale(MyVector4 scale)
		{
			x *= scale.x;
			y *= scale.y;
			z *= scale.z;
			w *= scale.w;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
		}

		public override bool Equals(object other)
		{
			if (!(other is MyVector4))
			{
				return false;
			}
			MyVector4 myVector = (MyVector4)other;
			if (x.Equals(myVector.x) && y.Equals(myVector.y) && z.Equals(myVector.z))
			{
				return w.Equals(myVector.w);
			}
			return false;
		}

		public static MyVector4 Normalize(MyVector4 a)
		{
			float num = Magnitude(a);
			if (num > 1E-05f)
			{
				return a / num;
			}
			return zero;
		}

		public void Normalize()
		{
			float num = Magnitude(this);
			if (num > 1E-05f)
			{
				this /= num;
			}
			else
			{
				this = zero;
			}
		}

		public static float Dot(MyVector4 a, MyVector4 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		public static MyVector4 Project(MyVector4 a, MyVector4 b)
		{
			return b * Dot(a, b) / Dot(b, b);
		}

		public static float Distance(MyVector4 a, MyVector4 b)
		{
			return Magnitude(a - b);
		}

		public static float Magnitude(MyVector4 a)
		{
			return Mathf.Sqrt(Dot(a, a));
		}

		public static MyVector4 Min(MyVector4 lhs, MyVector4 rhs)
		{
			return new MyVector4(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z), Mathf.Min(lhs.w, rhs.w));
		}

		public static MyVector4 Max(MyVector4 lhs, MyVector4 rhs)
		{
			return new MyVector4(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z), Mathf.Max(lhs.w, rhs.w));
		}

		public static MyVector4 operator +(MyVector4 a, MyVector4 b)
		{
			return new MyVector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
		}

		public static MyVector4 operator -(MyVector4 a, MyVector4 b)
		{
			return new MyVector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
		}

		public static MyVector4 operator -(MyVector4 a)
		{
			return new MyVector4(0f - a.x, 0f - a.y, 0f - a.z, 0f - a.w);
		}

		public static MyVector4 operator *(MyVector4 a, float d)
		{
			return new MyVector4(a.x * d, a.y * d, a.z * d, a.w * d);
		}

		public static MyVector4 operator *(float d, MyVector4 a)
		{
			return new MyVector4(a.x * d, a.y * d, a.z * d, a.w * d);
		}

		public static MyVector4 operator /(MyVector4 a, float d)
		{
			return new MyVector4(a.x / d, a.y / d, a.z / d, a.w / d);
		}

		public static bool operator ==(MyVector4 lhs, MyVector4 rhs)
		{
			return SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
		}

		public static bool operator !=(MyVector4 lhs, MyVector4 rhs)
		{
			return !(lhs == rhs);
		}

		public static implicit operator MyVector4(MyVector3 v)
		{
			return new MyVector4(v.x, v.y, v.z, 0f);
		}

		public static implicit operator MyVector3(MyVector4 v)
		{
			return new MyVector3(v.x, v.y, v.z);
		}

		public static implicit operator MyVector4(MyVector2 v)
		{
			return new MyVector4(v.x, v.y, 0f, 0f);
		}

		public static implicit operator MyVector2(MyVector4 v)
		{
			return new MyVector2(v.x, v.y);
		}

		public override string ToString()
		{
			return string.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", x, y, z, w);
		}

		public string ToString(string format)
		{
			return string.Format("({0}, {1}, {2}, {3})", x.ToString(format), y.ToString(format), z.ToString(format), w.ToString(format));
		}

		public static float SqrMagnitude(MyVector4 a)
		{
			return Dot(a, a);
		}

		public float SqrMagnitude()
		{
			return Dot(this, this);
		}
	}
}
