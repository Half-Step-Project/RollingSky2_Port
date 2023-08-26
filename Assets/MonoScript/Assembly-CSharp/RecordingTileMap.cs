using System.Collections.Generic;
using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class RecordingTileMap : MonoBehaviour
{
	public class RecordingGridCellData
	{
		public int m_gridId;

		public Point m_point;

		public override string ToString()
		{
			return "Grid : m_gridId=" + m_gridId + ", Point : " + m_point;
		}
	}

	public bool m_isRecording;

	public RecordingTileMapData m_recordingTileMapData;

	public bool m_isHaveRecording = true;

	public int m_targetGridID;

	public int m_targetRowNumber = 30;

	private RecordingGridCellData m_currentRecordingGridCellData;

	private RecordingGridCellData m_lastRecordingGridCellData;

	public static RecordingTileMap Instance;

	public RecordingRowData GetRecordingRowDataByGridIdAddRowNumber(int gridId, int rowNumber)
	{
		if (m_recordingTileMapData == null)
		{
			return null;
		}
		for (int i = 0; i < m_recordingTileMapData.m_gridDatas.Count; i++)
		{
			if (m_recordingTileMapData.m_gridDatas[i].m_gridId != gridId)
			{
				continue;
			}
			for (int j = 0; j < m_recordingTileMapData.m_gridDatas[i].m_recordingRowData.Count; j++)
			{
				if (m_recordingTileMapData.m_gridDatas[i].m_recordingRowData[j].m_row == rowNumber)
				{
					return m_recordingTileMapData.m_gridDatas[i].m_recordingRowData[j];
				}
			}
		}
		return null;
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (!m_isRecording)
		{
			return;
		}
		m_currentRecordingGridCellData = GetRecordingGridCellData(MapController.Instance.GetGrids, BaseRole.BallPosition);
		if (m_lastRecordingGridCellData == null)
		{
			if (m_currentRecordingGridCellData != null)
			{
				Recording();
				m_lastRecordingGridCellData = m_currentRecordingGridCellData;
			}
		}
		else if (m_currentRecordingGridCellData != null)
		{
			if (m_currentRecordingGridCellData.m_gridId != m_lastRecordingGridCellData.m_gridId || m_currentRecordingGridCellData.m_point != m_lastRecordingGridCellData.m_point)
			{
				Recording();
				m_lastRecordingGridCellData = m_currentRecordingGridCellData;
			}
		}
		else
		{
			m_lastRecordingGridCellData = m_currentRecordingGridCellData;
		}
	}

	private void Recording()
	{
		RecordingGridData recordingGridData = null;
		for (int i = 0; i < m_recordingTileMapData.m_gridDatas.Count; i++)
		{
			if (m_recordingTileMapData.m_gridDatas[i].m_gridId == m_currentRecordingGridCellData.m_gridId)
			{
				recordingGridData = m_recordingTileMapData.m_gridDatas[i];
				break;
			}
		}
		if (recordingGridData == null)
		{
			recordingGridData = new RecordingGridData();
			recordingGridData.m_gridId = m_currentRecordingGridCellData.m_gridId;
			recordingGridData.m_recordingRowData = new List<RecordingRowData>();
			RecordingRowData rowData = GetRowData(m_currentRecordingGridCellData);
			recordingGridData.m_recordingRowData.Add(rowData);
			m_recordingTileMapData.m_gridDatas.Add(recordingGridData);
		}
		else
		{
			RecordingRowData rowData2 = GetRowData(m_currentRecordingGridCellData);
			recordingGridData.m_recordingRowData.Add(rowData2);
		}
	}

	private RecordingRowData GetRowData(RecordingGridCellData cellData)
	{
		RecordingRowData recordingRowData = new RecordingRowData();
		recordingRowData.m_gridId = cellData.m_gridId;
		recordingRowData.m_row = cellData.m_point.m_y;
		recordingRowData.m_data = RebirthBoxTrigger.GetRebirthBoxData(recordingRowData.m_gridId, cellData.m_point, MapController.Instance.GetGridById(recordingRowData.m_gridId).transform, BaseRole.theBall.transform);
		return recordingRowData;
	}

	public RecordingGridCellData GetRecordingGridCellData(List<Grid> grids, Vector3 position)
	{
		RecordingGridCellData recordingGridCellData = null;
		for (int i = 0; i < grids.Count; i++)
		{
			if (grids[i].IsOnGrid(position))
			{
				recordingGridCellData = new RecordingGridCellData();
				recordingGridCellData.m_gridId = grids[i].m_id;
				recordingGridCellData.m_point = grids[i].GetPointByPosition(position);
				break;
			}
		}
		return recordingGridCellData;
	}
}
