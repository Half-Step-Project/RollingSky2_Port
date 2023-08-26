using System.Collections.Generic;
using UnityEngine;
using UnityExpansion;

public class RecordOriginRebirthManager
{
	private static Dictionary<OriginRebirthForRowKey, List<OriginRebirthForRowValue>> m_dicData = new Dictionary<OriginRebirthForRowKey, List<OriginRebirthForRowValue>>();

	private static OriginRebirthData m_originRebirthData;

	public static bool m_isCanRecord = false;

	public static bool m_isBson = true;

	public static Dictionary<OriginRebirthForRowKey, List<OriginRebirthForRowValue>> RebirthDatas
	{
		get
		{
			return m_dicData;
		}
	}

	public static void RecordOriginRebirthForStart()
	{
		m_isCanRecord = true;
		m_dicData.Clear();
	}

	public static void RecordOriginRebirthForTrigger(object obj)
	{
		if (!m_isCanRecord || GameController.Instance == null)
		{
			return;
		}
		BaseElement baseElement = obj as BaseElement;
		if (m_dicData != null && baseElement != null)
		{
			OriginRebirthForRowKey key = new OriginRebirthForRowKey(baseElement.m_gridId, baseElement.point.m_y);
			List<OriginRebirthForRowValue> list = null;
			if (list == null)
			{
				list = new List<OriginRebirthForRowValue>();
			}
			SingleOriginRebirthLocationData singleOriginRebirthLocationData = new SingleOriginRebirthLocationData();
			singleOriginRebirthLocationData.m_point = baseElement.point;
			singleOriginRebirthLocationData.m_position = baseElement.transform.position.ToMyVector3();
			singleOriginRebirthLocationData.m_rotation = baseElement.transform.rotation.ToMyQuaternion();
			singleOriginRebirthLocationData.m_scale = baseElement.transform.localScale.ToMyVector3();
			OriginRebirthForRowValue originRebirthForRowValue = null;
			originRebirthForRowValue = ((!m_isBson) ? (GameController.Instance.GetOriginRebirthData(singleOriginRebirthLocationData) as OriginRebirthForRowValue) : GameController.Instance.GetOriginRebirthBsonData(singleOriginRebirthLocationData));
			list.Add(originRebirthForRowValue);
			m_dicData[key] = list;
		}
	}

	public static OriginRebirthData ExportOriginRebirthData()
	{
		OriginRebirthData originRebirthData = ScriptableObject.CreateInstance<OriginRebirthData>();
		originRebirthData.m_originRebirthDatas = new List<OriginRebirthForRowData>();
		if (m_dicData != null)
		{
			foreach (KeyValuePair<OriginRebirthForRowKey, List<OriginRebirthForRowValue>> dicDatum in m_dicData)
			{
				OriginRebirthForRowData originRebirthForRowData = new OriginRebirthForRowData();
				originRebirthForRowData.m_rowKey = dicDatum.Key;
				originRebirthForRowData.m_rowValues = dicDatum.Value;
				originRebirthData.m_originRebirthDatas.Add(originRebirthForRowData);
			}
			return originRebirthData;
		}
		return originRebirthData;
	}

	public static OriginRebirthData Merge(OriginRebirthData data1, OriginRebirthData data2)
	{
		OriginRebirthData originRebirthData = ScriptableObject.CreateInstance<OriginRebirthData>();
		Dictionary<OriginRebirthForRowKey, List<OriginRebirthForRowValue>> dictionary = new Dictionary<OriginRebirthForRowKey, List<OriginRebirthForRowValue>>();
		List<OriginRebirthForRowData> originRebirthDatas = data1.m_originRebirthDatas;
		for (int i = 0; i < originRebirthDatas.Count; i++)
		{
			dictionary.Add(originRebirthDatas[i].m_rowKey, originRebirthDatas[i].m_rowValues);
		}
		List<OriginRebirthForRowData> originRebirthDatas2 = data2.m_originRebirthDatas;
		for (int j = 0; j < originRebirthDatas2.Count; j++)
		{
			List<OriginRebirthForRowValue> value = null;
			List<OriginRebirthForRowValue> rowValues = originRebirthDatas2[j].m_rowValues;
			if (dictionary.TryGetValue(originRebirthDatas2[j].m_rowKey, out value))
			{
				for (int k = 0; k < rowValues.Count; k++)
				{
					bool flag = false;
					for (int l = 0; l < value.Count; l++)
					{
						if (rowValues[k].m_locationData.m_point == value[l].m_locationData.m_point)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						value.Add(rowValues[k]);
					}
				}
				dictionary[originRebirthDatas2[j].m_rowKey] = value;
			}
			else
			{
				dictionary[originRebirthDatas2[j].m_rowKey] = originRebirthDatas2[j].m_rowValues;
			}
		}
		originRebirthData.m_originRebirthDatas = new List<OriginRebirthForRowData>();
		foreach (KeyValuePair<OriginRebirthForRowKey, List<OriginRebirthForRowValue>> item in dictionary)
		{
			OriginRebirthForRowData originRebirthForRowData = new OriginRebirthForRowData();
			originRebirthForRowData.m_rowKey = item.Key;
			originRebirthForRowData.m_rowValues = item.Value;
			originRebirthData.m_originRebirthDatas.Add(originRebirthForRowData);
		}
		return originRebirthData;
	}
}
