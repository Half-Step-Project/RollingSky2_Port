using System;
using System.Collections.Generic;
using UnityEngine;

namespace User.TileMap
{
	[Serializable]
	public class GridData
	{
		public int m_id;

		public List<int> m_parentIds = new List<int>();

		public float m_samplingInterval = 3f;

		public float m_samplingCenterY;

		public int m_beforeTheLength;

		public Vector3 m_position;

		public Vector3 m_rotation;

		public int m_height;

		public int m_width;

		public List<GridCellData> m_gridCellDatas = new List<GridCellData>();

		[NonSerialized]
		public Dictionary<Point, GridCellData> m_gridCellDatasDictionary = new Dictionary<Point, GridCellData>();
	}
}
