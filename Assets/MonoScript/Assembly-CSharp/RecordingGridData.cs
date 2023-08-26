using System;
using System.Collections.Generic;

[Serializable]
public class RecordingGridData
{
	public int m_gridId;

	public List<RecordingRowData> m_recordingRowData = new List<RecordingRowData>();

	public Dictionary<int, RecordingRowData> m_dicRowDatas = new Dictionary<int, RecordingRowData>();
}
