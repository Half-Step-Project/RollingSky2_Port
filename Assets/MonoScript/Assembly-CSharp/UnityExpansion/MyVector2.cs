using System;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityExpansion
{
	[Serializable]
	public struct MyVector2
	{
		public float x;

		public float y;

		private static readonly MyVector2 zeroVector = new MyVector2(0f, 0f);

		private static readonly MyVector2 oneVector = new MyVector2(1f, 1f);

		private static readonly MyVector2 upVector = new MyVector2(0f, 1f);

		private static readonly MyVector2 downVector = new MyVector2(0f, -1f);

		private static readonly MyVector2 leftVector = new MyVector2(-1f, 0f);

		private static readonly MyVector2 rightVector = new MyVector2(1f, 0f);

		private static readonly MyVector2 positiveInfinityVector = new MyVector2(float.PositiveInfinity, float.PositiveInfinity);

		private static readonly MyVector2 negativeInfinityVector = new MyVector2(float.NegativeInfinity, float.NegativeInfinity);

		public const float kEpsilon = 1E-05f;

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
				default:
					throw new IndexOutOfRangeException("Invalid Vector2 index!");
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
				default:
					throw new IndexOutOfRangeException("Invalid Vector2 index!");
				}
			}
		}

		public MyVector2 normalized
		{
			get
			{
				MyVector2 result = new MyVector2(x, y);
				result.Normalize();
				return result;
			}
		}

		public float magnitude
		{
			get
			{
				return Mathf.Sqrt(x * x + y * y);
			}
		}

		public float sqrMagnitude
		{
			get
			{
				return x * x + y * y;
			}
		}

		public static MyVector2 zero
		{
			get
			{
				return zeroVector;
			}
		}

		public static MyVector2 one
		{
			get
			{
				return oneVector;
			}
		}

		public static MyVector2 up
		{
			get
			{
				return upVector;
			}
		}

		public static MyVector2 down
		{
			get
			{
				return downVector;
			}
		}

		public static MyVector2 left
		{
			get
			{
				return leftVector;
			}
		}

		public static MyVector2 right
		{
			get
			{
				return rightVector;
			}
		}

		public static MyVector2 positiveInfinity
		{
			get
			{
				return positiveInfinityVector;
			}
		}

		public static MyVector2 negativeInfinity
		{
			get
			{
				return negativeInfinityVector;
			}
		}

		public MyVector2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public void Set(float newX, float newY)
		{
			x = newX;
			y = newY;
		}

		public static MyVector2 Lerp(MyVector2 a, MyVector2 b, float t)
		{
			t = Mathf.Clamp01(t);
			return new MyVector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
		}

		public static MyVector2 LerpUnclamped(MyVector2 a, MyVector2 b, float t)
		{
			return new MyVector2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
		}

		public static MyVector2 MoveTowards(MyVector2 current, MyVector2 target, float maxDistanceDelta)
		{
			MyVector2 myVector = target - current;
			float num = myVector.magnitude;
			if (num <= maxDistanceDelta || num == 0f)
			{
				return target;
			}
			return current + myVector / num * maxDistanceDelta;
		}

		public static MyVector2 Scale(MyVector2 a, MyVector2 b)
		{
			return new MyVector2(a.x * b.x, a.y * b.y);
		}

		public void Scale(MyVector2 scale)
		{
			x *= scale.x;
			y *= scale.y;
		}

		public void Normalize()
		{
			float num = magnitude;
			if (num > 1E-05f)
			{
				this /= num;
			}
			else
			{
				this = zero;
			}
		}

		public override string ToString()
		{
			return string.Format("({0:F1}, {1:F1})", x, y);
		}

		public string ToString(string format)
		{
			return string.Format("({0}, {1})", x.ToString(format), y.ToString(format));
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ (y.GetHashCode() << 2);
		}

		public override bool Equals(object other)
		{
			if (!(other is MyVector2))
			{
				return false;
			}
			MyVector2 myVector = (MyVector2)other;
			if (x.Equals(myVector.x))
			{
				return y.Equals(myVector.y);
			}
			return false;
		}

		public static MyVector2 Reflect(MyVector2 inDirection, MyVector2 inNormal)
		{
			return -2f * Dot(inNormal, inDirection) * inNormal + inDirection;
		}

		public static MyVector2 Perpendicular(MyVector2 inDirection)
		{
			return new MyVector2(0f - inDirection.y, inDirection.x);
		}

		public static float Dot(MyVector2 lhs, MyVector2 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y;
		}

		public static float Angle(MyVector2 from, MyVector2 to)
		{
			return Mathf.Acos(Mathf.Clamp(Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
		}

		public static float SignedAngle(MyVector2 from, MyVector2 to)
		{
			MyVector2 lhs = from.normalized;
			MyVector2 rhs = to.normalized;
			float num = Mathf.Acos(Mathf.Clamp(Dot(lhs, rhs), -1f, 1f)) * 57.29578f;
			float num2 = Mathf.Sign(lhs.x * rhs.y - lhs.y * rhs.x);
			return num * num2;
		}

		public static float Distance(MyVector2 a, MyVector2 b)
		{
			return (a - b).magnitude;
		}

		public static MyVector2 ClampMagnitude(MyVector2 vector, float maxLength)
		{
			if (vector.sqrMagnitude > maxLength * maxLength)
			{
				return vector.normalized * maxLength;
			}
			return vector;
		}

		public static float SqrMagnitude(MyVector2 a)
		{
			return a.x * a.x + a.y * a.y;
		}

		public float SqrMagnitude()
		{
			return x * x + y * y;
		}

		public static MyVector2 Min(MyVector2 lhs, MyVector2 rhs)
		{
			return new MyVector2(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
		}

		public static MyVector2 Max(MyVector2 lhs, MyVector2 rhs)
		{
			return new MyVector2(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
		}

		[ExcludeFromDocs]
		public static MyVector2 SmoothDamp(MyVector2 current, MyVector2 target, ref MyVector2 currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		[ExcludeFromDocs]
		public static MyVector2 SmoothDamp(MyVector2 current, MyVector2 target, ref MyVector2 currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float maxSpeed = float.PositiveInfinity;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static MyVector2 SmoothDamp(MyVector2 current, MyVector2 target, ref MyVector2 currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
		{
			smoothTime = Mathf.Max(0.0001f, smoothTime);
			float num = 2f / smoothTime;
			float num2 = num * deltaTime;
			float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
			MyVector2 vector = current - target;
			MyVector2 myVector = target;
			float maxLength = maxSpeed * smoothTime;
			vector = ClampMagnitude(vector, maxLength);
			target = current - vector;
			MyVector2 myVector2 = (currentVelocity + num * vector) * deltaTime;
			currentVelocity = (currentVelocity - num * myVector2) * num3;
			MyVector2 myVector3 = target + (vector + myVector2) * num3;
			if (Dot(myVector - current, myVector3 - myVector) > 0f)
			{
				myVector3 = myVector;
				currentVelocity = (myVector3 - myVector) / deltaTime;
			}
			return myVector3;
		}

		public static MyVector2 operator +(MyVector2 a, MyVector2 b)
		{
			return new MyVector2(a.x + b.x, a.y + b.y);
		}

		public static MyVector2 operator -(MyVector2 a, MyVector2 b)
		{
			return new MyVector2(a.x - b.x, a.y - b.y);
		}

		public static MyVector2 operator *(MyVector2 a, MyVector2 b)
		{
			return new MyVector2(a.x * b.x, a.y * b.y);
		}

		public static MyVector2 operator /(MyVector2 a, MyVector2 b)
		{
			return new MyVector2(a.x / b.x, a.y / b.y);
		}

		public static MyVector2 operator -(MyVector2 a)
		{
			return new MyVector2(0f - a.x, 0f - a.y);
		}

		public static MyVector2 operator *(MyVector2 a, float d)
		{
			return new MyVector2(a.x * d, a.y * d);
		}

		public static MyVector2 operator *(float d, MyVector2 a)
		{
			return new MyVector2(a.x * d, a.y * d);
		}

		public static MyVector2 operator /(MyVector2 a, float d)
		{
			return new MyVector2(a.x / d, a.y / d);
		}

		public static bool operator ==(MyVector2 lhs, MyVector2 rhs)
		{
			return (lhs - rhs).sqrMagnitude < 9.99999944E-11f;
		}

		public static bool operator !=(MyVector2 lhs, MyVector2 rhs)
		{
			return !(lhs == rhs);
		}

		public static implicit operator MyVector2(MyVector3 v)
		{
			return new MyVector2(v.x, v.y);
		}

		public static implicit operator MyVector3(MyVector2 v)
		{
			return new MyVector3(v.x, v.y, 0f);
		}
	}
}
