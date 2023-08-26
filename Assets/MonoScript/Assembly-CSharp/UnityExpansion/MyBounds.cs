using System;

namespace UnityExpansion
{
	[Serializable]
	public struct MyBounds
	{
		private MyVector3 m_Center;

		private MyVector3 m_Extents;

		public MyVector3 center
		{
			get
			{
				return m_Center;
			}
			set
			{
				m_Center = value;
			}
		}

		public MyVector3 size
		{
			get
			{
				return m_Extents * 2f;
			}
			set
			{
				m_Extents = value * 0.5f;
			}
		}

		public MyVector3 extents
		{
			get
			{
				return m_Extents;
			}
			set
			{
				m_Extents = value;
			}
		}

		public MyVector3 min
		{
			get
			{
				return center - extents;
			}
			set
			{
				SetMinMax(value, max);
			}
		}

		public MyVector3 max
		{
			get
			{
				return center + extents;
			}
			set
			{
				SetMinMax(min, value);
			}
		}

		public MyBounds(MyVector3 center, MyVector3 size)
		{
			m_Center = center;
			m_Extents = size * 0.5f;
		}

		public override int GetHashCode()
		{
			return center.GetHashCode() ^ (extents.GetHashCode() << 2);
		}

		public override bool Equals(object other)
		{
			if (!(other is MyBounds))
			{
				return false;
			}
			MyBounds myBounds = (MyBounds)other;
			if (center.Equals(myBounds.center))
			{
				return extents.Equals(myBounds.extents);
			}
			return false;
		}

		public static bool operator ==(MyBounds lhs, MyBounds rhs)
		{
			if (lhs.center == rhs.center)
			{
				return lhs.extents == rhs.extents;
			}
			return false;
		}

		public static bool operator !=(MyBounds lhs, MyBounds rhs)
		{
			return !(lhs == rhs);
		}

		public void SetMinMax(MyVector3 min, MyVector3 max)
		{
			extents = (max - min) * 0.5f;
			center = min + extents;
		}

		public void Encapsulate(MyVector3 point)
		{
			SetMinMax(MyVector3.Min(min, point), MyVector3.Max(max, point));
		}

		public void Encapsulate(MyBounds bounds)
		{
			Encapsulate(bounds.center - bounds.extents);
			Encapsulate(bounds.center + bounds.extents);
		}

		public void Expand(float amount)
		{
			amount *= 0.5f;
			extents += new MyVector3(amount, amount, amount);
		}

		public void Expand(MyVector3 amount)
		{
			extents += amount * 0.5f;
		}

		public bool Intersects(MyBounds bounds)
		{
			if (min.x <= bounds.max.x && max.x >= bounds.min.x && min.y <= bounds.max.y && max.y >= bounds.min.y && min.z <= bounds.max.z)
			{
				return max.z >= bounds.min.z;
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("Center: {0}, Extents: {1}", m_Center, m_Extents);
		}

		public string ToString(string format)
		{
			return string.Format("Center: {0}, Extents: {1}", m_Center.ToString(format), m_Extents.ToString(format));
		}
	}
}
