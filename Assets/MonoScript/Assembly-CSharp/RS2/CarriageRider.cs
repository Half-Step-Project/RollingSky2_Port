using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CarriageRider : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float BeginDistance;

			public Vector3[] PathPoints;

			public int FillNum;

			[HideInInspector]
			public Vector3[] BezierPoints;

			public bool IfFollowDir;

			public bool IfAutoSpeed;

			public float SpeedScaler;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				BeginDistance = bytes.GetSingle(ref startIndex);
				PathPoints = bytes.GetVector3Array(ref startIndex);
				FillNum = bytes.GetInt32(ref startIndex);
				BezierPoints = bytes.GetVector3Array(ref startIndex);
				IfFollowDir = bytes.GetBoolean(ref startIndex);
				IfAutoSpeed = bytes.GetBoolean(ref startIndex);
				SpeedScaler = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(PathPoints.GetBytes(), ref offset);
					memoryStream.WriteByteArray(FillNum.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BezierPoints.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfFollowDir.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfAutoSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public TriggerData data;

		private BezierMover bezierMover = new BezierMover();

		private float moveSpeed;

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
			commonState = CommonState.None;
			moveSpeed = (data.IfAutoSpeed ? data.SpeedScaler : Railway.theRailway.SpeedForward);
			bezierMover = new BezierMover(data.BezierPoints);
		}

		public override void ResetElement()
		{
			base.ResetElement();
			bezierMover.ResetData();
			commonState = CommonState.End;
		}

		public override void UpdateElement()
		{
			if (commonState == CommonState.None)
			{
				Vector3 position = BaseRole.theBall.transform.position;
				Vector3 position2 = base.transform.position;
				if (base.groupTransform.InverseTransformPoint(position).z - base.groupTransform.InverseTransformPoint(position2).z - data.BeginDistance > 0f)
				{
					base.transform.Find("model").GetComponent<Animation>().Play();
					commonState = CommonState.Active;
				}
			}
			else if (commonState == CommonState.Active)
			{
				float disLocalZ = moveSpeed * Time.deltaTime;
				Vector3 localPosition = base.transform.localPosition;
				Vector3 targetLocPos = Vector3.zero;
				Vector3 moveLocDir = Vector3.forward;
				bool num = bezierMover.MoveForwardByZ(disLocalZ, base.groupTransform, localPosition, ref targetLocPos, ref moveLocDir);
				base.transform.localPosition = targetLocPos;
				if (data.IfFollowDir)
				{
					moveLocDir.y = 0f;
					base.transform.forward = moveLocDir;
				}
				if (num)
				{
					commonState = CommonState.End;
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (commonState == CommonState.Active)
			{
				FreeMoveBarrelEnemy gameComponent = other.gameObject.GetGameComponent<FreeMoveBarrelEnemy>();
				if (gameComponent != null)
				{
					gameComponent.OnCollideWithCarriage();
				}
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TriggerData>(info);
		}

		public override string Write()
		{
			Vector3[] worldPointsByLocal = BaseElement.GetWorldPointsByLocal(data.PathPoints, base.transform);
			data.BezierPoints = Bezier.GetPathByPositions(worldPointsByLocal, data.FillNum);
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			Vector3[] worldPointsByLocal = BaseElement.GetWorldPointsByLocal(data.PathPoints, base.transform);
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

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_CarriageRider_DATA rD_CarriageRider_DATA = JsonUtility.FromJson<RD_CarriageRider_DATA>(rd_data as string);
			base.transform.SetTransData(rD_CarriageRider_DATA.transformData);
			commonState = (CommonState)rD_CarriageRider_DATA.commonState;
			bezierMover.SetBezierData(rD_CarriageRider_DATA.bezierMoverData);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_CarriageRider_DATA
			{
				transformData = base.transform.GetTransData(),
				commonState = (int)commonState,
				bezierMoverData = bezierMover.GetBezierData()
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_CarriageRider_DATA rD_CarriageRider_DATA = Bson.ToObject<RD_CarriageRider_DATA>(rd_data);
			base.transform.SetTransData(rD_CarriageRider_DATA.transformData);
			commonState = (CommonState)rD_CarriageRider_DATA.commonState;
			bezierMover.SetBezierData(rD_CarriageRider_DATA.bezierMoverData);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_CarriageRider_DATA
			{
				transformData = base.transform.GetTransData(),
				commonState = (int)commonState,
				bezierMoverData = bezierMover.GetBezierData()
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
