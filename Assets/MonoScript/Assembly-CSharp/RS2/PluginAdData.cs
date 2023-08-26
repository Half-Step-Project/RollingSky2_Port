using UnityEngine.Events;

namespace RS2
{
	public class PluginAdData
	{
		private int m_pluginId = -1;

		private bool m_isTutorial;

		private UnityAction m_endHandler;

		public bool Tutorial
		{
			get
			{
				return m_isTutorial;
			}
			set
			{
				m_isTutorial = value;
			}
		}

		public int PluginId
		{
			get
			{
				return m_pluginId;
			}
			set
			{
				m_pluginId = value;
			}
		}

		public UnityAction EndHandler
		{
			get
			{
				return m_endHandler;
			}
			set
			{
				m_endHandler = value;
			}
		}
	}
}
