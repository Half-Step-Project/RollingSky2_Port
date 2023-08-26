using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CoupleFollowTrigger : BaseTriggerBox
	{
		private enum FollowState
		{
			None,
			RoleEnter,
			Following,
			RoleOut,
			EndFollow
		}

		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float BeginDistance;

			public Vector3 BeginPos;

			public CoupleThiefAnim FirstAnimType;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				BeginDistance = bytes.GetSingle(ref startIndex);
				BeginPos = bytes.GetVector3(ref startIndex);
				FirstAnimType = (CoupleThiefAnim)bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(((int)FirstAnimType).GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public static readonly string ModelNode = "role";

		public TriggerData m_data;

		private FollowState m_state;

		private Queue<Vector3> m_cacheRolePosition;

		private Animator m_coupleAnimator;

		private GameObject m_couple;

		private RD_CoupleFollowTrigger_DATA rebirthData;

		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<CoupleDetachEventArgs>.EventId, OnCoupleDetach);
			Mod.Event.Subscribe(EventArgs<CoupleThiefAnimEventArgs>.EventId, OnCouplePlayAnim);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<CoupleDetachEventArgs>.EventId, OnCoupleDetach);
			Mod.Event.Unsubscribe(EventArgs<CoupleThiefAnimEventArgs>.EventId, OnCouplePlayAnim);
		}

		public override void Initialize()
		{
			m_state = FollowState.None;
			int capacity = (int)(Railway.theRailway.SpeedForward / m_data.BeginDistance * 61f);
			m_cacheRolePosition = new Queue<Vector3>(capacity);
			m_coupleAnimator = GetComponentInChildren<Animator>(true);
			Transform transform = base.transform.Find(ModelNode);
			if ((bool)transform)
			{
				m_couple = transform.gameObject;
				m_couple.SetActive(false);
			}
		}

		public override void UpdateElement()
		{
			if (m_state == FollowState.RoleEnter)
			{
				m_cacheRolePosition.Enqueue(BaseRole.BallPosition);
				if (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z >= m_data.BeginDistance)
				{
					m_state = FollowState.Following;
				}
			}
			else if (m_state == FollowState.Following)
			{
				m_cacheRolePosition.Enqueue(BaseRole.BallPosition);
				if (m_cacheRolePosition.Count > 0)
				{
					Vector3 position = m_cacheRolePosition.Dequeue();
					base.transform.position = position;
					if (m_cacheRolePosition.Count != 0)
					{
						base.transform.LookAt(m_cacheRolePosition.Peek());
					}
				}
			}
			else
			{
				if (m_state != FollowState.RoleOut)
				{
					return;
				}
				if (m_cacheRolePosition.Count > 0)
				{
					Vector3 position2 = m_cacheRolePosition.Dequeue();
					base.transform.position = position2;
					if (m_cacheRolePosition.Count != 0)
					{
						base.transform.LookAt(m_cacheRolePosition.Peek());
					}
				}
				else
				{
					m_state = FollowState.EndFollow;
					m_couple.SetActive(false);
				}
			}
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (m_state == FollowState.None)
			{
				m_state = FollowState.RoleEnter;
				m_couple.SetActive(true);
				CouplePlayAnim(m_data.FirstAnimType.ToString());
			}
		}

		private void OnTriggerEnter(Collider collider)
		{
			BaseElement gameComponent = collider.gameObject.GetGameComponent<CoupleAnimatorTrigger>();
			if ((bool)gameComponent)
			{
				gameComponent.CoupleTriggerEnter(null, collider);
			}
		}

		private void OnCoupleDetach(object sender, Foundation.EventArgs e)
		{
			m_state = FollowState.RoleOut;
		}

		private void OnCouplePlayAnim(object sender, Foundation.EventArgs e)
		{
			CoupleThiefAnimEventArgs coupleThiefAnimEventArgs = (CoupleThiefAnimEventArgs)e;
			if (coupleThiefAnimEventArgs != null && coupleThiefAnimEventArgs.Receiver == CoupleAnimReceiverType.eFollower && m_coupleAnimator != null)
			{
				CouplePlayAnim(((CoupleThiefAnim)coupleThiefAnimEventArgs.AnimType).ToString());
			}
		}

		private void CouplePlayAnim(string animName)
		{
			m_coupleAnimator.Play(animName);
		}

		public override void ResetElement()
		{
			m_state = FollowState.None;
			m_couple.SetActive(false);
			m_cacheRolePosition.Clear();
			m_couple.transform.localPosition = Vector3.zero;
		}

		public override void Read(string info)
		{
			m_data = JsonUtility.FromJson<TriggerData>(info);
			base.transform.Find("triggerPoint").position = m_data.BeginPos;
		}

		public override string Write()
		{
			Transform transform = base.transform.Find("triggerPoint");
			m_data.BeginPos = transform.position;
			m_data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			return JsonUtility.ToJson(m_data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			m_data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
			base.transform.Find("triggerPoint").position = m_data.BeginPos;
		}

		public override byte[] WriteBytes()
		{
			Transform transform = base.transform.Find("triggerPoint");
			m_data.BeginPos = transform.position;
			m_data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
			return StructTranslatorUtility.ToByteArray(m_data);
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			Transform transform = base.transform.Find("triggerPoint");
			if ((bool)transform)
			{
				Color color = Gizmos.color;
				Gizmos.color = Color.green;
				Gizmos.DrawCube(transform.position, new Vector3(1f, 0.1f, 0.1f));
				Gizmos.color = color;
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_CoupleFollowTrigger_DATA rD_CoupleFollowTrigger_DATA = (rebirthData = JsonUtility.FromJson<RD_CoupleFollowTrigger_DATA>(rd_data as string));
			base.transform.SetTransData(rD_CoupleFollowTrigger_DATA.transformData);
			m_state = (FollowState)rD_CoupleFollowTrigger_DATA.state;
			for (int i = 0; i < rD_CoupleFollowTrigger_DATA.cachedLength; i++)
			{
				m_cacheRolePosition.Enqueue(rD_CoupleFollowTrigger_DATA.cachedPos);
			}
			m_coupleAnimator.SetAnimData(rD_CoupleFollowTrigger_DATA.coupleAnimatorData, ProcessState.Pause);
			m_couple.SetActive(rD_CoupleFollowTrigger_DATA.ifShowCouple);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			RD_CoupleFollowTrigger_DATA rD_CoupleFollowTrigger_DATA = new RD_CoupleFollowTrigger_DATA();
			rD_CoupleFollowTrigger_DATA.transformData = base.transform.GetTransData();
			rD_CoupleFollowTrigger_DATA.state = (int)m_state;
			rD_CoupleFollowTrigger_DATA.cachedLength = m_cacheRolePosition.Count;
			if (rD_CoupleFollowTrigger_DATA.cachedLength > 0)
			{
				rD_CoupleFollowTrigger_DATA.cachedPos = m_cacheRolePosition.Peek();
			}
			rD_CoupleFollowTrigger_DATA.coupleAnimatorData = m_coupleAnimator.GetAnimData();
			rD_CoupleFollowTrigger_DATA.ifShowCouple = m_couple != null && m_couple.activeSelf;
			return JsonUtility.ToJson(rD_CoupleFollowTrigger_DATA);
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			m_coupleAnimator.SetAnimData(rebirthData.coupleAnimatorData, ProcessState.UnPause);
			rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_CoupleFollowTrigger_DATA rD_CoupleFollowTrigger_DATA = (rebirthData = Bson.ToObject<RD_CoupleFollowTrigger_DATA>(rd_data));
			base.transform.SetTransData(rD_CoupleFollowTrigger_DATA.transformData);
			m_state = (FollowState)rD_CoupleFollowTrigger_DATA.state;
			for (int i = 0; i < rD_CoupleFollowTrigger_DATA.cachedLength; i++)
			{
				m_cacheRolePosition.Enqueue(rD_CoupleFollowTrigger_DATA.cachedPos);
			}
			m_coupleAnimator.SetAnimData(rD_CoupleFollowTrigger_DATA.coupleAnimatorData, ProcessState.Pause);
			m_couple.SetActive(rD_CoupleFollowTrigger_DATA.ifShowCouple);
		}

		public override byte[] RebirthWriteByteData()
		{
			RD_CoupleFollowTrigger_DATA rD_CoupleFollowTrigger_DATA = new RD_CoupleFollowTrigger_DATA();
			rD_CoupleFollowTrigger_DATA.transformData = base.transform.GetTransData();
			rD_CoupleFollowTrigger_DATA.state = (int)m_state;
			rD_CoupleFollowTrigger_DATA.cachedLength = m_cacheRolePosition.Count;
			if (rD_CoupleFollowTrigger_DATA.cachedLength > 0)
			{
				rD_CoupleFollowTrigger_DATA.cachedPos = m_cacheRolePosition.Peek();
			}
			rD_CoupleFollowTrigger_DATA.coupleAnimatorData = m_coupleAnimator.GetAnimData();
			rD_CoupleFollowTrigger_DATA.ifShowCouple = m_couple != null && m_couple.activeSelf;
			return Bson.ToBson(rD_CoupleFollowTrigger_DATA);
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			m_coupleAnimator.SetAnimData(rebirthData.coupleAnimatorData, ProcessState.UnPause);
			rebirthData = null;
		}
	}
}
