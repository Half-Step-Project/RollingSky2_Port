using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class FreeMoveCrownByCouple : BaseAward, IAwardComplete, IAward
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public Vector3[] PathPoints;

			public int FillNum;

			[HideInInspector]
			public Vector3[] BezierPoints;

			[HideInInspector]
			public Vector3 TriggerPos;

			public bool IfFollowDir;

			public bool IfAutoSpeed;

			public bool IfLoop;

			public float MoveSpeed;

			public float RotateSpeed;

			[Label]
			public int sortID;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				PathPoints = bytes.GetVector3Array(ref startIndex);
				FillNum = bytes.GetInt32(ref startIndex);
				BezierPoints = bytes.GetVector3Array(ref startIndex);
				TriggerPos = bytes.GetVector3(ref startIndex);
				IfFollowDir = bytes.GetBoolean(ref startIndex);
				IfAutoSpeed = bytes.GetBoolean(ref startIndex);
				IfLoop = bytes.GetBoolean(ref startIndex);
				MoveSpeed = bytes.GetSingle(ref startIndex);
				RotateSpeed = bytes.GetSingle(ref startIndex);
				sortID = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(PathPoints.GetBytes(), ref offset);
					memoryStream.WriteByteArray(FillNum.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BezierPoints.GetBytes(), ref offset);
					memoryStream.WriteByteArray(TriggerPos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfFollowDir.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfAutoSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfLoop.GetBytes(), ref offset);
					memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(RotateSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(sortID.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public static readonly string ModelNode = "model";

		public static readonly string AwardNode = "award";

		public static readonly string EffectNode = "effect";

		public static readonly string TriggerNode = "trigger";

		public const string NodeShowModelPath_SP = "award/showNode";

		public const string NodeHideModelPath_SP = "award/hideNode";

		public TriggerData data;

		private BezierMover bezierMover = new BezierMover();

		private float moveSpeed;

		private Vector3 cacheLocalPos;

		protected GameObject model;

		protected GameObject award;

		protected GameObject trigger;

		protected Animation anim;

		protected ParticleSystem[] particle;

		private RD_FreeMoveCrownByCouple_DATA m_rebirthData;

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
			Transform transform = base.transform.Find(AwardNode);
			if ((bool)transform)
			{
				award = transform.gameObject;
				cacheLocalPos = transform.localPosition;
			}
			Transform transform2 = transform.Find(EffectNode);
			if ((bool)transform2)
			{
				particle = transform2.GetComponentsInChildren<ParticleSystem>();
			}
			commonState = CommonState.None;
			moveSpeed = (data.IfAutoSpeed ? Railway.theRailway.SpeedForward : data.MoveSpeed);
			bezierMover = new BezierMover(data.BezierPoints);
			Transform transform3 = base.transform.Find("award/showNode");
			Transform transform4 = base.transform.Find("award/hideNode");
			modelShowNode = (transform3 ? transform3.gameObject : null);
			modelHideNode = (transform4 ? transform4.gameObject : null);
			animShowNode = (modelShowNode ? modelShowNode.GetComponent<Animation>() : null);
			animHideNode = (modelHideNode ? modelHideNode.GetComponent<Animation>() : null);
			InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
			DropType dropType = GetDropType();
			bool flag = dataModule.IsShowForAwardByDropType(dropType, data.sortID);
			anim = (flag ? animShowNode : animHideNode);
			if ((bool)modelShowNode)
			{
				modelShowNode.SetActive(flag);
			}
			if ((bool)modelHideNode)
			{
				modelHideNode.SetActive(!flag);
			}
			model = (flag ? modelShowNode : modelHideNode);
			if ((bool)model)
			{
				model.SetActive(false);
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			bezierMover.ResetData();
			if ((bool)model)
			{
				model.SetActive(false);
			}
			award.transform.localPosition = cacheLocalPos;
			commonState = CommonState.End;
		}

		public override void UpdateElement()
		{
			if (commonState == CommonState.Active)
			{
				float disLocalZ = moveSpeed * Time.deltaTime;
				Vector3 localPosition = award.transform.localPosition;
				Vector3 targetLocPos = Vector3.zero;
				Vector3 moveLocDir = Vector3.forward;
				bezierMover.MoveForwardByZ(disLocalZ, award.transform.parent, localPosition, ref targetLocPos, ref moveLocDir);
				award.transform.localPosition = targetLocPos;
				if (data.IfFollowDir)
				{
					moveLocDir.y = 0f;
					award.transform.forward = moveLocDir;
				}
				award.transform.Rotate(Vector3.up * data.RotateSpeed);
			}
		}

		protected override void OnCollideBall(BaseRole ball)
		{
			if (model.gameObject.activeSelf)
			{
				ball.GainCrown(m_uuId, data.sortID);
				model.SetActive(false);
				PlayParticle(particle, true);
				PlaySoundEffect();
				commonState = CommonState.End;
			}
		}

		protected override void OnCollideCouple(BaseCouple couple, Collider collider)
		{
			if (commonState != 0)
			{
				return;
			}
			if ((bool)model)
			{
				model.SetActive(true);
			}
			if ((bool)anim)
			{
				if (data.IfLoop)
				{
					anim.wrapMode = WrapMode.Loop;
				}
				else
				{
					anim.wrapMode = WrapMode.ClampForever;
				}
				PlayAnim(anim, true);
			}
			commonState = CommonState.Active;
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TriggerData>(info);
			base.transform.Find(TriggerNode).position = data.TriggerPos;
		}

		public override string Write()
		{
			if (trigger == null)
			{
				trigger = base.transform.Find(TriggerNode).gameObject;
			}
			data.TriggerPos = trigger.transform.position;
			if (award == null)
			{
				award = base.transform.Find(AwardNode).gameObject;
			}
			Vector3[] worldPointsByLocal = BaseElement.GetWorldPointsByLocal(data.PathPoints, award.transform);
			data.BezierPoints = Bezier.GetPathByPositions(worldPointsByLocal, data.FillNum);
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
			base.transform.Find(TriggerNode).position = data.TriggerPos;
		}

		public override byte[] WriteBytes()
		{
			if (trigger == null)
			{
				trigger = base.transform.Find(TriggerNode).gameObject;
			}
			data.TriggerPos = trigger.transform.position;
			if (award == null)
			{
				award = base.transform.Find(AwardNode).gameObject;
			}
			Vector3[] worldPointsByLocal = BaseElement.GetWorldPointsByLocal(data.PathPoints, award.transform);
			data.BezierPoints = Bezier.GetPathByPositions(worldPointsByLocal, data.FillNum);
			return StructTranslatorUtility.ToByteArray(data);
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (data.PathPoints != null && data.PathPoints.Length > 2)
			{
				BaseElement.DrawWorldPath(Bezier.GetPathByPositions(BaseElement.GetWorldPointsByLocal(data.PathPoints, base.transform)), Color.red);
			}
		}

		public int GetAwardSortID()
		{
			return data.sortID;
		}

		public void SetAwardSortID(int id)
		{
			data.sortID = id;
		}

		public virtual DropType GetDropType()
		{
			return DropType.CROWN;
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
			m_rebirthData = JsonUtility.FromJson<RD_FreeMoveCrownByCouple_DATA>(rd_data as string);
			base.transform.SetTransData(m_rebirthData.transform);
			bezierMover.SetBezierData(m_rebirthData.bezierMover);
			if (model != null)
			{
				model.SetActive(m_rebirthData.model == 1);
			}
			if (award != null)
			{
				award.transform.SetTransData(m_rebirthData.award);
			}
			if (trigger != null)
			{
				trigger.transform.SetTransData(m_rebirthData.trigger);
			}
			anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			particle.SetParticlesData(m_rebirthData.particle, ProcessState.Pause);
			commonState = m_rebirthData.commonState;
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			RD_FreeMoveCrownByCouple_DATA rD_FreeMoveCrownByCouple_DATA = new RD_FreeMoveCrownByCouple_DATA();
			rD_FreeMoveCrownByCouple_DATA.bezierMover = bezierMover.GetBezierData();
			rD_FreeMoveCrownByCouple_DATA.transform = base.transform.GetTransData();
			if (model != null)
			{
				rD_FreeMoveCrownByCouple_DATA.model = (model.activeSelf ? 1 : 0);
			}
			if (award != null)
			{
				rD_FreeMoveCrownByCouple_DATA.award = award.transform.GetTransData();
			}
			if (trigger != null)
			{
				rD_FreeMoveCrownByCouple_DATA.trigger = trigger.transform.GetTransData();
			}
			rD_FreeMoveCrownByCouple_DATA.anim = anim.GetAnimData();
			rD_FreeMoveCrownByCouple_DATA.particle = particle.GetParticlesData();
			rD_FreeMoveCrownByCouple_DATA.commonState = commonState;
			return JsonUtility.ToJson(rD_FreeMoveCrownByCouple_DATA);
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			if (m_rebirthData != null)
			{
				anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
				particle.SetParticlesData(m_rebirthData.particle, ProcessState.UnPause);
				m_rebirthData = null;
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override void RebirthReadDataForDrop(object rd_data)
		{
			particle.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			if (model != null)
			{
				model.SetActive(false);
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override void RebirthStartGameForDrop(object rd_data)
		{
			particle.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			if (model != null)
			{
				model.SetActive(false);
			}
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			m_rebirthData = Bson.ToObject<RD_FreeMoveCrownByCouple_DATA>(rd_data);
			base.transform.SetTransData(m_rebirthData.transform);
			bezierMover.SetBezierData(m_rebirthData.bezierMover);
			if (model != null)
			{
				model.SetActive(m_rebirthData.model == 1);
			}
			if (award != null)
			{
				award.transform.SetTransData(m_rebirthData.award);
			}
			if (trigger != null)
			{
				trigger.transform.SetTransData(m_rebirthData.trigger);
			}
			anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			particle.SetParticlesData(m_rebirthData.particle, ProcessState.Pause);
			commonState = m_rebirthData.commonState;
		}

		public override byte[] RebirthWriteByteData()
		{
			RD_FreeMoveCrownByCouple_DATA rD_FreeMoveCrownByCouple_DATA = new RD_FreeMoveCrownByCouple_DATA();
			rD_FreeMoveCrownByCouple_DATA.bezierMover = bezierMover.GetBezierData();
			rD_FreeMoveCrownByCouple_DATA.transform = base.transform.GetTransData();
			if (model != null)
			{
				rD_FreeMoveCrownByCouple_DATA.model = (model.activeSelf ? 1 : 0);
			}
			if (award != null)
			{
				rD_FreeMoveCrownByCouple_DATA.award = award.transform.GetTransData();
			}
			if (trigger != null)
			{
				rD_FreeMoveCrownByCouple_DATA.trigger = trigger.transform.GetTransData();
			}
			rD_FreeMoveCrownByCouple_DATA.anim = anim.GetAnimData();
			rD_FreeMoveCrownByCouple_DATA.particle = particle.GetParticlesData();
			rD_FreeMoveCrownByCouple_DATA.commonState = commonState;
			return Bson.ToBson(rD_FreeMoveCrownByCouple_DATA);
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			if (m_rebirthData != null)
			{
				anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
				particle.SetParticlesData(m_rebirthData.particle, ProcessState.UnPause);
				m_rebirthData = null;
			}
		}

		public override void RebirthReadByteDataForDrop(byte[] rd_data)
		{
			particle.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			if (model != null)
			{
				model.SetActive(false);
			}
		}

		public override void RebirthStartGameByteDataForDrop(byte[] rd_data)
		{
			particle.StopParticle(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			if (model != null)
			{
				model.SetActive(false);
			}
		}
	}
}
