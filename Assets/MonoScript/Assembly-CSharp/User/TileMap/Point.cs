using System;
using UnityEngine;

namespace User.TileMap
{
	[Serializable]
	public struct Point : IEquatable<Point>
	{
		public int m_x;

		public int m_y;

		public static Point zero
		{
			get
			{
				return new Point(0, 0);
			}
		}

		public static Point DefaultValue
		{
			get
			{
				return new Point(-1, -1);
			}
		}

		public Point(int x, int y)
		{
			m_x = x;
			m_y = y;
		}

		public static float Distance(Point a, Point b)
		{
			Point point = new Point(a.m_x - b.m_x, a.m_y - b.m_y);
			return Mathf.Sqrt(point.m_x * point.m_x + point.m_y * point.m_y);
		}

		public override string ToString()
		{
			return "Point(" + m_x + "," + m_y + ")";
		}

		public static bool operator ==(Point point1, Point point2)
		{
			return point1.Equals(point2);
		}

		public static bool operator !=(Point point1, Point point2)
		{
			return !point1.Equals(point2);
		}

		public static Point operator +(Point point1, Point point2)
		{
			return new Point(point1.m_x + point2.m_x, point1.m_y + point2.m_y);
		}

		public override int GetHashCode()
		{
			return m_x.GetHashCode() ^ m_y.GetHashCode();
		}

		public bool Equals(Point other)
		{
			if (m_x == other.m_x && m_y == other.m_y)
			{
				return true;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is Point)
			{
				return Equals((Point)obj);
			}
			return false;
		}
	}
}
