using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class FreeMoveBarrelEnemy : BaseEnemy
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

		public static readonly string ModelNode = "model";

		public TriggerData data;

		private BezierMover bezierMover = new BezierMover();

		private float moveSpeed;

		private Animation anim;

		public GameObject model;

		private RD_FreeMoveBarrelEnemy_DATA rebirthData;

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
			anim = GetComponentInChildren<Animation>();
			Transform transform = base.transform.Find(ModelNode);
			if ((bool)transform)
			{
				model = transform.gameObject;
				model.SetActive(false);
			}
		}

		public override void ResetElement()
		{
			base.ResetElement();
			bezierMover.ResetData();
			PlayAnim(anim, false);
			if ((bool)model)
			{
				model.SetActive(false);
			}
			commonState = CommonState.End;
		}

		public override void UpdateElement()
		{
			if (commonState != CommonState.Active)
			{
				return;
			}
			float disLocalZ = moveSpeed * Time.deltaTime;
			Vector3 localPosition = base.transform.localPosition;
			Vector3 targetLocPos = Vector3.zero;
			Vector3 moveLocDir = Vector3.forward;
			bool num = bezierMover.MoveForwardByZ(disLocalZ, base.transform.parent, localPosition, ref targetLocPos, ref moveLocDir);
			base.transform.localPosition = targetLocPos;
			if (data.IfFollowDir)
			{
				moveLocDir.y = 0f;
				base.transform.forward = moveLocDir;
			}
			if (num)
			{
				if ((bool)anim)
				{
					anim.Stop();
				}
				commonState = CommonState.End;
			}
		}

		public void OnCollideWithCarriage()
		{
			if (commonState == CommonState.None)
			{
				if ((bool)model)
				{
					model.SetActive(true);
				}
				if ((bool)anim)
				{
					PlayAnim(anim, true);
				}
				commonState = CommonState.Active;
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
			RD_FreeMoveBarrelEnemy_DATA rD_FreeMoveBarrelEnemy_DATA = (rebirthData = JsonUtility.FromJson<RD_FreeMoveBarrelEnemy_DATA>(rd_data as string));
			bezierMover.SetBezierData(rD_FreeMoveBarrelEnemy_DATA.bezierMoverData);
			anim.SetAnimData(rD_FreeMoveBarrelEnemy_DATA.animData, ProcessState.Pause);
			commonState = (CommonState)rD_FreeMoveBarrelEnemy_DATA.commonState;
			model.transform.SetTransData(rD_FreeMoveBarrelEnemy_DATA.modelData);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_FreeMoveBarrelEnemy_DATA
			{
				bezierMoverData = bezierMover.GetBezierData(),
				animData = anim.GetAnimData(),
				commonState = (int)commonState,
				modelData = model.transform.GetTransData()
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			anim.SetAnimData(rebirthData.animData, ProcessState.UnPause);
			rebirthData = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_FreeMoveBarrelEnemy_DATA rD_FreeMoveBarrelEnemy_DATA = (rebirthData = Bson.ToObject<RD_FreeMoveBarrelEnemy_DATA>(rd_data));
			bezierMover.SetBezierData(rD_FreeMoveBarrelEnemy_DATA.bezierMoverData);
			anim.SetAnimData(rD_FreeMoveBarrelEnemy_DATA.animData, ProcessState.Pause);
			commonState = (CommonState)rD_FreeMoveBarrelEnemy_DATA.commonState;
			model.transform.SetTransData(rD_FreeMoveBarrelEnemy_DATA.modelData);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_FreeMoveBarrelEnemy_DATA
			{
				bezierMoverData = bezierMover.GetBezierData(),
				animData = anim.GetAnimData(),
				commonState = (int)commonState,
				modelData = model.transform.GetTransData()
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			anim.SetAnimData(rebirthData.animData, ProcessState.UnPause);
			rebirthData = null;
		}
	}
}
