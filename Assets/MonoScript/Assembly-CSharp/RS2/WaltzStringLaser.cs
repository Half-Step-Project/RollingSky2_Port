using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class WaltzStringLaser : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float BeginDistance1;

			public float EndDistance1;

			public float LaserLength1;

			public float ShootSpeed1;

			public bool IfDouble;

			public float BeginDistance2;

			public float EndDistance2;

			public float LaserLength2;

			public float ShootSpeed2;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				BeginDistance1 = bytes.GetSingle(ref startIndex);
				EndDistance1 = bytes.GetSingle(ref startIndex);
				LaserLength1 = bytes.GetSingle(ref startIndex);
				ShootSpeed1 = bytes.GetSingle(ref startIndex);
				IfDouble = bytes.GetBoolean(ref startIndex);
				BeginDistance2 = bytes.GetSingle(ref startIndex);
				EndDistance2 = bytes.GetSingle(ref startIndex);
				LaserLength2 = bytes.GetSingle(ref startIndex);
				ShootSpeed2 = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(BeginDistance1.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndDistance1.GetBytes(), ref offset);
					memoryStream.WriteByteArray(LaserLength1.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ShootSpeed1.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfDouble.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginDistance2.GetBytes(), ref offset);
					memoryStream.WriteByteArray(EndDistance2.GetBytes(), ref offset);
					memoryStream.WriteByteArray(LaserLength2.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ShootSpeed2.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		private static readonly string nodeShootRoot = "model";

		private static readonly string nodeDebugRoot = "triggerPoints";

		private static readonly string nodeDebugBegin1 = "beginPoint1";

		private static readonly string nodeDebugEnd1 = "endPoint1";

		private static readonly string nodeDebugBegin2 = "beginPoint2";

		private static readonly string nodeDebugEnd2 = "endPoint2";

		public static readonly float baseLength = 30f;

		public TriggerData data;

		private bool ifBeginShoot = true;

		private int cycleIndex;

		private Transform shootNode;

		private Animation anim;

		private RD_WaltzStringLaser_DATA m_rebirthData;

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
			shootNode = base.transform.Find(nodeShootRoot);
			if ((bool)shootNode)
			{
				shootNode.SetLocalScaleZ(0f);
				shootNode.SetLocalPositionZ(data.LaserLength1);
			}
			anim = base.transform.GetComponentInChildren<Animation>();
			commonState = CommonState.None;
			cycleIndex = 0;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			if ((bool)shootNode)
			{
				shootNode.SetLocalScaleZ(0f);
				shootNode.SetLocalPositionZ(data.LaserLength1);
				shootNode.gameObject.SetActive(true);
			}
			PlayAnim(anim, false);
			commonState = CommonState.None;
			cycleIndex = 0;
		}

		public override void UpdateElement()
		{
			if (commonState == CommonState.None)
			{
				UpdateWait();
			}
			else if (commonState == CommonState.Active)
			{
				UpdateShootForward();
			}
			else if (commonState == CommonState.InActive)
			{
				UpdateShootBackward();
			}
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (!GameController.IfNotDeath)
			{
				ball.CrashBall();
			}
		}

		private void UpdateWait()
		{
			float num = -1f;
			num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			num = ((cycleIndex == 0) ? (num - data.BeginDistance1) : (num - data.BeginDistance2));
			if (num >= 0f)
			{
				shootNode.gameObject.SetActive(true);
				commonState = CommonState.Active;
				ifBeginShoot = true;
				PlayAnim(anim, true);
			}
		}

		private void UpdateShootForward()
		{
			float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			if (ifBeginShoot)
			{
				float num2 = num;
				float num3 = 0f;
				if (cycleIndex == 0)
				{
					num2 -= data.BeginDistance1;
					num3 = num2 / data.ShootSpeed1;
					ShootByPercent(shootNode, num3, 0f, data.LaserLength1);
				}
				else if (cycleIndex == 1)
				{
					num2 -= data.BeginDistance2;
					num3 = num2 / data.ShootSpeed2;
					ShootByPercent(shootNode, num3, 0f, data.LaserLength2);
				}
				if (num3 >= 1f)
				{
					ifBeginShoot = false;
				}
			}
			float num4 = num;
			if (cycleIndex == 0)
			{
				num4 -= data.EndDistance1;
			}
			else if (cycleIndex == 1)
			{
				num4 -= data.EndDistance2;
			}
			if (num4 > 0f)
			{
				commonState = CommonState.InActive;
				ifBeginShoot = true;
			}
		}

		private void UpdateShootBackward()
		{
			float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
			if (!ifBeginShoot)
			{
				return;
			}
			float num2 = num;
			float num3 = 0f;
			if (cycleIndex == 0)
			{
				num2 -= data.EndDistance1;
				num3 = num2 / data.ShootSpeed1;
				ShootByPercent(shootNode, num3, data.LaserLength1, 0f);
			}
			else if (cycleIndex == 1)
			{
				num2 -= data.EndDistance1;
				num3 = num2 / data.ShootSpeed2;
				ShootByPercent(shootNode, num3, data.LaserLength2, 0f);
			}
			if (!(num3 >= 1f))
			{
				return;
			}
			PlayAnim(anim, false);
			if ((bool)shootNode)
			{
				shootNode.gameObject.SetActive(false);
			}
			if (data.IfDouble)
			{
				if (cycleIndex == 0)
				{
					cycleIndex = 1;
					commonState = CommonState.None;
					if ((bool)shootNode)
					{
						shootNode.SetLocalPositionZ(data.LaserLength2 / baseLength);
					}
				}
				else
				{
					commonState = CommonState.End;
				}
			}
			else
			{
				commonState = CommonState.End;
			}
			ifBeginShoot = false;
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TriggerData>(info);
		}

		public override string Write()
		{
			Transform transform = base.transform.Find(nodeDebugRoot);
			if ((bool)transform)
			{
				Transform transform2 = transform.Find(nodeDebugBegin1);
				Transform transform3 = transform.Find(nodeDebugEnd1);
				Transform transform4 = transform.Find(nodeDebugBegin2);
				Transform transform5 = transform.Find(nodeDebugEnd2);
				data.BeginDistance1 = transform2.localPosition.z;
				data.EndDistance1 = transform3.localPosition.z;
				data.BeginDistance2 = transform4.localPosition.z;
				data.EndDistance2 = transform5.localPosition.z;
			}
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			Transform transform = base.transform.Find(nodeDebugRoot);
			if ((bool)transform)
			{
				Transform transform2 = transform.Find(nodeDebugBegin1);
				Transform transform3 = transform.Find(nodeDebugEnd1);
				Transform transform4 = transform.Find(nodeDebugBegin2);
				Transform transform5 = transform.Find(nodeDebugEnd2);
				data.BeginDistance1 = transform2.localPosition.z;
				data.EndDistance1 = transform3.localPosition.z;
				data.BeginDistance2 = transform4.localPosition.z;
				data.EndDistance2 = transform5.localPosition.z;
			}
			return StructTranslatorUtility.ToByteArray(data);
		}

		private void ShootByPercent(Transform trans, float percent, float fromLen, float toLen)
		{
			if ((bool)trans)
			{
				trans.SetLocalScaleZ(Mathf.Lerp(fromLen, toLen, percent) / baseLength);
			}
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			Transform transform = base.transform.Find(nodeDebugRoot);
			if ((bool)transform)
			{
				Transform obj = transform.Find(nodeDebugBegin1);
				Transform transform2 = transform.Find(nodeDebugEnd1);
				Transform transform3 = transform.Find(nodeDebugBegin2);
				Transform transform4 = transform.Find(nodeDebugEnd2);
				Gizmos.color = Color.green;
				Gizmos.DrawCube(obj.position, new Vector3(1f, 0.1f, 0.1f));
				Gizmos.color = Color.red;
				Gizmos.DrawCube(transform2.position, new Vector3(1f, 0.1f, 0.1f));
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(transform3.position, new Vector3(1f, 0.1f, 0.1f));
				Gizmos.color = Color.yellow;
				Gizmos.DrawCube(transform4.position, new Vector3(1f, 0.1f, 0.1f));
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_WaltzStringLaser_DATA
			{
				ifBeginShoot = (ifBeginShoot ? 1 : 0),
				cycleIndex = cycleIndex,
				shootNode = shootNode.GetTransData(),
				anim = anim.GetAnimData(),
				commonState = commonState
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			m_rebirthData = JsonUtility.FromJson<RD_WaltzStringLaser_DATA>(rd_data as string);
			ifBeginShoot = m_rebirthData.ifBeginShoot == 1;
			cycleIndex = m_rebirthData.cycleIndex;
			shootNode.SetTransData(m_rebirthData.shootNode);
			anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			commonState = m_rebirthData.commonState;
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			if (m_rebirthData != null)
			{
				anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
			}
			m_rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			m_rebirthData = Bson.ToObject<RD_WaltzStringLaser_DATA>(rd_data);
			ifBeginShoot = m_rebirthData.ifBeginShoot == 1;
			cycleIndex = m_rebirthData.cycleIndex;
			shootNode.SetTransData(m_rebirthData.shootNode);
			anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
			commonState = m_rebirthData.commonState;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_WaltzStringLaser_DATA
			{
				ifBeginShoot = (ifBeginShoot ? 1 : 0),
				cycleIndex = cycleIndex,
				shootNode = shootNode.GetTransData(),
				anim = anim.GetAnimData(),
				commonState = commonState
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			if (m_rebirthData != null)
			{
				anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
			}
			m_rebirthData = null;
		}
	}
}
