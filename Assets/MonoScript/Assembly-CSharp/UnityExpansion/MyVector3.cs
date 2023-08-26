using System;
using UnityEngine;
using UnityEngine.Internal;

namespace UnityExpansion
{
	[Serializable]
	public struct MyVector3
	{
		public const float kEpsilon = 1E-05f;

		public float x;

		public float y;

		public float z;

		private static readonly MyVector3 zeroVector = new MyVector3(0f, 0f, 0f);

		private static readonly MyVector3 oneVector = new MyVector3(1f, 1f, 1f);

		private static readonly MyVector3 upVector = new MyVector3(0f, 1f, 0f);

		private static readonly MyVector3 downVector = new MyVector3(0f, -1f, 0f);

		private static readonly MyVector3 leftVector = new MyVector3(-1f, 0f, 0f);

		private static readonly MyVector3 rightVector = new MyVector3(1f, 0f, 0f);

		private static readonly MyVector3 forwardVector = new MyVector3(0f, 0f, 1f);

		private static readonly MyVector3 backVector = new MyVector3(0f, 0f, -1f);

		private static readonly MyVector3 positiveInfinityVector = new MyVector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		private static readonly MyVector3 negativeInfinityVector = new MyVector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

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
				default:
					throw new IndexOutOfRangeException("Invalid Vector3 index!");
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
				default:
					throw new IndexOutOfRangeException("Invalid Vector3 index!");
				}
			}
		}

		public MyVector3 normalized
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
				return Mathf.Sqrt(x * x + y * y + z * z);
			}
		}

		public float sqrMagnitude
		{
			get
			{
				return x * x + y * y + z * z;
			}
		}

		public static MyVector3 zero
		{
			get
			{
				return zeroVector;
			}
		}

		public static MyVector3 one
		{
			get
			{
				return oneVector;
			}
		}

		public static MyVector3 forward
		{
			get
			{
				return forwardVector;
			}
		}

		public static MyVector3 back
		{
			get
			{
				return backVector;
			}
		}

		public static MyVector3 up
		{
			get
			{
				return upVector;
			}
		}

		public static MyVector3 down
		{
			get
			{
				return downVector;
			}
		}

		public static MyVector3 left
		{
			get
			{
				return leftVector;
			}
		}

		public static MyVector3 right
		{
			get
			{
				return rightVector;
			}
		}

		public static MyVector3 positiveInfinity
		{
			get
			{
				return positiveInfinityVector;
			}
		}

		public static MyVector3 negativeInfinity
		{
			get
			{
				return negativeInfinityVector;
			}
		}

		[Obsolete("Use Vector3.forward instead.")]
		public static MyVector3 fwd
		{
			get
			{
				return new MyVector3(0f, 0f, 1f);
			}
		}

		public static MyVector3 Lerp(MyVector3 a, MyVector3 b, float t)
		{
			t = Mathf.Clamp01(t);
			return new MyVector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
		}

		public static MyVector3 LerpUnclamped(MyVector3 a, MyVector3 b, float t)
		{
			return new MyVector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
		}

		public static MyVector3 MoveTowards(MyVector3 current, MyVector3 target, float maxDistanceDelta)
		{
			MyVector3 myVector = target - current;
			float num = myVector.magnitude;
			if (num <= maxDistanceDelta || num < float.Epsilon)
			{
				return target;
			}
			return current + myVector / num * maxDistanceDelta;
		}

		[ExcludeFromDocs]
		public static MyVector3 SmoothDamp(MyVector3 current, MyVector3 target, ref MyVector3 currentVelocity, float smoothTime, float maxSpeed)
		{
			float deltaTime = Time.deltaTime;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		[ExcludeFromDocs]
		public static MyVector3 SmoothDamp(MyVector3 current, MyVector3 target, ref MyVector3 currentVelocity, float smoothTime)
		{
			float deltaTime = Time.deltaTime;
			float maxSpeed = float.PositiveInfinity;
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		public static MyVector3 SmoothDamp(MyVector3 current, MyVector3 target, ref MyVector3 currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
		{
			smoothTime = Mathf.Max(0.0001f, smoothTime);
			float num = 2f / smoothTime;
			float num2 = num * deltaTime;
			float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
			MyVector3 vector = current - target;
			MyVector3 myVector = target;
			float maxLength = maxSpeed * smoothTime;
			vector = ClampMagnitude(vector, maxLength);
			target = current - vector;
			MyVector3 myVector2 = (currentVelocity + num * vector) * deltaTime;
			currentVelocity = (currentVelocity - num * myVector2) * num3;
			MyVector3 myVector3 = target + (vector + myVector2) * num3;
			if (Dot(myVector - current, myVector3 - myVector) > 0f)
			{
				myVector3 = myVector;
				currentVelocity = (myVector3 - myVector) / deltaTime;
			}
			return myVector3;
		}

		public MyVector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public MyVector3(float x, float y)
		{
			this.x = x;
			this.y = y;
			z = 0f;
		}

		public void Set(float newX, float newY, float newZ)
		{
			x = newX;
			y = newY;
			z = newZ;
		}

		public static MyVector3 Scale(MyVector3 a, MyVector3 b)
		{
			return new MyVector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public void Scale(MyVector3 scale)
		{
			x *= scale.x;
			y *= scale.y;
			z *= scale.z;
		}

		public static MyVector3 Cross(MyVector3 lhs, MyVector3 rhs)
		{
			return new MyVector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
		}

		public override bool Equals(object other)
		{
			if (!(other is MyVector3))
			{
				return false;
			}
			MyVector3 myVector = (MyVector3)other;
			if (x.Equals(myVector.x) && y.Equals(myVector.y))
			{
				return z.Equals(myVector.z);
			}
			return false;
		}

		public static MyVector3 Reflect(MyVector3 inDirection, MyVector3 inNormal)
		{
			return -2f * Dot(inNormal, inDirection) * inNormal + inDirection;
		}

		public static MyVector3 Normalize(MyVector3 value)
		{
			float num = Magnitude(value);
			if (num > 1E-05f)
			{
				return value / num;
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

		public static float Dot(MyVector3 lhs, MyVector3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

		public static MyVector3 Project(MyVector3 vector, MyVector3 onNormal)
		{
			float num = Dot(onNormal, onNormal);
			if (num < Mathf.Epsilon)
			{
				return zero;
			}
			return onNormal * Dot(vector, onNormal) / num;
		}

		public static MyVector3 ProjectOnPlane(MyVector3 vector, MyVector3 planeNormal)
		{
			return vector - Project(vector, planeNormal);
		}

		public static float Angle(MyVector3 from, MyVector3 to)
		{
			return Mathf.Acos(Mathf.Clamp(Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
		}

		public static float SignedAngle(MyVector3 from, MyVector3 to, MyVector3 axis)
		{
			MyVector3 lhs = from.normalized;
			MyVector3 rhs = to.normalized;
			float num = Mathf.Acos(Mathf.Clamp(Dot(lhs, rhs), -1f, 1f)) * 57.29578f;
			float num2 = Mathf.Sign(Dot(axis, Cross(lhs, rhs)));
			return num * num2;
		}

		public static float Distance(MyVector3 a, MyVector3 b)
		{
			MyVector3 myVector = new MyVector3(a.x - b.x, a.y - b.y, a.z - b.z);
			return Mathf.Sqrt(myVector.x * myVector.x + myVector.y * myVector.y + myVector.z * myVector.z);
		}

		public static MyVector3 ClampMagnitude(MyVector3 vector, float maxLength)
		{
			if (vector.sqrMagnitude > maxLength * maxLength)
			{
				return vector.normalized * maxLength;
			}
			return vector;
		}

		public static float Magnitude(MyVector3 vector)
		{
			return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
		}

		public static float SqrMagnitude(MyVector3 vector)
		{
			return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
		}

		public static MyVector3 Min(MyVector3 lhs, MyVector3 rhs)
		{
			return new MyVector3(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z));
		}

		public static MyVector3 Max(MyVector3 lhs, MyVector3 rhs)
		{
			return new MyVector3(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z));
		}

		public static MyVector3 operator +(MyVector3 a, MyVector3 b)
		{
			return new MyVector3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static MyVector3 operator -(MyVector3 a, MyVector3 b)
		{
			return new MyVector3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static MyVector3 operator -(MyVector3 a)
		{
			return new MyVector3(0f - a.x, 0f - a.y, 0f - a.z);
		}

		public static MyVector3 operator *(MyVector3 a, float d)
		{
			return new MyVector3(a.x * d, a.y * d, a.z * d);
		}

		public static MyVector3 operator *(float d, MyVector3 a)
		{
			return new MyVector3(a.x * d, a.y * d, a.z * d);
		}

		public static MyVector3 operator /(MyVector3 a, float d)
		{
			return new MyVector3(a.x / d, a.y / d, a.z / d);
		}

		public static bool operator ==(MyVector3 lhs, MyVector3 rhs)
		{
			return SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
		}

		public static bool operator !=(MyVector3 lhs, MyVector3 rhs)
		{
			return !(lhs == rhs);
		}

		public override string ToString()
		{
			return string.Format("({0:F1}, {1:F1}, {2:F1})", x, y, z);
		}

		public string ToString(string format)
		{
			return string.Format("({0}, {1}, {2})", x.ToString(format), y.ToString(format), z.ToString(format));
		}

		[Obsolete("Use Vector3.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
		public static float AngleBetween(MyVector3 from, MyVector3 to)
		{
			return Mathf.Acos(Mathf.Clamp(Dot(from.normalized, to.normalized), -1f, 1f));
		}

		[Obsolete("Use Vector3.ProjectOnPlane instead.")]
		public static MyVector3 Exclude(MyVector3 excludeThis, MyVector3 fromThat)
		{
			return ProjectOnPlane(fromThat, excludeThis);
		}
	}
}
