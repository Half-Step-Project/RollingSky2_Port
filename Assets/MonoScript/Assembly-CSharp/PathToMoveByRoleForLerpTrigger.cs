using System;
using System.IO;
using Foundation;
using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class PathToMoveByRoleForLerpTrigger : BaseTriggerBox, IPathLerpRebirth
{
	[Serializable]
	public struct PathToMoveByRoleForLerpTriggerData : IReadWriteBytes
	{
		public PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData m_pathData;

		[Header("大等于0")]
		public int m_lerpIndex;

		public PathToMoveByRoleForLerpTriggerData Copy()
		{
			PathToMoveByRoleForLerpTriggerData result = default(PathToMoveByRoleForLerpTriggerData);
			result.m_pathData = m_pathData.Copy();
			result.m_lerpIndex = m_lerpIndex;
			return result;
		}

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_lerpIndex = bytes.GetInt32(ref startIndex);
			m_pathData = default(PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData);
			byte[] array = new byte[bytes.Length - startIndex];
			Array.Copy(bytes, startIndex, array, 0, array.Length);
			m_pathData.ReadBytes(array);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_lerpIndex.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_pathData.WriteBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public PathToMoveByRoleForLerpTriggerData m_data;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void InitElement()
	{
		base.InitElement();
	}

	public override void ResetElement()
	{
		base.ResetElement();
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (PathToMoveByRoleForLerpTriggerData)objs[0];
	}

	public override string Write()
	{
		if (m_data.m_pathData.m_smooth <= 0)
		{
			Debug.LogError("PathToMoveByRoleForLerpTrigger.m_pathData.smooth <=0");
		}
		Grid componentInParent = base.transform.GetComponentInParent<Grid>();
		if (componentInParent != null)
		{
			Vector3[] array = new Vector3[m_data.m_pathData.m_positions.Length];
			for (int i = 0; i < m_data.m_pathData.m_positions.Length; i++)
			{
				Vector3 position = base.transform.TransformPoint(m_data.m_pathData.m_positions[i]);
				array[i] = componentInParent.transform.InverseTransformPoint(position);
			}
			Vector3[] pathByPositions = ThreeBezier.GetPathByPositions(array, m_data.m_pathData.m_smooth);
			if (pathByPositions.Length < 500)
			{
				m_data.m_pathData.m_bezierPositions = pathByPositions;
			}
		}
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<PathToMoveByRoleForLerpTriggerData>(info);
	}

	public override byte[] WriteBytes()
	{
		if (m_data.m_pathData.m_smooth <= 0)
		{
			Debug.LogError("PathToMoveByRoleForLerpTrigger.m_pathData.smooth <=0");
		}
		Grid componentInParent = base.transform.GetComponentInParent<Grid>();
		if (componentInParent != null)
		{
			Vector3[] array = new Vector3[m_data.m_pathData.m_positions.Length];
			for (int i = 0; i < m_data.m_pathData.m_positions.Length; i++)
			{
				Vector3 position = base.transform.TransformPoint(m_data.m_pathData.m_positions[i]);
				array[i] = componentInParent.transform.InverseTransformPoint(position);
			}
			Vector3[] pathByPositions = ThreeBezier.GetPathByPositions(array, m_data.m_pathData.m_smooth);
			if (pathByPositions.Length < 500)
			{
				m_data.m_pathData.m_bezierPositions = pathByPositions;
			}
		}
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<PathToMoveByRoleForLerpTriggerData>(bytes);
	}

	public override void TriggerEnter(BaseRole ball)
	{
		PathToMoveByRoleForLerpTriggerData pathLerpData = GetPathLerpData();
		ball.CallChangeToPathToMoveLerp(pathLerpData, m_uuId);
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		ThreeBezier.DrawGizmos(base.gameObject, m_data.m_pathData.m_positions, m_data.m_pathData.m_smooth);
	}

	public PathToMoveByRoleForLerpTriggerData GetPathLerpData()
	{
		PathToMoveByRoleForLerpTriggerData result = m_data.Copy();
		if (m_data.m_pathData.m_bezierPositions == null || m_data.m_pathData.m_bezierPositions.Length == 0)
		{
			for (int i = 0; i < result.m_pathData.m_positions.Length; i++)
			{
				result.m_pathData.m_positions[i] = base.transform.TransformPoint(result.m_pathData.m_positions[i]);
			}
		}
		return result;
	}
}
