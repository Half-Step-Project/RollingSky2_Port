using System;
using UnityEngine;
using User.LightingMap;

namespace User.TileMap
{
	[Serializable]
	public class SubstanceData
	{
		public int m_index;

		public int m_uuID;

		public int m_type;

		public int m_id;

		public Vector3 m_position;

		public Vector3 m_rotation;

		public Vector3 m_localScale;

		public byte[] m_saveAttrs;

		public ParentInfo m_parentData;

		public ChildInfo m_tileChildData;

		public ChildInfo m_enemyChildData;

		public LightingMapSubstanceGroupData m_lightingMapSubstanceGroupData;
	}
}
