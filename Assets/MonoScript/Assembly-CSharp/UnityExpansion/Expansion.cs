using UnityEngine;

namespace UnityExpansion
{
	public static class Expansion
	{
		public static MyVector2 ToMyVector2(this Vector2 value)
		{
			return new MyVector2(value.x, value.y);
		}

		public static Vector2 ToVector2(this MyVector2 value)
		{
			return new Vector2(value.x, value.y);
		}

		public static MyVector3 ToMyVector3(this Vector3 value)
		{
			return new MyVector3(value.x, value.y, value.z);
		}

		public static Vector3 ToVector3(this MyVector3 value)
		{
			return new Vector3(value.x, value.y, value.z);
		}

		public static MyVector4 ToMyVector4(this Vector4 value)
		{
			return new MyVector4(value.x, value.y, value.z, value.w);
		}

		public static Vector4 ToVector4(this MyVector4 value)
		{
			return new Vector4(value.x, value.y, value.z, value.w);
		}

		public static MyQuaternion ToMyQuaternion(this Quaternion value)
		{
			return new MyQuaternion(value.x, value.y, value.z, value.w);
		}

		public static Quaternion ToQuaternion(this MyQuaternion value)
		{
			return new Quaternion(value.x, value.y, value.z, value.w);
		}

		public static MyRect ToMyRect(this Rect value)
		{
			return new MyRect(value.x, value.y, value.width, value.height);
		}

		public static Rect ToRect(this MyRect value)
		{
			return new Rect(value.x, value.y, value.width, value.height);
		}

		public static MyColor ToMyColor(this Color value)
		{
			return new MyColor(value.r, value.g, value.b, value.a);
		}

		public static Color ToColor(this MyColor value)
		{
			return new Color(value.r, value.g, value.b, value.a);
		}

		public static MyColor32 ToMyColor32(this Color32 value)
		{
			return new MyColor32(value.r, value.g, value.b, value.a);
		}

		public static Color32 ToColor32(this MyColor32 value)
		{
			return new Color32(value.r, value.g, value.b, value.a);
		}

		public static MyMatrix4x4 ToMyMatrix4x4(Matrix4x4 value)
		{
			MyMatrix4x4 result = default(MyMatrix4x4);
			result.m00 = value.m00;
			result.m01 = value.m01;
			result.m02 = value.m02;
			result.m03 = value.m03;
			result.m10 = value.m10;
			result.m11 = value.m11;
			result.m12 = value.m12;
			result.m13 = value.m13;
			result.m20 = value.m20;
			result.m21 = value.m21;
			result.m22 = value.m22;
			result.m23 = value.m23;
			result.m30 = value.m30;
			result.m31 = value.m31;
			result.m32 = value.m32;
			result.m33 = value.m33;
			return result;
		}

		public static Matrix4x4 ToMatrix4x4(MyMatrix4x4 value)
		{
			Matrix4x4 result = default(Matrix4x4);
			result.m00 = value.m00;
			result.m01 = value.m01;
			result.m02 = value.m02;
			result.m03 = value.m03;
			result.m10 = value.m10;
			result.m11 = value.m11;
			result.m12 = value.m12;
			result.m13 = value.m13;
			result.m20 = value.m20;
			result.m21 = value.m21;
			result.m22 = value.m22;
			result.m23 = value.m23;
			result.m30 = value.m30;
			result.m31 = value.m31;
			result.m32 = value.m32;
			result.m33 = value.m33;
			return result;
		}

		public static MyBounds ToMyBounds(this Bounds value)
		{
			return new MyBounds(value.center.ToMyVector3(), value.size.ToMyVector3());
		}

		public static Bounds ToBounds(this MyBounds value)
		{
			return new Bounds(value.center.ToVector3(), value.size.ToVector3());
		}
	}
}
