using Foundation;
using UnityEngine;

public class GridController : IOriginRebirth
{
	public struct RebirthData
	{
		public RD_ElementTransform_DATA[] m_transforms;
	}

	private MapController m_mapController;

	public bool IsRecordOriginRebirth
	{
		get
		{
			return true;
		}
	}

	public GridController(MapController mapController)
	{
		m_mapController = mapController;
	}

	public void Shutdown()
	{
	}

	public object GetOriginRebirthData(object obj = null)
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.m_transforms = new RD_ElementTransform_DATA[m_mapController.GetGrids.Count];
		for (int i = 0; i < m_mapController.GetGrids.Count; i++)
		{
			rebirthData.m_transforms[i] = m_mapController.GetGrids[i].gameObject.transform.GetTransData();
		}
		return JsonUtility.ToJson(rebirthData);
	}

	public void SetOriginRebirthData(object dataInfo)
	{
		if (dataInfo == null)
		{
			return;
		}
		string text = (string)dataInfo;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		RebirthData rebirthData = JsonUtility.FromJson<RebirthData>(text);
		if (rebirthData.m_transforms != null && rebirthData.m_transforms.Length == m_mapController.GetGrids.Count)
		{
			for (int i = 0; i < m_mapController.GetGrids.Count; i++)
			{
				m_mapController.GetGrids[i].gameObject.transform.SetTransData(rebirthData.m_transforms[i]);
			}
		}
	}

	public void StartRunByOriginRebirthData(object dataInfo)
	{
	}

	public byte[] GetOriginRebirthBsonData(object obj = null)
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.m_transforms = new RD_ElementTransform_DATA[m_mapController.GetGrids.Count];
		for (int i = 0; i < m_mapController.GetGrids.Count; i++)
		{
			rebirthData.m_transforms[i] = m_mapController.GetGrids[i].gameObject.transform.GetTransData();
		}
		return Bson.ToBson(rebirthData);
	}

	public void SetOriginRebirthBsonData(byte[] dataInfo)
	{
		if (dataInfo == null || dataInfo.Length == 0)
		{
			return;
		}
		RebirthData rebirthData = Bson.ToObject<RebirthData>(dataInfo);
		if (rebirthData.m_transforms != null && rebirthData.m_transforms.Length == m_mapController.GetGrids.Count)
		{
			for (int i = 0; i < m_mapController.GetGrids.Count; i++)
			{
				m_mapController.GetGrids[i].gameObject.transform.SetTransData(rebirthData.m_transforms[i]);
			}
		}
	}

	public void StartRunByOriginRebirthBsonData(byte[] dataInfo)
	{
	}
}
