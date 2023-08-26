using System;
using System.Collections.Generic;

[Serializable]
public class RecordingTileMapData
{
	public List<RecordingGridData> m_gridDatas = new List<RecordingGridData>();

	public Dictionary<int, RecordingGridData> m_dicGridDatas = new Dictionary<int, RecordingGridData>();
}
