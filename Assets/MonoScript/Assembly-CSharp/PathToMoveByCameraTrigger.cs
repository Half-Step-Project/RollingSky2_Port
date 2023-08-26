using System;
using Foundation;
using RS2;
using UnityEngine;

public class PathToMoveByCameraTrigger : BaseTriggerBox
{
	[Serializable]
	public struct ElementData
	{
		public PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData m_pathData;

		public float m_speed;

		public bool m_isLocalSpace;

		public string m_animationName;
	}

	public static readonly string BASE_PATH = "Assets/_RS2Art/Res/Brush/Related/Animations/";

	[HideInInspector]
	public AnimationClip m_animationClip;

	public ElementData m_data;

	public override void Initialize()
	{
		base.Initialize();
		if (m_animationClip == null)
		{
			string path = string.Format("{0}{1}.anim", BASE_PATH, m_data.m_animationName);
			m_animationClip = MapController.Instance.GetRelatedAnimationClipByPath(path);
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
		if (m_animationClip != null)
		{
			CameraController.theCamera.TriggerPlayAnimClip(m_animationClip, m_animationClip.name);
		}
		else
		{
			Log.Error("wyj=CameraAnimTrigger has no anim!!!==" + base.name);
		}
		Vector3[] array = null;
		if (m_data.m_pathData.m_bezierPositions != null && m_data.m_pathData.m_bezierPositions.Length != 0)
		{
			array = m_data.m_pathData.m_bezierPositions;
		}
		else if (m_data.m_isLocalSpace)
		{
			array = ThreeBezier.GetPathByPositions(m_data.m_pathData.m_positions, m_data.m_pathData.m_smooth);
		}
		else
		{
			Vector3[] array2 = new Vector3[m_data.m_pathData.m_positions.Length];
			for (int i = 0; i < m_data.m_pathData.m_positions.Length; i++)
			{
				array2[i] = base.gameObject.transform.TransformPoint(m_data.m_pathData.m_positions[i]);
			}
			array = ThreeBezier.GetPathByPositions(array2, m_data.m_pathData.m_smooth);
		}
		m_data.m_pathData.m_bezierPositions = array;
		Mod.Event.FireNow(this, Mod.Reference.Acquire<PathToMoveByCameraEventArgs>().Initialize(m_data));
	}

	public override void ResetElement()
	{
		base.ResetElement();
		CameraController.theCamera.TriggerRemoveAnimClip(m_animationClip, m_animationClip.name);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<ElementData>(info);
	}

	public override string Write()
	{
		if (m_data.m_isLocalSpace)
		{
			Vector3[] pathByPositions = ThreeBezier.GetPathByPositions(m_data.m_pathData.m_positions, m_data.m_pathData.m_smooth);
			if (pathByPositions.Length < 500)
			{
				m_data.m_pathData.m_bezierPositions = pathByPositions;
			}
		}
		else
		{
			Vector3[] positions = new Vector3[m_data.m_pathData.m_positions.Length];
			for (int i = 0; i < m_data.m_pathData.m_positions.Length; i++)
			{
				base.transform.TransformPoint(m_data.m_pathData.m_positions[i]);
			}
			Vector3[] pathByPositions2 = ThreeBezier.GetPathByPositions(positions, m_data.m_pathData.m_smooth);
			if (pathByPositions2.Length < 500)
			{
				m_data.m_pathData.m_bezierPositions = pathByPositions2;
			}
		}
		return JsonUtility.ToJson(m_data);
	}

	public override byte[] WriteBytes()
	{
		if (m_data.m_isLocalSpace)
		{
			Vector3[] pathByPositions = ThreeBezier.GetPathByPositions(m_data.m_pathData.m_positions, m_data.m_pathData.m_smooth);
			if (pathByPositions.Length < 500)
			{
				m_data.m_pathData.m_bezierPositions = pathByPositions;
			}
		}
		else
		{
			Vector3[] positions = new Vector3[m_data.m_pathData.m_positions.Length];
			for (int i = 0; i < m_data.m_pathData.m_positions.Length; i++)
			{
				base.transform.TransformPoint(m_data.m_pathData.m_positions[i]);
			}
			Vector3[] pathByPositions2 = ThreeBezier.GetPathByPositions(positions, m_data.m_pathData.m_smooth);
			if (pathByPositions2.Length < 500)
			{
				m_data.m_pathData.m_bezierPositions = pathByPositions2;
			}
		}
		return Bson.ToBson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = Bson.ToObject<ElementData>(bytes);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (ElementData)objs[0];
	}
}
