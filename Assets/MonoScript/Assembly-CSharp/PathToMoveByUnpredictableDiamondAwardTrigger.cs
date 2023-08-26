using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

public class PathToMoveByUnpredictableDiamondAwardTrigger : BaseTriggerBox, IBrushTrigger, IRebirth, IAwardComplete, IAward
{
	[Serializable]
	public struct ElementData : IReadWriteBytes
	{
		public PathToMoveByRoleTrigger.PathToMoveByRoleTriggerData m_pathData;

		public PointData[] m_pointDatas;

		public float m_speed;

		public bool m_isLookAt;

		public bool m_animationPlayAutomatically;

		public WrapMode m_animationWrapMode;

		[Label]
		public int sortID;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_pathData.ReadBytes(bytes, ref startIndex);
			int @int = bytes.GetInt32(ref startIndex);
			m_pointDatas = new PointData[@int];
			for (int i = 0; i < @int; i++)
			{
				m_pointDatas[i].ReadBytes(bytes, ref startIndex);
			}
			m_speed = bytes.GetSingle(ref startIndex);
			m_isLookAt = bytes.GetBoolean(ref startIndex);
			m_animationPlayAutomatically = bytes.GetBoolean(ref startIndex);
			m_animationWrapMode = (WrapMode)bytes.GetInt32(ref startIndex);
			sortID = bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_pathData.WriteBytes(), ref offset);
				int value = m_pointDatas.Length;
				memoryStream.WriteByteArray(value.GetBytes(), ref offset);
				PointData[] pointDatas = m_pointDatas;
				foreach (PointData pointData in pointDatas)
				{
					memoryStream.WriteByteArray(pointData.WriteBytes(), ref offset);
				}
				memoryStream.WriteByteArray(m_speed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_isLookAt.GetBytes(), ref offset);
				memoryStream.WriteByteArray(m_animationPlayAutomatically.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)m_animationWrapMode).GetBytes(), ref offset);
				memoryStream.WriteByteArray(sortID.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	[Serializable]
	public struct PointData
	{
		public int m_index;

		public void ReadBytes(byte[] bytes, ref int startIndex)
		{
			m_index = bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return m_index.GetBytes();
		}
	}

	public class RebirthData
	{
		public int m_currentPointIndex;

		public bool m_isTriggerNext;

		public Vector3 m_position;

		public Quaternion m_rotation;

		public bool m_awardEnable;
	}

	public ElementData m_data;

	[HideInInspector]
	public int m_currentPointIndex;

	private GameObject m_modelObject;

	private Collider m_awardCollider;

	private Collider m_nextCollider;

	private Animation m_awardShowAnimation;

	private Animation m_awardHideAnimation;

	private GameObject m_effectObject;

	private ParticleSystem[] m_particles;

	private Vector3[] m_paths;

	private List<Vector3[]> m_positions = new List<Vector3[]>();

	private BezierMover m_bezierMover;

	private float m_deltaLocZ;

	private Vector3 m_beginPos = Vector3.zero;

	private Vector3 m_targetPos = Vector3.zero;

	private Vector3 m_moveLocDir = Vector3.forward;

	private bool m_isFinish;

	private bool m_isTriggerNext;

	private RD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA m_rebirthData;

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
			return true;
		}
	}

	public override void Initialize()
	{
		m_currentPointIndex = 0;
		if (m_modelObject == null)
		{
			Transform transform = base.gameObject.transform.Find("model");
			if (transform != null)
			{
				m_modelObject = transform.gameObject;
			}
		}
		if (m_awardShowAnimation == null && m_modelObject != null)
		{
			Transform transform2 = m_modelObject.transform.Find("showNode");
			if (transform2 != null)
			{
				m_awardShowAnimation = transform2.gameObject.GetComponentInChildren<Animation>();
			}
		}
		if (m_awardHideAnimation == null && m_modelObject != null)
		{
			Transform transform3 = m_modelObject.transform.Find("hideNode");
			if (transform3 != null)
			{
				m_awardHideAnimation = transform3.gameObject.GetComponentInChildren<Animation>();
			}
		}
		if (m_nextCollider == null)
		{
			m_nextCollider = base.gameObject.transform.Find("model/nextCollider").gameObject.GetComponent<Collider>();
		}
		if (m_awardCollider == null)
		{
			m_awardCollider = base.gameObject.transform.Find("collider").gameObject.GetComponent<Collider>();
		}
		if (m_effectObject == null)
		{
			m_effectObject = base.gameObject.transform.Find("effect").gameObject;
			if (m_effectObject != null)
			{
				m_particles = m_effectObject.GetComponentsInChildren<ParticleSystem>();
			}
		}
		if (m_data.m_pathData.m_bezierPositions != null && m_data.m_pathData.m_bezierPositions.Length != 0)
		{
			m_paths = m_data.m_pathData.m_bezierPositions;
		}
		else
		{
			Vector3[] array = new Vector3[m_data.m_pathData.m_positions.Length];
			for (int i = 0; i < m_data.m_pathData.m_positions.Length; i++)
			{
				array[i] = base.gameObject.transform.TransformPoint(m_data.m_pathData.m_positions[i]);
			}
			m_paths = ThreeBezier.GetPathByPositions(array, m_data.m_pathData.m_smooth);
		}
		m_bezierMover = new BezierMover();
		m_isTriggerNext = false;
		m_positions.Clear();
		if (m_data.m_pointDatas.Length > 1)
		{
			for (int j = 0; j < m_data.m_pointDatas.Length - 1; j++)
			{
				int num = j + 1;
				int index = m_data.m_pointDatas[j].m_index;
				int num2 = m_data.m_pointDatas[num].m_index - index + 1;
				Vector3[] array2 = new Vector3[num2];
				Array.Copy(m_paths, index, array2, 0, num2);
				m_positions.Add(array2);
			}
		}
		if (m_data.m_animationPlayAutomatically)
		{
			PlayAnimation();
		}
		StopParticle();
		if (m_awardShowAnimation != null)
		{
			m_awardShowAnimation.gameObject.SetActive(false);
		}
		if (m_awardHideAnimation != null)
		{
			m_awardHideAnimation.gameObject.SetActive(false);
		}
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		DropType dropType = GetDropType();
		if (dataModule.IsShowForAwardByDropType(dropType, m_data.sortID))
		{
			if (m_awardShowAnimation != null)
			{
				m_awardShowAnimation.gameObject.SetActive(true);
			}
		}
		else if (m_awardHideAnimation != null)
		{
			m_awardHideAnimation.gameObject.SetActive(true);
		}
		base.Initialize();
	}

	public override void LateInitialize()
	{
		base.LateInitialize();
		m_awardCollider.gameObject.SetActive(false);
		m_nextCollider.gameObject.SetActive(true);
		m_awardCollider.transform.position = m_paths[m_data.m_pointDatas[m_data.m_pointDatas.Length - 1].m_index];
		m_modelObject.transform.position = m_paths[m_data.m_pointDatas[0].m_index];
	}

	public override void UpdateElement()
	{
		base.UpdateElement();
		if (m_isTriggerNext && m_bezierMover != null && m_currentPointIndex < m_positions.Count)
		{
			m_deltaLocZ = Railway.theRailway.SpeedForward * Time.deltaTime * m_data.m_speed;
			m_beginPos = m_modelObject.transform.position;
			m_isFinish = m_bezierMover.MoveForwardByDis(m_deltaLocZ, m_beginPos, ref m_targetPos, ref m_moveLocDir);
			m_modelObject.transform.position = m_targetPos;
			m_nextCollider.transform.position = m_targetPos;
			if (m_data.m_isLookAt)
			{
				m_modelObject.transform.forward = m_moveLocDir;
			}
			if (m_isFinish)
			{
				m_currentPointIndex++;
				m_isTriggerNext = false;
			}
		}
	}

	public override void ResetElement()
	{
		m_modelObject.gameObject.SetActive(true);
		m_currentPointIndex = 0;
		m_awardCollider.transform.position = m_paths[m_data.m_pointDatas[m_data.m_pointDatas.Length - 1].m_index];
		m_awardCollider.gameObject.SetActive(false);
		m_modelObject.transform.position = m_paths[m_data.m_pointDatas[0].m_index];
		m_nextCollider.gameObject.SetActive(true);
		m_isTriggerNext = false;
		m_bezierMover = null;
		StopAnimation();
		StopParticle();
		base.ResetElement();
	}

	public override string Write()
	{
		if (m_data.m_pathData.m_smooth <= 0)
		{
			Debug.LogError("PathToMoveByUnpredictableDiamondAwardTrigger.m_pathData.smooth <=0");
		}
		Vector3[] array = new Vector3[m_data.m_pathData.m_positions.Length];
		for (int i = 0; i < m_data.m_pathData.m_positions.Length; i++)
		{
			array[i] = base.transform.TransformPoint(m_data.m_pathData.m_positions[i]);
		}
		Vector3[] pathByPositions = ThreeBezier.GetPathByPositions(array, m_data.m_pathData.m_smooth);
		if (pathByPositions.Length < 500)
		{
			m_data.m_pathData.m_bezierPositions = pathByPositions;
		}
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<ElementData>(info);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = StructTranslatorUtility.ToStructure<ElementData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		if (m_data.m_pathData.m_smooth <= 0)
		{
			Debug.LogError("PathToMoveByUnpredictableDiamondAwardTrigger.m_pathData.smooth <=0");
		}
		Vector3[] array = new Vector3[m_data.m_pathData.m_positions.Length];
		for (int i = 0; i < m_data.m_pathData.m_positions.Length; i++)
		{
			array[i] = base.transform.TransformPoint(m_data.m_pathData.m_positions[i]);
		}
		Vector3[] pathByPositions = ThreeBezier.GetPathByPositions(array, m_data.m_pathData.m_smooth);
		if (pathByPositions.Length < 500)
		{
			m_data.m_pathData.m_bezierPositions = pathByPositions;
		}
		return StructTranslatorUtility.ToByteArray(m_data);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (ElementData)objs[0];
	}

	public override void OnDrawGizmos()
	{
		ThreeBezier.DrawGizmos(base.gameObject, m_data.m_pathData.m_positions, m_data.m_pathData.m_smooth);
		base.OnDrawGizmos();
	}

	public void TriggerEnter(BaseRole ball, Collider collider)
	{
		if (!(ball != null))
		{
			return;
		}
		if (collider == m_nextCollider)
		{
			if (!m_isTriggerNext)
			{
				m_isTriggerNext = true;
				if (m_currentPointIndex < m_positions.Count && m_positions[m_currentPointIndex].Length != 0)
				{
					m_modelObject.transform.position = m_positions[m_currentPointIndex][0];
					m_bezierMover.ResetData(m_positions[m_currentPointIndex]);
				}
				else
				{
					m_awardCollider.gameObject.SetActive(true);
				}
			}
		}
		else if (collider == m_awardCollider)
		{
			m_isTriggerNext = false;
			OnGainAward(ball);
			m_modelObject.gameObject.SetActive(false);
			m_effectObject.transform.position = m_modelObject.transform.position;
			audioSource.transform.position = m_modelObject.transform.position;
			PlayParticle();
			PlaySoundEffect();
		}
	}

	protected virtual void OnGainAward(BaseRole ball)
	{
		ball.GainDiamond(m_uuId, m_data.sortID);
	}

	public bool IsRecordRebirth()
	{
		return true;
	}

	public object GetRebirthData(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		return new RebirthData
		{
			m_currentPointIndex = m_currentPointIndex,
			m_isTriggerNext = m_isTriggerNext,
			m_position = m_modelObject.transform.position,
			m_rotation = m_modelObject.transform.rotation,
			m_awardEnable = m_awardCollider.gameObject.activeSelf
		};
	}

	public void ResetBySavePointData(object obj)
	{
		RebirthData rebirthData = (RebirthData)obj;
		m_currentPointIndex = rebirthData.m_currentPointIndex;
		m_modelObject.transform.position = rebirthData.m_position;
		m_modelObject.transform.rotation = rebirthData.m_rotation;
		m_awardCollider.gameObject.SetActive(rebirthData.m_awardEnable);
		if (m_currentPointIndex < m_positions.Count && m_positions[m_currentPointIndex].Length != 0)
		{
			m_bezierMover.ResetData(m_positions[m_currentPointIndex]);
		}
	}

	public void StartRunningForRebirthData(object obj)
	{
	}

	private void PlayAnimation()
	{
		if (m_awardShowAnimation != null && m_awardShowAnimation.gameObject.activeSelf)
		{
			m_awardShowAnimation.wrapMode = m_data.m_animationWrapMode;
			m_awardShowAnimation.Play();
		}
		if (m_awardHideAnimation != null && m_awardHideAnimation.gameObject.activeSelf)
		{
			m_awardHideAnimation.wrapMode = m_data.m_animationWrapMode;
			m_awardHideAnimation.Play();
		}
	}

	private void StopAnimation()
	{
		if (m_awardShowAnimation != null && m_awardShowAnimation.gameObject.activeSelf)
		{
			m_awardShowAnimation.wrapMode = m_data.m_animationWrapMode;
			m_awardShowAnimation.Stop();
		}
		if (m_awardHideAnimation != null && m_awardHideAnimation.gameObject.activeSelf)
		{
			m_awardHideAnimation.wrapMode = m_data.m_animationWrapMode;
			m_awardHideAnimation.Stop();
		}
	}

	private void PlayParticle()
	{
		if (m_particles != null)
		{
			m_particles.PlayParticle();
		}
	}

	private void StopParticle()
	{
		if (m_particles != null)
		{
			m_particles.StopParticle();
		}
	}

	public int GetAwardSortID()
	{
		return m_data.sortID;
	}

	public void SetAwardSortID(int id)
	{
		m_data.sortID = id;
	}

	public virtual DropType GetDropType()
	{
		return DropType.DIAMOND;
	}

	public bool IsHaveFragment()
	{
		return false;
	}

	public int GetHaveFragmentCount()
	{
		return 0;
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA>(rd_data as string);
		m_modelObject.transform.SetTransData(m_rebirthData.m_modelObject);
		m_effectObject.transform.SetTransData(m_rebirthData.m_effectObject);
		m_currentPointIndex = m_rebirthData.m_currentPointIndex;
		if (m_awardCollider != null)
		{
			m_awardCollider.transform.SetTransData(m_rebirthData.m_awardColliderObject);
		}
		if (m_nextCollider != null)
		{
			m_nextCollider.transform.SetTransData(m_rebirthData.m_nextColliderObject);
		}
		m_awardShowAnimation.SetAnimData(m_rebirthData.m_awardShowAnimation, ProcessState.Pause);
		m_awardHideAnimation.SetAnimData(m_rebirthData.m_awardHideAnimation, ProcessState.Pause);
		m_particles.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		m_isTriggerNext = m_rebirthData.m_isTriggerNext == 1;
		if (m_currentPointIndex < m_positions.Count && m_positions[m_currentPointIndex].Length != 0)
		{
			m_bezierMover.ResetData(m_positions[m_currentPointIndex]);
		}
		m_bezierMover.SetBezierData(m_rebirthData.m_bezierMover);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA = new RD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_awardShowAnimation = m_awardShowAnimation.GetAnimData();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_awardHideAnimation = m_awardHideAnimation.GetAnimData();
		if (m_awardCollider != null)
		{
			rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_awardColliderObject = m_awardCollider.transform.GetTransData();
		}
		if (m_nextCollider != null)
		{
			rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_nextColliderObject = m_nextCollider.transform.GetTransData();
		}
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_particles = m_particles.GetParticlesData();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_modelObject = m_modelObject.transform.GetTransData();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_effectObject = m_effectObject.transform.GetTransData();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_currentPointIndex = m_currentPointIndex;
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_isTriggerNext = (m_isTriggerNext ? 1 : 0);
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_bezierMover = m_bezierMover.GetBezierData();
		return JsonUtility.ToJson(rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		if (m_rebirthData != null)
		{
			m_awardShowAnimation.SetAnimData(m_rebirthData.m_awardShowAnimation, ProcessState.UnPause);
			m_awardHideAnimation.SetAnimData(m_rebirthData.m_awardHideAnimation, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override void RebirthReadDataForDrop(object rd_data)
	{
		m_isTriggerNext = false;
		m_modelObject.gameObject.SetActive(false);
		m_particles.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override void RebirthStartGameForDrop(object rd_data)
	{
		m_isTriggerNext = false;
		m_modelObject.gameObject.SetActive(false);
		m_particles.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA>(rd_data);
		m_modelObject.transform.SetTransData(m_rebirthData.m_modelObject);
		m_effectObject.transform.SetTransData(m_rebirthData.m_effectObject);
		m_currentPointIndex = m_rebirthData.m_currentPointIndex;
		if (m_awardCollider != null)
		{
			m_awardCollider.transform.SetTransData(m_rebirthData.m_awardColliderObject);
		}
		if (m_nextCollider != null)
		{
			m_nextCollider.transform.SetTransData(m_rebirthData.m_nextColliderObject);
		}
		m_awardShowAnimation.SetAnimData(m_rebirthData.m_awardShowAnimation, ProcessState.Pause);
		m_awardHideAnimation.SetAnimData(m_rebirthData.m_awardHideAnimation, ProcessState.Pause);
		m_particles.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		m_isTriggerNext = m_rebirthData.m_isTriggerNext == 1;
		if (m_currentPointIndex < m_positions.Count && m_positions[m_currentPointIndex].Length != 0)
		{
			m_bezierMover.ResetData(m_positions[m_currentPointIndex]);
		}
		m_bezierMover.SetBezierData(m_rebirthData.m_bezierMover);
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA = new RD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_awardShowAnimation = m_awardShowAnimation.GetAnimData();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_awardHideAnimation = m_awardHideAnimation.GetAnimData();
		if (m_awardCollider != null)
		{
			rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_awardColliderObject = m_awardCollider.transform.GetTransData();
		}
		if (m_nextCollider != null)
		{
			rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_nextColliderObject = m_nextCollider.transform.GetTransData();
		}
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_particles = m_particles.GetParticlesData();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_modelObject = m_modelObject.transform.GetTransData();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_effectObject = m_effectObject.transform.GetTransData();
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_currentPointIndex = m_currentPointIndex;
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_isTriggerNext = (m_isTriggerNext ? 1 : 0);
		rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA.m_bezierMover = m_bezierMover.GetBezierData();
		return Bson.ToBson(rD_PathToMoveByUnpredictableDiamondAwardTrigger_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (m_rebirthData != null)
		{
			m_awardShowAnimation.SetAnimData(m_rebirthData.m_awardShowAnimation, ProcessState.UnPause);
			m_awardHideAnimation.SetAnimData(m_rebirthData.m_awardHideAnimation, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	public override void RebirthReadByteDataForDrop(byte[] rd_data)
	{
		m_isTriggerNext = false;
		m_modelObject.gameObject.SetActive(false);
		m_particles.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	public override void RebirthStartGameByteDataForDrop(byte[] rd_data)
	{
		m_isTriggerNext = false;
		m_modelObject.gameObject.SetActive(false);
		m_particles.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}
}
