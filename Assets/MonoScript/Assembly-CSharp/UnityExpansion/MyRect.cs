using System;
using UnityEngine;

namespace UnityExpansion
{
	[Serializable]
	public struct MyRect
	{
		private float m_XMin;

		private float m_YMin;

		private float m_Width;

		private float m_Height;

		public static MyRect zero = new MyRect(0f, 0f, 0f, 0f);

		public float x
		{
			get
			{
				return m_XMin;
			}
			set
			{
				m_XMin = value;
			}
		}

		public float y
		{
			get
			{
				return m_YMin;
			}
			set
			{
				m_YMin = value;
			}
		}

		public MyVector2 position
		{
			get
			{
				return new MyVector2(m_XMin, m_YMin);
			}
			set
			{
				m_XMin = value.x;
				m_YMin = value.y;
			}
		}

		public MyVector2 center
		{
			get
			{
				return new MyVector2(x + m_Width / 2f, y + m_Height / 2f);
			}
			set
			{
				m_XMin = value.x - m_Width / 2f;
				m_YMin = value.y - m_Height / 2f;
			}
		}

		public MyVector2 min
		{
			get
			{
				return new MyVector2(xMin, yMin);
			}
			set
			{
				xMin = value.x;
				yMin = value.y;
			}
		}

		public MyVector2 max
		{
			get
			{
				return new MyVector2(xMax, yMax);
			}
			set
			{
				xMax = value.x;
				yMax = value.y;
			}
		}

		public float width
		{
			get
			{
				return m_Width;
			}
			set
			{
				m_Width = value;
			}
		}

		public float height
		{
			get
			{
				return m_Height;
			}
			set
			{
				m_Height = value;
			}
		}

		public MyVector2 size
		{
			get
			{
				return new MyVector2(m_Width, m_Height);
			}
			set
			{
				m_Width = value.x;
				m_Height = value.y;
			}
		}

		public float xMin
		{
			get
			{
				return m_XMin;
			}
			set
			{
				float num = xMax;
				m_XMin = value;
				m_Width = num - m_XMin;
			}
		}

		public float yMin
		{
			get
			{
				return m_YMin;
			}
			set
			{
				float num = yMax;
				m_YMin = value;
				m_Height = num - m_YMin;
			}
		}

		public float xMax
		{
			get
			{
				return m_Width + m_XMin;
			}
			set
			{
				m_Width = value - m_XMin;
			}
		}

		public float yMax
		{
			get
			{
				return m_Height + m_YMin;
			}
			set
			{
				m_Height = value - m_YMin;
			}
		}

		[Obsolete("use xMin")]
		public float left
		{
			get
			{
				return m_XMin;
			}
		}

		[Obsolete("use xMax")]
		public float right
		{
			get
			{
				return m_XMin + m_Width;
			}
		}

		[Obsolete("use yMin")]
		public float top
		{
			get
			{
				return m_YMin;
			}
		}

		[Obsolete("use yMax")]
		public float bottom
		{
			get
			{
				return m_YMin + m_Height;
			}
		}

		public MyRect(float x, float y, float width, float height)
		{
			m_XMin = x;
			m_YMin = y;
			m_Width = width;
			m_Height = height;
		}

		public MyRect(MyVector2 position, MyVector2 size)
		{
			m_XMin = position.x;
			m_YMin = position.y;
			m_Width = size.x;
			m_Height = size.y;
		}

		public MyRect(MyRect source)
		{
			m_XMin = source.m_XMin;
			m_YMin = source.m_YMin;
			m_Width = source.m_Width;
			m_Height = source.m_Height;
		}

		public static MyRect MinMaxRect(float xmin, float ymin, float xmax, float ymax)
		{
			return new MyRect(xmin, ymin, xmax - xmin, ymax - ymin);
		}

		public void Set(float x, float y, float width, float height)
		{
			m_XMin = x;
			m_YMin = y;
			m_Width = width;
			m_Height = height;
		}

		public bool Contains(MyVector2 point)
		{
			if (point.x >= xMin && point.x < xMax && point.y >= yMin)
			{
				return point.y < yMax;
			}
			return false;
		}

		public bool Contains(MyVector3 point)
		{
			if (point.x >= xMin && point.x < xMax && point.y >= yMin)
			{
				return point.y < yMax;
			}
			return false;
		}

		public bool Contains(MyVector3 point, bool allowInverse)
		{
			if (!allowInverse)
			{
				return Contains(point);
			}
			bool flag = false;
			if ((width < 0f && point.x <= xMin && point.x > xMax) || (width >= 0f && point.x >= xMin && point.x < xMax))
			{
				flag = true;
			}
			if (flag && ((height < 0f && point.y <= yMin && point.y > yMax) || (height >= 0f && point.y >= yMin && point.y < yMax)))
			{
				return true;
			}
			return false;
		}

		private static MyRect OrderMinMax(MyRect rect)
		{
			if (rect.xMin > rect.xMax)
			{
				float num = rect.xMin;
				rect.xMin = rect.xMax;
				rect.xMax = num;
			}
			if (rect.yMin > rect.yMax)
			{
				float num2 = rect.yMin;
				rect.yMin = rect.yMax;
				rect.yMax = num2;
			}
			return rect;
		}

		public bool Overlaps(MyRect other)
		{
			if (other.xMax > xMin && other.xMin < xMax && other.yMax > yMin)
			{
				return other.yMin < yMax;
			}
			return false;
		}

		public bool Overlaps(MyRect other, bool allowInverse)
		{
			MyRect rect = this;
			if (allowInverse)
			{
				rect = OrderMinMax(rect);
				other = OrderMinMax(other);
			}
			return rect.Overlaps(other);
		}

		public static MyVector2 NormalizedToPoint(MyRect rectangle, MyVector2 normalizedRectCoordinates)
		{
			return new MyVector2(Mathf.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x), Mathf.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y));
		}

		public static MyVector2 PointToNormalized(MyRect rectangle, MyVector2 point)
		{
			return new MyVector2(Mathf.InverseLerp(rectangle.x, rectangle.xMax, point.x), Mathf.InverseLerp(rectangle.y, rectangle.yMax, point.y));
		}

		public static bool operator !=(MyRect lhs, MyRect rhs)
		{
			return !(lhs == rhs);
		}

		public static bool operator ==(MyRect lhs, MyRect rhs)
		{
			if (lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width)
			{
				return lhs.height == rhs.height;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ (width.GetHashCode() << 2) ^ (y.GetHashCode() >> 2) ^ (height.GetHashCode() >> 1);
		}

		public override bool Equals(object other)
		{
			if (!(other is MyRect))
			{
				return false;
			}
			MyRect myRect = (MyRect)other;
			if (x.Equals(myRect.x) && y.Equals(myRect.y) && width.Equals(myRect.width))
			{
				return height.Equals(myRect.height);
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", x, y, width, height);
		}

		public string ToString(string format)
		{
			return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})", x.ToString(format), y.ToString(format), width.ToString(format), height.ToString(format));
		}
	}
}
