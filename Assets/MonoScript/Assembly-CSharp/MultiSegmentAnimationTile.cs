using System;
using Foundation;
using UnityEngine;

public class MultiSegmentAnimationTile : BaseTile
{
	[Serializable]
	public struct Data
	{
		[Header("放入地图触发动画")]
		public bool m_isPlayInitAnim;

		public string m_initAnimName;

		[Header("距离感应播放动画")]
		public bool m_isPlayTriggerAnim;

		public AnimPlayOnceDistanceData m_triggerAnim;

		[Header("距离感应移动")]
		public bool m_isCanMove;

		public string m_moveTargetName;

		public EasingVector3ByDistanceData m_move;

		[Header("距离感应旋转")]
		public bool m_isCanRotate;

		public string m_rotateTargetName;

		public EasingVector3ByDistanceData m_rotate;

		[Header("距离运动时播放的动画")]
		public bool m_isPlayMotionAnim;

		public AnimPlayOnceDistanceData m_motionAnim;

		[Header("踩到地块触发的动画")]
		public bool m_isPlayStepOnAnim;

		public string m_stepOnAnimName;

		public static Data DefaultValue
		{
			get
			{
				Data result = default(Data);
				result.m_isPlayInitAnim = false;
				result.m_initAnimName = string.Empty;
				result.m_isPlayTriggerAnim = false;
				result.m_triggerAnim = AnimPlayOnceDistanceData.DefaultValue;
				result.m_isCanMove = false;
				result.m_moveTargetName = string.Empty;
				result.m_move = EasingVector3ByDistanceData.DefaultValue;
				result.m_isCanRotate = false;
				result.m_rotateTargetName = string.Empty;
				result.m_rotate = EasingVector3ByDistanceData.DefaultValue;
				result.m_isPlayMotionAnim = false;
				result.m_motionAnim = AnimPlayOnceDistanceData.DefaultValue;
				result.m_isPlayStepOnAnim = false;
				result.m_stepOnAnimName = string.Empty;
				return result;
			}
		}
	}

	[Serializable]
	public struct RebirthData
	{
		public RD_ElementTransform_DATA tran;

		public RD_ElementAnim_DATA anim;

		public RD_ElementTransform_DATA move;

		public RD_ElementTransform_DATA rotate;

		public bool trigger;

		public bool motionTrigger;
	}

	[SerializeField]
	private Animation m_animation;

	[SerializeField]
	private Collider m_collider;

	public Data m_data;

	[SerializeField]
	private float m_currentDistance;

	[Header("用于测试")]
	[SerializeField]
	[Range(0f, 1f)]
	private float m_currentAnimationTime;

	private GameObject m_moveTarget;

	private GameObject m_rotateTarget;

	private DistancSensore m_triggerAnimTrigger;

	private DistancSensore m_motionAnimTrigger;

	private EasingVector3ByDistance m_move;

	private EasingQuaternionByDistance m_rotate;

	private Vector3 m_firstPosition;

	private Vector3 m_firstRotation;

	private string m_firstAnimName;

	private bool m_isIgnoreTrigger;

	[SerializeField]
	private RebirthData m_rebirthData;

	public override Vector3 TileCenter
	{
		get
		{
			if (m_collider != null)
			{
				if (m_collider != null)
				{
					BoxCollider boxCollider = m_collider as BoxCollider;
					if (boxCollider != null)
					{
						return m_collider.transform.position + new Vector3(boxCollider.center.x, 0f, boxCollider.center.z);
					}
				}
				return m_collider.transform.position;
			}
			return base.TileCenter;
		}
	}

	public override float TileWidth
	{
		get
		{
			if (m_collider != null)
			{
				BoxCollider boxCollider = m_collider as BoxCollider;
				if (boxCollider != null)
				{
					return boxCollider.size.x / 2f + BaseTile.RecycleWidthTolerance;
				}
			}
			return base.TileWidth;
		}
	}

	public override float TileHeight
	{
		get
		{
			if (m_collider != null)
			{
				BoxCollider boxCollider = m_collider as BoxCollider;
				if (boxCollider != null)
				{
					return boxCollider.size.z / 2f + BaseTile.RecycleWidthTolerance;
				}
			}
			return base.TileHeight;
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
		FindTileChindren();
		OnInit();
		m_isIgnoreTrigger = false;
		if (m_data.m_isPlayInitAnim && m_animation != null)
		{
			PlayAnim(m_animation, m_data.m_initAnimName, true);
		}
		if (m_animation != null && m_animation.clip != null)
		{
			m_firstAnimName = m_animation.clip.name;
		}
		if (m_move != null && m_moveTarget != null)
		{
			m_firstPosition = m_moveTarget.transform.localPosition;
			m_moveTarget.transform.localPosition = m_data.m_move.m_from;
		}
		if (m_rotate != null && m_rotateTarget != null)
		{
			m_firstRotation = m_rotateTarget.transform.localEulerAngles;
			m_rotateTarget.transform.localEulerAngles = m_data.m_rotate.m_from;
		}
	}

	public override void UpdateElement()
	{
		if (Application.isPlaying)
		{
			m_currentDistance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		}
		base.UpdateElement();
		if (m_data.m_isPlayTriggerAnim && m_triggerAnimTrigger.Run(m_currentDistance) && m_animation != null)
		{
			PlayAnim(m_animation, m_data.m_triggerAnim.m_animName, true);
		}
		if (m_data.m_isPlayMotionAnim && m_motionAnimTrigger.Run(m_currentDistance) && m_animation != null)
		{
			PlayAnim(m_animation, m_data.m_motionAnim.m_animName, true);
		}
		if (m_move != null && m_moveTarget != null)
		{
			m_moveTarget.transform.localPosition = m_move.Run(m_currentDistance);
		}
		if (m_rotate != null && m_rotateTarget != null)
		{
			m_rotateTarget.transform.localRotation = m_rotate.Run(m_currentDistance);
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		m_isIgnoreTrigger = false;
		if (m_move != null && m_moveTarget != null)
		{
			m_moveTarget.transform.localPosition = m_firstPosition;
		}
		if (m_rotate != null && m_rotateTarget != null)
		{
			m_rotateTarget.transform.localEulerAngles = m_firstRotation;
		}
		if (m_animation != null)
		{
			if (!string.IsNullOrEmpty(m_firstAnimName))
			{
				m_animation[m_firstAnimName].normalizedTime = 0f;
				m_animation.Play();
				m_animation.Sample();
				m_animation.Stop();
			}
			else
			{
				m_animation.Stop();
			}
		}
		if (m_triggerAnimTrigger != null)
		{
			m_triggerAnimTrigger = null;
		}
		if (m_motionAnimTrigger != null)
		{
			m_motionAnimTrigger = null;
		}
		if (m_move != null)
		{
			m_move = null;
		}
		if (m_rotate != null)
		{
			m_rotate = null;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (!m_isIgnoreTrigger)
		{
			base.TriggerEnter(ball);
			if (m_data.m_isPlayStepOnAnim)
			{
				PlayAnim(m_animation, m_data.m_stepOnAnimName, true);
			}
			m_isIgnoreTrigger = true;
		}
	}

	public override string Write()
	{
		return JsonUtility.ToJson(m_data);
	}

	public override void Read(string info)
	{
		m_data = JsonUtility.FromJson<Data>(info);
	}

	public override byte[] WriteBytes()
	{
		return Bson.ToBson(m_data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		m_data = Bson.ToObject<Data>(bytes);
	}

	public override void SetDefaultValue(object[] objs)
	{
		m_data = (Data)objs[0];
	}

	private void FindTileChindren()
	{
		if (m_data.m_isCanMove || m_data.m_isCanRotate)
		{
			if (m_data.m_isCanMove && !string.IsNullOrEmpty(m_data.m_moveTargetName))
			{
				Transform transform = base.gameObject.transform.Find(m_data.m_moveTargetName);
				if (transform != null)
				{
					m_moveTarget = transform.gameObject;
				}
			}
			if (m_data.m_isCanRotate && !string.IsNullOrEmpty(m_data.m_rotateTargetName))
			{
				Transform transform2 = base.gameObject.transform.Find(m_data.m_rotateTargetName);
				if (transform2 != null)
				{
					m_rotateTarget = transform2.gameObject;
				}
			}
		}
		if (m_animation == null)
		{
			m_animation = base.gameObject.GetComponentInChildren<Animation>();
		}
		if (m_collider == null)
		{
			m_collider = base.gameObject.GetComponentInChildren<Collider>();
		}
	}

	private void OnInit()
	{
		if (m_data.m_isCanMove)
		{
			m_move = new EasingVector3ByDistance(m_data.m_move);
		}
		if (m_data.m_isCanRotate)
		{
			m_rotate = new EasingQuaternionByDistance(m_data.m_rotate);
		}
		if (m_data.m_isPlayTriggerAnim)
		{
			m_triggerAnimTrigger = new DistancSensore(m_data.m_triggerAnim.m_distance);
		}
		if (m_data.m_isPlayMotionAnim)
		{
			m_motionAnimTrigger = new DistancSensore(m_data.m_motionAnim.m_distance);
		}
	}

	public override byte[] RebirthWriteByteData()
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.tran = base.gameObject.transform.GetTransData();
		if (m_animation != null)
		{
			rebirthData.anim = m_animation.GetAnimData();
		}
		if (m_moveTarget != null)
		{
			rebirthData.move = m_moveTarget.transform.GetTransData();
		}
		if (m_rotateTarget != null)
		{
			rebirthData.rotate = m_rotateTarget.transform.GetTransData();
		}
		if (m_triggerAnimTrigger != null)
		{
			rebirthData.trigger = m_triggerAnimTrigger.m_isTriggerStay;
		}
		if (m_motionAnimTrigger != null)
		{
			rebirthData.motionTrigger = m_motionAnimTrigger.m_isTriggerStay;
		}
		return Bson.ToBson(rebirthData);
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RebirthData>(rd_data);
		m_isIgnoreTrigger = true;
		base.gameObject.transform.SetTransData(m_rebirthData.tran);
		if (m_animation != null)
		{
			m_animation.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		}
		if (m_moveTarget != null)
		{
			m_moveTarget.transform.SetTransData(m_rebirthData.move);
		}
		if (m_rotateTarget != null)
		{
			m_rotateTarget.transform.SetTransData(m_rebirthData.rotate);
		}
		if (m_triggerAnimTrigger != null)
		{
			m_triggerAnimTrigger.m_isTriggerStay = m_rebirthData.trigger;
		}
		if (m_motionAnimTrigger != null)
		{
			m_motionAnimTrigger.m_isTriggerStay = m_rebirthData.motionTrigger;
		}
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (m_animation != null)
		{
			m_animation.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		}
		m_isIgnoreTrigger = false;
	}

	public override object RebirthWriteData()
	{
		RebirthData rebirthData = default(RebirthData);
		rebirthData.tran = base.gameObject.transform.GetTransData();
		if (m_animation != null)
		{
			rebirthData.anim = m_animation.GetAnimData();
		}
		if (m_moveTarget != null)
		{
			rebirthData.move = m_moveTarget.transform.GetTransData();
		}
		if (m_rotateTarget != null)
		{
			rebirthData.rotate = m_rotateTarget.transform.GetTransData();
		}
		if (m_triggerAnimTrigger != null)
		{
			rebirthData.trigger = m_triggerAnimTrigger.m_isTriggerStay;
		}
		if (m_motionAnimTrigger != null)
		{
			rebirthData.motionTrigger = m_motionAnimTrigger.m_isTriggerStay;
		}
		return JsonUtility.ToJson(rebirthData);
	}

	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RebirthData>((string)rd_data);
		m_isIgnoreTrigger = true;
		base.gameObject.transform.SetTransData(m_rebirthData.tran);
		if (m_animation != null)
		{
			m_animation.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		}
		if (m_moveTarget != null)
		{
			m_moveTarget.transform.SetTransData(m_rebirthData.move);
		}
		if (m_rotateTarget != null)
		{
			m_rotateTarget.transform.SetTransData(m_rebirthData.rotate);
		}
		if (m_triggerAnimTrigger != null)
		{
			m_triggerAnimTrigger.m_isTriggerStay = m_rebirthData.trigger;
		}
		if (m_motionAnimTrigger != null)
		{
			m_motionAnimTrigger.m_isTriggerStay = m_rebirthData.motionTrigger;
		}
	}

	public override void RebirthStartGame(object rd_data)
	{
		if (m_animation != null)
		{
			m_animation.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		}
		m_isIgnoreTrigger = false;
	}
}
