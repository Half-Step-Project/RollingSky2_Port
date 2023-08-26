using System;
using Foundation;
using RS2;
using UnityEngine;

public class PathToMoveByDiamondAwardListenTrigger : BaseTriggerBox, IAwardComplete, IAward
{
	[Serializable]
	public struct Data
	{
		public PathToMoveData m_pathData;

		public float m_speed;

		public int m_groupID;

		[Label]
		public int sortID;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.m_pathData = PathToMoveData.DefaultValue;
				result.m_speed = 2.5f;
				result.m_groupID = 1;
				result.sortID = -1;
				return result;
			}
		}
	}

	[Serializable]
	public class RebirthData
	{
		public RD_ElementTransform_DATA m_trans;

		public RD_ElementTransform_DATA m_moveObject;

		public RD_ElementAnim_DATA m_awardShowAnimation;

		public RD_ElementAnim_DATA m_awardHideAnimation;

		public RD_ElementParticle_DATA m_particle;

		public RD_BezierMove_Data m_bezierMover;

		public bool m_isPlaying;

		public bool m_isFinished;
	}

	public Animation m_showAnimation;

	public Animation m_hideAnimation;

	public ParticleSystem m_particle;

	public GameObject m_moveObj;

	public Data m_data = Data.DefaultValue;

	private BezierMover m_bezierMover;

	[Label]
	public bool m_isPlaying;

	[Label]
	public bool m_isFinished;

	private float m_deltaLocZ;

	private Vector3 m_beginPos = Vector3.zero;

	private Vector3 m_targetPos = Vector3.zero;

	private Vector3 m_moveLocDir = Vector3.forward;

	private RebirthData m_rebirthData;

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
		base.Initialize();
		if (m_particle == null)
		{
			Transform transform = base.gameObject.transform.Find("effect");
			if (transform != null)
			{
				m_particle = transform.gameObject.GetComponentInChildren<ParticleSystem>();
			}
		}
		if (m_moveObj == null)
		{
			Transform transform2 = base.gameObject.transform.Find("model");
			if (transform2 != null)
			{
				m_moveObj = transform2.gameObject;
			}
		}
		if (m_showAnimation == null)
		{
			Transform transform3 = base.gameObject.transform.Find("model/showNode");
			if (transform3 != null)
			{
				m_showAnimation = transform3.gameObject.GetComponentInChildren<Animation>();
			}
		}
		if (m_hideAnimation == null)
		{
			Transform transform4 = base.gameObject.transform.Find("model/hideNode");
			if (transform4 != null)
			{
				m_hideAnimation = transform4.gameObject.GetComponentInChildren<Animation>();
			}
		}
		m_data.m_pathData.RefreshBezierPositions(base.gameObject);
		m_bezierMover = new BezierMover(m_data.m_pathData.m_bezierPositions);
		if (m_moveObj != null)
		{
			m_moveObj.gameObject.SetActive(true);
			m_moveObj.transform.position = m_data.m_pathData.m_bezierPositions[0];
		}
		m_isPlaying = false;
		m_isFinished = false;
		if (m_showAnimation != null)
		{
			m_showAnimation.gameObject.SetActive(false);
		}
		if (m_hideAnimation != null)
		{
			m_hideAnimation.gameObject.SetActive(false);
		}
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		DropType dropType = GetDropType();
		if (dataModule.IsShowForAwardByDropType(dropType, m_data.sortID))
		{
			if (m_showAnimation != null)
			{
				m_showAnimation.gameObject.SetActive(true);
				m_showAnimation.Play();
			}
		}
		else if (m_hideAnimation != null)
		{
			m_hideAnimation.gameObject.SetActive(true);
			m_hideAnimation.Play();
		}
		SubscribeEvent();
	}

	public override void ResetElement()
	{
		base.ResetElement();
		if (m_showAnimation != null)
		{
			m_showAnimation.Stop();
		}
		if (m_hideAnimation != null)
		{
			m_hideAnimation.Stop();
		}
		m_isPlaying = false;
		m_isFinished = false;
		UnSubscribeEvent();
	}

	public override void UpdateElement()
	{
		if (m_isPlaying && m_bezierMover != null && !m_isFinished)
		{
			m_deltaLocZ = Railway.theRailway.SpeedForward * Time.deltaTime * m_data.m_speed;
			m_beginPos = m_moveObj.transform.position;
			m_isFinished = m_bezierMover.MoveForwardByDis(m_deltaLocZ, m_beginPos, ref m_targetPos, ref m_moveLocDir);
			m_moveObj.transform.position = m_targetPos;
			bool isFinished = m_isFinished;
		}
		base.UpdateElement();
	}

	public override void TriggerEnter(BaseRole ball)
	{
		OnGainAward(ball);
		if (m_moveObj != null)
		{
			m_moveObj.gameObject.SetActive(false);
			if (m_particle != null)
			{
				m_particle.transform.position = m_moveObj.transform.position;
				m_particle.Play(true);
			}
			if (audioSource != null)
			{
				audioSource.transform.position = m_moveObj.transform.position;
				PlaySoundEffect();
			}
		}
	}

	protected virtual void OnGainAward(BaseRole ball)
	{
		ball.GainDiamond(m_uuId, m_data.sortID);
	}

	protected virtual void SubscribeEvent()
	{
		Mod.Event.Subscribe(EventArgs<PathToMoveByDiamondAwardEventArgs>.EventId, OnListenEvent);
	}

	protected virtual void UnSubscribeEvent()
	{
		Mod.Event.Unsubscribe(EventArgs<PathToMoveByDiamondAwardEventArgs>.EventId, OnListenEvent);
	}

	protected void OnListenEvent(object sender, Foundation.EventArgs e)
	{
		PathToMoveByDiamondAwardEventArgs pathToMoveByDiamondAwardEventArgs = e as PathToMoveByDiamondAwardEventArgs;
		if (pathToMoveByDiamondAwardEventArgs != null && pathToMoveByDiamondAwardEventArgs.m_groupID == m_data.m_groupID)
		{
			m_isPlaying = true;
		}
	}

	private void OnValidate()
	{
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (m_data.m_pathData.m_positions != null && m_data.m_pathData.m_positions.Length >= 4 && (m_data.m_pathData.m_positions.Length - 1) % 3 == 0)
		{
			ThreeBezier.DrawGizmos(base.gameObject, m_data.m_pathData.m_positions, m_data.m_pathData.m_smooth);
		}
	}

	public override string Write()
	{
		m_data.m_pathData.RefreshBezierPositions(base.gameObject, true);
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<Data>(info);
	}

	public override byte[] WriteBytes()
	{
		m_data.m_pathData.RefreshBezierPositions(base.gameObject, true);
		return Bson.ToBson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = Bson.ToObject<Data>(bytes);
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
		m_rebirthData = JsonUtility.FromJson<RebirthData>(rd_data as string);
		base.transform.SetTransData(m_rebirthData.m_trans);
		if ((bool)m_moveObj)
		{
			m_moveObj.transform.SetTransData(m_rebirthData.m_moveObject);
		}
		m_bezierMover.SetBezierData(m_rebirthData.m_bezierMover);
		m_isPlaying = m_rebirthData.m_isPlaying;
		m_isFinished = m_rebirthData.m_isFinished;
		m_showAnimation.SetAnimData(m_rebirthData.m_awardShowAnimation, ProcessState.Pause);
		m_hideAnimation.SetAnimData(m_rebirthData.m_awardHideAnimation, ProcessState.Pause);
		m_particle.SetParticleData(m_rebirthData.m_particle, ProcessState.Pause);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RebirthData rebirthData = new RebirthData();
		rebirthData.m_trans = base.transform.GetTransData();
		if ((bool)m_moveObj)
		{
			rebirthData.m_moveObject = m_moveObj.transform.GetTransData();
		}
		rebirthData.m_bezierMover = m_bezierMover.GetBezierData();
		rebirthData.m_isPlaying = m_isPlaying;
		rebirthData.m_awardShowAnimation = m_showAnimation.GetAnimData();
		rebirthData.m_awardHideAnimation = m_hideAnimation.GetAnimData();
		rebirthData.m_particle = m_particle.GetParticleData();
		return JsonUtility.ToJson(rebirthData);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		if (m_rebirthData != null)
		{
			m_showAnimation.SetAnimData(m_rebirthData.m_awardShowAnimation, ProcessState.UnPause);
			m_hideAnimation.SetAnimData(m_rebirthData.m_awardHideAnimation, ProcessState.UnPause);
			m_particle.SetParticleData(m_rebirthData.m_particle, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override void RebirthReadDataForDrop(object rd_data)
	{
		m_moveObj.SetActive(false);
		m_particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override void RebirthStartGameForDrop(object rd_data)
	{
		m_moveObj.SetActive(false);
		m_particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RebirthData>(rd_data);
		base.transform.SetTransData(m_rebirthData.m_trans);
		if ((bool)m_moveObj)
		{
			m_moveObj.transform.SetTransData(m_rebirthData.m_moveObject);
		}
		m_bezierMover.SetBezierData(m_rebirthData.m_bezierMover);
		m_isPlaying = m_rebirthData.m_isPlaying;
		m_isFinished = m_rebirthData.m_isFinished;
		m_showAnimation.SetAnimData(m_rebirthData.m_awardShowAnimation, ProcessState.Pause);
		m_hideAnimation.SetAnimData(m_rebirthData.m_awardHideAnimation, ProcessState.Pause);
		m_particle.SetParticleData(m_rebirthData.m_particle, ProcessState.Pause);
	}

	public override byte[] RebirthWriteByteData()
	{
		RebirthData rebirthData = new RebirthData();
		rebirthData.m_trans = base.transform.GetTransData();
		if ((bool)m_moveObj)
		{
			rebirthData.m_moveObject = m_moveObj.transform.GetTransData();
		}
		rebirthData.m_bezierMover = m_bezierMover.GetBezierData();
		rebirthData.m_isPlaying = m_isPlaying;
		rebirthData.m_awardShowAnimation = m_showAnimation.GetAnimData();
		rebirthData.m_awardHideAnimation = m_hideAnimation.GetAnimData();
		rebirthData.m_particle = m_particle.GetParticleData();
		return Bson.ToBson(rebirthData);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (m_rebirthData != null)
		{
			m_showAnimation.SetAnimData(m_rebirthData.m_awardShowAnimation, ProcessState.UnPause);
			m_hideAnimation.SetAnimData(m_rebirthData.m_awardHideAnimation, ProcessState.UnPause);
			m_particle.SetParticleData(m_rebirthData.m_particle, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	public override void RebirthReadByteDataForDrop(byte[] rd_data)
	{
		m_moveObj.SetActive(false);
		m_particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	public override void RebirthStartGameByteDataForDrop(byte[] rd_data)
	{
		m_moveObj.SetActive(false);
		m_particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}
}
