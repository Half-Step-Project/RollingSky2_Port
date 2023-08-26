using System;
using System.Collections.Generic;
using Foundation;
using RS2;
using UnityEngine;
using User.TileMap;

using Grid = User.TileMap.Grid;

public class LeftRightRotateListenTrigger : BaseTriggerBox
{
	[Serializable]
	public struct Data
	{
		[HideInInspector]
		public int[] m_gridIDs;

		[Header("向左旋转 Left")]
		public float m_leftStartDistance;

		public float m_leftEndDistance;

		public float m_leftAngle;

		public EaseType m_leftEaseType;

		public int m_leftGroupID;

		[Header("向右旋转 Right")]
		public float m_rightStartDistance;

		public float m_rightEndDistance;

		public float m_rightAngle;

		public EaseType m_rightEaseType;

		public int m_rightGroupID;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.m_gridIDs = new int[0];
				result.m_leftStartDistance = -10f;
				result.m_leftEndDistance = 0f;
				result.m_leftAngle = 90f;
				result.m_leftEaseType = EaseType.Linear;
				result.m_leftGroupID = 1;
				result.m_rightStartDistance = -10f;
				result.m_rightEndDistance = 0f;
				result.m_rightAngle = -90f;
				result.m_rightEaseType = EaseType.Linear;
				result.m_rightGroupID = 1;
				return result;
			}
		}
	}

	[Serializable]
	public struct RebirthData
	{
		public RD_ElementTransform_DATA tran;

		public RD_ElementTransform_DATA m_rotateObjTransform;

		public LeftRightState m_state;

		public Vector3[] m_gridPointPos;

		public Quaternion[] m_gridQuaternions;
	}

	public Data m_data = Data.DefaultValue;

	public Grid[] m_targetGrids = new Grid[0];

	private Vector3[] m_gridPointPos = new Vector3[0];

	private Quaternion[] m_gridQuaternions = new Quaternion[0];

	private LeftRightState m_currentState;

	private Transform m_rotateObjeTarget;

	private DistancSensore m_playSensore;

	[Range(-1f, 1f)]
	public float m_currentProgress;

	[HideInInspector]
	public float m_currentDistance;

	private Transform RotateObjectTarget
	{
		get
		{
			if (m_rotateObjeTarget == null)
			{
				m_rotateObjeTarget = base.gameObject.transform.Find("model");
			}
			return m_rotateObjeTarget;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override bool CanRecycle
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		m_currentDistance = 0f;
		m_currentProgress = 0f;
		OnInit();
		RotateObjectTarget.localRotation = Quaternion.identity;
		GetGridData();
		Mod.Event.Subscribe(EventArgs<LeftRightRotateEventArg>.EventId, OnListenEvent);
	}

	public override void UpdateElement()
	{
		if (Application.isPlaying)
		{
			switch (m_currentState)
			{
			case LeftRightState.Left:
				if (m_playSensore != null)
				{
					m_currentDistance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
					if (m_playSensore.Run(m_currentDistance))
					{
						float num3 = m_data.m_rightEndDistance - m_data.m_rightStartDistance;
						float num4 = (m_currentDistance - m_data.m_rightStartDistance) / num3;
						m_currentProgress = ((num4 >= 1f) ? 1f : num4);
						OnLeft(m_currentProgress);
					}
					else if (m_playSensore.IsBeyondFarthestDistance(m_currentDistance) && m_currentProgress != 1f)
					{
						m_currentProgress = 1f;
						OnLeft(m_currentProgress);
					}
				}
				break;
			case LeftRightState.Right:
				if (m_playSensore != null)
				{
					m_currentDistance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
					if (m_playSensore.Run(m_currentDistance))
					{
						float num = m_data.m_rightEndDistance - m_data.m_rightStartDistance;
						float num2 = (m_currentDistance - m_data.m_rightStartDistance) / num;
						m_currentProgress = ((num2 >= 1f) ? 1f : num2);
						OnRight(m_currentProgress);
					}
					else if (m_playSensore.IsBeyondFarthestDistance(m_currentDistance) && m_currentProgress != 1f)
					{
						m_currentProgress = 1f;
						OnRight(m_currentProgress);
					}
				}
				break;
			case LeftRightState.Null:
				break;
			}
		}
		else if (m_currentProgress <= 0f)
		{
			RotateObjectTarget.localRotation = Easing.EasingQuaternion(0f - m_currentProgress, Vector3.zero, new Vector3(0f, 0f, m_data.m_leftAngle), 1f, m_data.m_leftEaseType);
		}
		else
		{
			RotateObjectTarget.localRotation = Easing.EasingQuaternion(m_currentProgress, Vector3.zero, new Vector3(0f, 0f, m_data.m_rightAngle), 1f, m_data.m_rightEaseType);
		}
	}

	public override void ResetElement()
	{
		RotateObjectTarget.localRotation = Quaternion.identity;
		Mod.Event.Unsubscribe(EventArgs<LeftRightRotateEventArg>.EventId, OnListenEvent);
		base.ResetElement();
	}

	private void OnLeft(float progress)
	{
		RotateObjectTarget.localRotation = Easing.EasingQuaternion(progress, Vector3.zero, new Vector3(0f, 0f, m_data.m_leftAngle), 1f, m_data.m_leftEaseType);
		for (int i = 0; i < m_targetGrids.Length; i++)
		{
			m_targetGrids[i].transform.position = RotateObjectTarget.transform.TransformPoint(m_gridPointPos[i]);
			m_targetGrids[i].transform.rotation = RotateObjectTarget.transform.rotation * m_gridQuaternions[i];
		}
	}

	private void OnRight(float progress)
	{
		RotateObjectTarget.localRotation = Easing.EasingQuaternion(progress, Vector3.zero, new Vector3(0f, 0f, m_data.m_rightAngle), 1f, m_data.m_rightEaseType);
		for (int i = 0; i < m_targetGrids.Length; i++)
		{
			m_targetGrids[i].transform.position = RotateObjectTarget.transform.TransformPoint(m_gridPointPos[i]);
			m_targetGrids[i].transform.rotation = RotateObjectTarget.transform.rotation * m_gridQuaternions[i];
		}
	}

	public override string Write()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < m_targetGrids.Length; i++)
		{
			if (m_targetGrids[i] != null)
			{
				list.Add(m_targetGrids[i].m_id);
			}
		}
		m_data.m_gridIDs = list.ToArray();
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<Data>(info);
		if (Application.isPlaying)
		{
			return;
		}
		Grid[] array = UnityEngine.Object.FindObjectsOfType<Grid>();
		List<Grid> list = new List<Grid>();
		for (int i = 0; i < m_data.m_gridIDs.Length; i++)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].m_id == m_data.m_gridIDs[i])
				{
					list.Add(array[j]);
				}
			}
		}
		m_targetGrids = list.ToArray();
	}

	public override byte[] WriteBytes()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < m_targetGrids.Length; i++)
		{
			if (m_targetGrids[i] != null)
			{
				list.Add(m_targetGrids[i].m_id);
			}
		}
		m_data.m_gridIDs = list.ToArray();
		return Bson.ToBson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = Bson.ToObject<Data>(bytes);
		if (Application.isPlaying)
		{
			return;
		}
		Grid[] array = UnityEngine.Object.FindObjectsOfType<Grid>();
		List<Grid> list = new List<Grid>();
		for (int i = 0; i < m_data.m_gridIDs.Length; i++)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].m_id == m_data.m_gridIDs[i])
				{
					list.Add(array[j]);
				}
			}
		}
		m_targetGrids = list.ToArray();
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (Data)objs[0];
	}

	private void OnValidate()
	{
		OnInit();
		UpdateElement();
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}

	private void OnListenEvent(object sender, Foundation.EventArgs e)
	{
		LeftRightRotateEventArg leftRightRotateEventArg = e as LeftRightRotateEventArg;
		if (leftRightRotateEventArg == null)
		{
			return;
		}
		for (int i = 0; i < leftRightRotateEventArg.m_data.m_datas.Length; i++)
		{
			LeftRightRotateSendData leftRightRotateSendData = leftRightRotateEventArg.m_data.m_datas[i];
			if (leftRightRotateSendData.m_state != 0)
			{
				if (leftRightRotateSendData.m_state == LeftRightState.Left && leftRightRotateSendData.m_groupID == m_data.m_leftGroupID)
				{
					SwitchState(LeftRightState.Left);
					break;
				}
				if (leftRightRotateSendData.m_state == LeftRightState.Right && leftRightRotateSendData.m_groupID == m_data.m_rightGroupID)
				{
					SwitchState(LeftRightState.Right);
					break;
				}
			}
		}
	}

	private void SwitchState(LeftRightState state)
	{
		switch (state)
		{
		case LeftRightState.Left:
			m_playSensore = new DistancSensore(m_data.m_leftStartDistance, m_data.m_leftEndDistance, DistancSensore.DistancSensoreType.Continued);
			break;
		case LeftRightState.Right:
			m_playSensore = new DistancSensore(m_data.m_rightStartDistance, m_data.m_rightEndDistance, DistancSensore.DistancSensoreType.Continued);
			break;
		}
		m_currentProgress = 0f;
		RotateObjectTarget.localEulerAngles = new Vector3(0f, 0f, 0f);
		m_currentState = state;
	}

	private void GetGridData()
	{
		m_targetGrids = new Grid[m_data.m_gridIDs.Length];
		m_gridPointPos = new Vector3[m_data.m_gridIDs.Length];
		m_gridQuaternions = new Quaternion[m_data.m_gridIDs.Length];
		for (int i = 0; i < m_targetGrids.Length; i++)
		{
			m_targetGrids[i] = MapController.Instance.GetGridById(m_data.m_gridIDs[i]);
			m_gridPointPos[i] = RotateObjectTarget.InverseTransformPoint(m_targetGrids[i].transform.position);
			m_gridQuaternions[i] = m_targetGrids[i].transform.rotation;
		}
	}

	private void OnInit()
	{
	}

	public override object RebirthWriteData()
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.tran = base.gameObject.transform.GetTransData();
		rebirthData.m_rotateObjTransform = RotateObjectTarget.transform.GetTransData();
		rebirthData.m_state = m_currentState;
		rebirthData.m_gridPointPos = m_gridPointPos;
		rebirthData.m_gridQuaternions = m_gridQuaternions;
		return JsonUtility.ToJson(rebirthData);
	}

	public override void RebirthReadData(object rd_data)
	{
		if (rd_data != null)
		{
			string text = (string)rd_data;
			if (!string.IsNullOrEmpty(text))
			{
				RebirthData rebirthData = JsonUtility.FromJson<RebirthData>(text);
				base.gameObject.transform.SetTransData(rebirthData.tran);
				SwitchState(m_currentState);
				RotateObjectTarget.transform.SetTransData(rebirthData.m_rotateObjTransform);
				m_currentState = rebirthData.m_state;
				m_gridPointPos = rebirthData.m_gridPointPos;
				m_gridQuaternions = rebirthData.m_gridQuaternions;
			}
		}
	}

	public override byte[] RebirthWriteByteData()
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.tran = base.gameObject.transform.GetTransData();
		rebirthData.m_rotateObjTransform = RotateObjectTarget.transform.GetTransData();
		rebirthData.m_state = m_currentState;
		rebirthData.m_gridPointPos = m_gridPointPos;
		rebirthData.m_gridQuaternions = m_gridQuaternions;
		return Bson.ToBson(rebirthData);
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		if (rd_data != null && rd_data.Length != 0)
		{
			RebirthData rebirthData = Bson.ToObject<RebirthData>(rd_data);
			base.gameObject.transform.SetTransData(rebirthData.tran);
			SwitchState(m_currentState);
			RotateObjectTarget.transform.SetTransData(rebirthData.m_rotateObjTransform);
			m_currentState = rebirthData.m_state;
			m_gridPointPos = rebirthData.m_gridPointPos;
			m_gridQuaternions = rebirthData.m_gridQuaternions;
		}
	}
}
