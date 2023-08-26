using System.Collections.Generic;
using UnityEngine;

namespace User.TileMap
{
	public class TileMapScripTable : ScriptableObject
	{
		public List<GridData> m_gridDatas = new List<GridData>();

		public List<BrushBufferData> m_brushBufferDataList = new List<BrushBufferData>();

		public ProgressData m_progressData;

		public List<RelatedAssetData> m_relatedAsset = new List<RelatedAssetData>();

		public SubstanceUUIDData m_uuidData = new SubstanceUUIDData();
	}
}
