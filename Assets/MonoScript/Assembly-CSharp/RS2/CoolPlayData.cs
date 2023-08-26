using UnityEngine.Events;

namespace RS2
{
	public class CoolPlayData
	{
		public enum OpenType
		{
			NONE,
			BUY,
			INFO
		}

		private int m_shopId;

		private OpenType m_type;

		private UnityAction m_CallBack;

		public int ShopId
		{
			get
			{
				return m_shopId;
			}
			set
			{
				m_shopId = value;
			}
		}

		public OpenType Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}

		public UnityAction CallBack
		{
			get
			{
				return m_CallBack;
			}
			set
			{
				m_CallBack = value;
			}
		}
	}
}
