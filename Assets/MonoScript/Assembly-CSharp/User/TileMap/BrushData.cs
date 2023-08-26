using System;

namespace User.TileMap
{
	[Serializable]
	public class BrushData
	{
		public int m_id;

		public string m_resourcesPath;

		public Type m_typeClass;

		public BrushData(int id, string resourcesPath, Type typeClass)
		{
			m_id = id;
			m_resourcesPath = resourcesPath;
			m_typeClass = typeClass;
		}

		public override string ToString()
		{
			return "BrushData(" + m_id + "," + m_resourcesPath + "," + m_typeClass.ToString() + ")";
		}
	}
}
