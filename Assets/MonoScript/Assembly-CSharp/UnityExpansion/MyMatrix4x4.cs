using System;

namespace UnityExpansion
{
	[Serializable]
	public struct MyMatrix4x4
	{
		public float m00;

		public float m10;

		public float m20;

		public float m30;

		public float m01;

		public float m11;

		public float m21;

		public float m31;

		public float m02;

		public float m12;

		public float m22;

		public float m32;

		public float m03;

		public float m13;

		public float m23;

		public float m33;

		private static readonly MyMatrix4x4 zeroMatrix = new MyMatrix4x4(new MyVector4(0f, 0f, 0f, 0f), new MyVector4(0f, 0f, 0f, 0f), new MyVector4(0f, 0f, 0f, 0f), new MyVector4(0f, 0f, 0f, 0f));

		private static readonly MyMatrix4x4 identityMatrix = new MyMatrix4x4(new MyVector4(1f, 0f, 0f, 0f), new MyVector4(0f, 1f, 0f, 0f), new MyVector4(0f, 0f, 1f, 0f), new MyVector4(0f, 0f, 0f, 1f));

		public float this[int row, int column]
		{
			get
			{
				return this[row + column * 4];
			}
			set
			{
				this[row + column * 4] = value;
			}
		}

		public float this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return m00;
				case 1:
					return m10;
				case 2:
					return m20;
				case 3:
					return m30;
				case 4:
					return m01;
				case 5:
					return m11;
				case 6:
					return m21;
				case 7:
					return m31;
				case 8:
					return m02;
				case 9:
					return m12;
				case 10:
					return m22;
				case 11:
					return m32;
				case 12:
					return m03;
				case 13:
					return m13;
				case 14:
					return m23;
				case 15:
					return m33;
				default:
					throw new IndexOutOfRangeException("Invalid matrix index!");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					m00 = value;
					break;
				case 1:
					m10 = value;
					break;
				case 2:
					m20 = value;
					break;
				case 3:
					m30 = value;
					break;
				case 4:
					m01 = value;
					break;
				case 5:
					m11 = value;
					break;
				case 6:
					m21 = value;
					break;
				case 7:
					m31 = value;
					break;
				case 8:
					m02 = value;
					break;
				case 9:
					m12 = value;
					break;
				case 10:
					m22 = value;
					break;
				case 11:
					m32 = value;
					break;
				case 12:
					m03 = value;
					break;
				case 13:
					m13 = value;
					break;
				case 14:
					m23 = value;
					break;
				case 15:
					m33 = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid matrix index!");
				}
			}
		}

		public static MyMatrix4x4 zero
		{
			get
			{
				return zeroMatrix;
			}
		}

		public static MyMatrix4x4 identity
		{
			get
			{
				return identityMatrix;
			}
		}

		public MyMatrix4x4(MyVector4 column0, MyVector4 column1, MyVector4 column2, MyVector4 column3)
		{
			m00 = column0.x;
			m01 = column1.x;
			m02 = column2.x;
			m03 = column3.x;
			m10 = column0.y;
			m11 = column1.y;
			m12 = column2.y;
			m13 = column3.y;
			m20 = column0.z;
			m21 = column1.z;
			m22 = column2.z;
			m23 = column3.z;
			m30 = column0.w;
			m31 = column1.w;
			m32 = column2.w;
			m33 = column3.w;
		}

		public override int GetHashCode()
		{
			return GetColumn(0).GetHashCode() ^ (GetColumn(1).GetHashCode() << 2) ^ (GetColumn(2).GetHashCode() >> 2) ^ (GetColumn(3).GetHashCode() >> 1);
		}

		public override bool Equals(object other)
		{
			if (!(other is MyMatrix4x4))
			{
				return false;
			}
			MyMatrix4x4 myMatrix4x = (MyMatrix4x4)other;
			if (GetColumn(0).Equals(myMatrix4x.GetColumn(0)) && GetColumn(1).Equals(myMatrix4x.GetColumn(1)) && GetColumn(2).Equals(myMatrix4x.GetColumn(2)))
			{
				return GetColumn(3).Equals(myMatrix4x.GetColumn(3));
			}
			return false;
		}

		public static MyMatrix4x4 operator *(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
		{
			MyMatrix4x4 result = default(MyMatrix4x4);
			result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
			result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
			result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
			result.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;
			result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
			result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
			result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
			result.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;
			result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
			result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
			result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
			result.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;
			result.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
			result.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
			result.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
			result.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;
			return result;
		}

		public static MyVector4 operator *(MyMatrix4x4 lhs, MyVector4 vector)
		{
			MyVector4 result = default(MyVector4);
			result.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z + lhs.m03 * vector.w;
			result.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z + lhs.m13 * vector.w;
			result.z = lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z + lhs.m23 * vector.w;
			result.w = lhs.m30 * vector.x + lhs.m31 * vector.y + lhs.m32 * vector.z + lhs.m33 * vector.w;
			return result;
		}

		public static bool operator ==(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
		{
			if (lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2))
			{
				return lhs.GetColumn(3) == rhs.GetColumn(3);
			}
			return false;
		}

		public static bool operator !=(MyMatrix4x4 lhs, MyMatrix4x4 rhs)
		{
			return !(lhs == rhs);
		}

		public MyVector4 GetColumn(int index)
		{
			switch (index)
			{
			case 0:
				return new MyVector4(m00, m10, m20, m30);
			case 1:
				return new MyVector4(m01, m11, m21, m31);
			case 2:
				return new MyVector4(m02, m12, m22, m32);
			case 3:
				return new MyVector4(m03, m13, m23, m33);
			default:
				throw new IndexOutOfRangeException("Invalid column index!");
			}
		}

		public MyVector4 GetRow(int index)
		{
			switch (index)
			{
			case 0:
				return new MyVector4(m00, m01, m02, m03);
			case 1:
				return new MyVector4(m10, m11, m12, m13);
			case 2:
				return new MyVector4(m20, m21, m22, m23);
			case 3:
				return new MyVector4(m30, m31, m32, m33);
			default:
				throw new IndexOutOfRangeException("Invalid row index!");
			}
		}

		public void SetColumn(int index, MyVector4 column)
		{
			this[0, index] = column.x;
			this[1, index] = column.y;
			this[2, index] = column.z;
			this[3, index] = column.w;
		}

		public void SetRow(int index, MyVector4 row)
		{
			this[index, 0] = row.x;
			this[index, 1] = row.y;
			this[index, 2] = row.z;
			this[index, 3] = row.w;
		}

		public MyVector3 MultiplyPoint(MyVector3 point)
		{
			MyVector3 result = default(MyVector3);
			result.x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
			result.y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
			result.z = m20 * point.x + m21 * point.y + m22 * point.z + m23;
			float num = m30 * point.x + m31 * point.y + m32 * point.z + m33;
			num = 1f / num;
			result.x *= num;
			result.y *= num;
			result.z *= num;
			return result;
		}

		public MyVector3 MultiplyPoint3x4(MyVector3 point)
		{
			MyVector3 result = default(MyVector3);
			result.x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
			result.y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
			result.z = m20 * point.x + m21 * point.y + m22 * point.z + m23;
			return result;
		}

		public MyVector3 MultiplyVector(MyVector3 vector)
		{
			MyVector3 result = default(MyVector3);
			result.x = m00 * vector.x + m01 * vector.y + m02 * vector.z;
			result.y = m10 * vector.x + m11 * vector.y + m12 * vector.z;
			result.z = m20 * vector.x + m21 * vector.y + m22 * vector.z;
			return result;
		}

		public static MyMatrix4x4 Scale(MyVector3 vector)
		{
			MyMatrix4x4 result = default(MyMatrix4x4);
			result.m00 = vector.x;
			result.m01 = 0f;
			result.m02 = 0f;
			result.m03 = 0f;
			result.m10 = 0f;
			result.m11 = vector.y;
			result.m12 = 0f;
			result.m13 = 0f;
			result.m20 = 0f;
			result.m21 = 0f;
			result.m22 = vector.z;
			result.m23 = 0f;
			result.m30 = 0f;
			result.m31 = 0f;
			result.m32 = 0f;
			result.m33 = 1f;
			return result;
		}

		public static MyMatrix4x4 Translate(MyVector3 vector)
		{
			MyMatrix4x4 result = default(MyMatrix4x4);
			result.m00 = 1f;
			result.m01 = 0f;
			result.m02 = 0f;
			result.m03 = vector.x;
			result.m10 = 0f;
			result.m11 = 1f;
			result.m12 = 0f;
			result.m13 = vector.y;
			result.m20 = 0f;
			result.m21 = 0f;
			result.m22 = 1f;
			result.m23 = vector.z;
			result.m30 = 0f;
			result.m31 = 0f;
			result.m32 = 0f;
			result.m33 = 1f;
			return result;
		}

		public static MyMatrix4x4 Rotate(MyQuaternion q)
		{
			float num = q.x * 2f;
			float num2 = q.y * 2f;
			float num3 = q.z * 2f;
			float num4 = q.x * num;
			float num5 = q.y * num2;
			float num6 = q.z * num3;
			float num7 = q.x * num2;
			float num8 = q.x * num3;
			float num9 = q.y * num3;
			float num10 = q.w * num;
			float num11 = q.w * num2;
			float num12 = q.w * num3;
			MyMatrix4x4 result = default(MyMatrix4x4);
			result.m00 = 1f - (num5 + num6);
			result.m10 = num7 + num12;
			result.m20 = num8 - num11;
			result.m30 = 0f;
			result.m01 = num7 - num12;
			result.m11 = 1f - (num4 + num6);
			result.m21 = num9 + num10;
			result.m31 = 0f;
			result.m02 = num8 + num11;
			result.m12 = num9 - num10;
			result.m22 = 1f - (num4 + num5);
			result.m32 = 0f;
			result.m03 = 0f;
			result.m13 = 0f;
			result.m23 = 0f;
			result.m33 = 1f;
			return result;
		}

		public override string ToString()
		{
			return string.Format("{0:F5}\t{1:F5}\t{2:F5}\t{3:F5}\n{4:F5}\t{5:F5}\t{6:F5}\t{7:F5}\n{8:F5}\t{9:F5}\t{10:F5}\t{11:F5}\n{12:F5}\t{13:F5}\t{14:F5}\t{15:F5}\n", m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33);
		}

		public string ToString(string format)
		{
			return string.Format("{0}\t{1}\t{2}\t{3}\n{4}\t{5}\t{6}\t{7}\n{8}\t{9}\t{10}\t{11}\n{12}\t{13}\t{14}\t{15}\n", m00.ToString(format), m01.ToString(format), m02.ToString(format), m03.ToString(format), m10.ToString(format), m11.ToString(format), m12.ToString(format), m13.ToString(format), m20.ToString(format), m21.ToString(format), m22.ToString(format), m23.ToString(format), m30.ToString(format), m31.ToString(format), m32.ToString(format), m33.ToString(format));
		}
	}
}
