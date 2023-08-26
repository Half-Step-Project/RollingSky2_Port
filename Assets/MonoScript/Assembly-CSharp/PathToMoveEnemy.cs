using System;
using System.IO;
using Foundation;
using UnityEngine;

public class PathToMoveEnemy : BaseEnemy
{
	[Serializable]
	public struct PathToMoveData : IReadWriteBytes
	{
		public float m_beginDistance;

		public float m_resetDistance;

		public Vector3[] m_positions;

		public Vector3[] m_bezierPositions;

		public int m_pathSmoothCount;

		public float m_speed;

		public bool m_isLookAtNext;

		public bool m_isEnableTime;

		public float m_duration;

		public bool m_isFinishedStop;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_beginDistance = bytes.GetSingle(ref startIndex);
			m_resetDistance = bytes.GetSingle(ref startIndex);
			m_positions = bytes.GetVector3Array(ref startIndex);
			m_bezierPositions = bytes.GetVector3Array(ref startIndex);
			m_pathSmoothCount = bytes.GetInt32(ref startIndex);
			m_speed = bytes.GetSingle(ref startIndex);
			m_isLookAtNext = bytes.GetBoolean(ref startIndex);
			m_isEnableTime = bytes.GetBoolean(ref startIndex);
			m_duration = bytes.GetSingle(ref startIndex);
			m_isFinishedStop = bytes.GetBoolean(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_beginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_resetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_positions.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_bezierPositions.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_pathSmoothCount.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_speed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_isLookAtNext.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_isEnableTime.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_duration.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_isFinishedStop.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public PathToMoveData m_data;

	private Vector3[] m_paths;

	private GameObject m_moveTarget;

	private Animation m_animation;

	private Vector3 m_atFirst;

	private Vector3 m_atLast;

	private float m_averageZTime;

	private float m_startZTime;

	private float m_currentZProportion;

	private Vector3 m_nextPoint = Vector3.zero;

	private Vector3 m_position = Vector3.zero;

	private float m_nextPositionZ;

	private float m_nextPointDistance;

	private float m_nextPositionDistatnce;

	private float m_nextPointProportion;

	private Vector3 m_moveTargetInverseTransformPoint;

	private Vector3 m_moveObjectStartPosition = Vector3.zero;

	private int m_currentPathPointIndex;

	public override bool CanRecycle
	{
		get
		{
			return false;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		commonState = CommonState.None;
		m_moveTarget = base.gameObject.transform.Find("model").gameObject;
		m_animation = m_moveTarget.GetComponentInChildren<Animation>();
		m_moveObjectStartPosition = m_moveTarget.transform.localPosition;
		if (m_animation != null)
		{
			m_animation.Play();
			m_animation.Sample();
			m_animation.Stop();
		}
	}

	public override void OnTriggerPlay()
	{
		base.OnTriggerPlay();
		if (m_data.m_bezierPositions != null && m_data.m_bezierPositions.Length != 0)
		{
			m_paths = m_data.m_bezierPositions;
		}
		else
		{
			Vector3[] array = new Vector3[m_data.m_positions.Length];
			for (int i = 0; i < m_data.m_positions.Length; i++)
			{
				array[i] = m_data.m_positions[i];
			}
			m_paths = Bezier.GetPathByPositions(array, (m_data.m_pathSmoothCount < 5) ? 20 : m_data.m_pathSmoothCount);
		}
		float averageZTime = m_data.m_duration;
		m_atFirst = m_paths[0];
		m_currentPathPointIndex = 0;
		m_atLast = m_paths[m_paths.Length - 1];
		if (!m_data.m_isEnableTime)
		{
			averageZTime = Mathf.Abs(m_atLast.z - m_atFirst.z) / m_data.m_speed;
		}
		m_averageZTime = averageZTime;
		m_startZTime = Time.time;
		if (m_animation != null && !m_animation.isPlaying)
		{
			m_animation["anim01"].normalizedTime = 0f;
			m_animation.Play();
		}
	}

	public override void OnTriggerStop()
	{
		base.OnTriggerStop();
		m_moveTarget.transform.localPosition = m_moveObjectStartPosition;
	}

	public override void UpdateElement()
	{
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (num >= m_data.m_beginDistance)
			{
				OnTriggerPlay();
				commonState = CommonState.Active;
			}
		}
		else if (commonState == CommonState.Active)
		{
			OnActive();
			if (num >= m_data.m_resetDistance)
			{
				OnTriggerStop();
				commonState = CommonState.End;
			}
		}
	}

	private void OnActive()
	{
		base.UpdateElement();
		m_currentZProportion = (Time.time - m_startZTime) / m_averageZTime;
		if (m_currentZProportion <= 1f)
		{
			m_nextPoint = Vector3.zero;
			m_position = Vector3.zero;
			m_nextPositionZ = Mathf.Lerp(m_atFirst.z, m_atLast.z, m_currentZProportion);
			m_moveTargetInverseTransformPoint = base.transform.InverseTransformPoint(m_moveTarget.transform.position);
			if (!GetPathNextPoint(m_nextPositionZ, m_paths, ref m_nextPoint))
			{
				return;
			}
			m_nextPointDistance = Mathf.Abs(m_nextPoint.z - m_moveTargetInverseTransformPoint.z);
			m_nextPositionDistatnce = Mathf.Abs(m_nextPositionZ - m_moveTargetInverseTransformPoint.z);
			m_nextPointProportion = m_nextPositionDistatnce / m_nextPointDistance;
			if (m_nextPointProportion > 0f)
			{
				m_position = Vector3.Lerp(m_moveTargetInverseTransformPoint, m_nextPoint, m_nextPointProportion);
				m_moveTarget.transform.position = base.transform.TransformPoint(m_position);
				if (m_data.m_isLookAtNext)
				{
					m_moveTarget.transform.LookAt(base.transform.TransformPoint(m_nextPoint));
				}
			}
		}
		else if (m_data.m_isFinishedStop && m_animation != null && m_animation.isPlaying)
		{
			m_animation.Stop();
		}
	}

	private bool GetPathNextPoint(float targetZ, Vector3[] bezierGridPoints, ref Vector3 nextP)
	{
		for (int i = m_currentPathPointIndex; i < bezierGridPoints.Length; i++)
		{
			if (targetZ <= bezierGridPoints[i].z)
			{
				m_currentPathPointIndex = i;
				nextP = bezierGridPoints[m_currentPathPointIndex];
				return true;
			}
		}
		m_currentPathPointIndex = bezierGridPoints.Length - 1;
		nextP = bezierGridPoints[m_currentPathPointIndex];
		return false;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		base.TriggerEnter(ball);
	}

	public override void ResetElement()
	{
		base.ResetElement();
		OnTriggerStop();
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (PathToMoveData)objs[0];
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<PathToMoveData>(info);
	}

	public override string Write()
	{
		if (m_data.m_pathSmoothCount <= 0)
		{
			Debug.LogError("PathToMoveEnemy.m_pathSmoothCount <=0");
		}
		Vector3[] array = new Vector3[m_data.m_positions.Length];
		for (int i = 0; i < m_data.m_positions.Length; i++)
		{
			array[i] = m_data.m_positions[i];
		}
		m_data.m_bezierPositions = Bezier.GetPathByPositions(array, (m_data.m_pathSmoothCount < 5) ? 20 : m_data.m_pathSmoothCount);
		return JsonUtility.ToJson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<PathToMoveData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		if (m_data.m_pathSmoothCount <= 0)
		{
			Debug.LogError("PathToMoveEnemy.m_pathSmoothCount <=0");
		}
		Vector3[] array = new Vector3[m_data.m_positions.Length];
		for (int i = 0; i < m_data.m_positions.Length; i++)
		{
			array[i] = m_data.m_positions[i];
		}
		m_data.m_bezierPositions = Bezier.GetPathByPositions(array, (m_data.m_pathSmoothCount < 5) ? 20 : m_data.m_pathSmoothCount);
		return StructTranslatorUtility.ToByteArray(m_data);
	}
}
