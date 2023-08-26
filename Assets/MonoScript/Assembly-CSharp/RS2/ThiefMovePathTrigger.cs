using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ThiefMovePathTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public Vector3[] PathPoints;

			public Vector3[] PathSlowPoints;

			public int FillNum;

			[HideInInspector]
			public Vector3[] BezierPoints;

			[HideInInspector]
			public Vector3[] BezierSlowPoints;

			public bool IfFollowDir;

			public TreasureBelongType BelongType;

			public AnimationCurve SpeedQuickCurve;

			public AnimationCurve SpeedSlowCurve;

			public void ReadBytes(byte[] bytes)
			{
				TriggerData triggerData = (this = JsonUtility.FromJson<TriggerData>(bytes.GetString()));
			}

			public byte[] WriteBytes()
			{
				return JsonUtility.ToJson(this).GetBytes();
			}
		}

		public TriggerData data;

		[Range(0f, 1f)]
		public float DeubugPercent;

		private Vector3 beginWorldPos;

		private float cachedDebugPercent = -1f;

		private Vector3[] cachedTargetPos;

		private Color[] debugCubeColors = new Color[3]
		{
			Color.red,
			Color.green,
			Color.blue
		};

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
			beginWorldPos = base.transform.TransformPoint(data.PathPoints[0]);
			commonState = CommonState.None;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			commonState = CommonState.End;
		}

		public override void TriggerEnter(BaseRole ball)
		{
		}

		public override void CoupleTriggerEnter(BaseCouple couple, Collider collider)
		{
			if (commonState == CommonState.None)
			{
				CouplePathMoveData couplePathMoveData = GetCouplePathMoveData();
				couple.SetPathMoveData(couplePathMoveData);
				commonState = CommonState.Active;
			}
		}

		public override void UpdateElement()
		{
			if (commonState == CommonState.None)
			{
				float z = Railway.theRailway.transform.position.z;
				float z2 = beginWorldPos.z;
				if (z >= z2)
				{
					CoupleTriggerEnter(BaseCouple.theCouple, null);
					commonState = CommonState.Active;
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
			if (data.PathSlowPoints == null)
			{
				data.PathSlowPoints = new Vector3[0];
				data.BezierSlowPoints = new Vector3[0];
			}
			else if (data.PathSlowPoints.Length != 0)
			{
				worldPointsByLocal = BaseElement.GetWorldPointsByLocal(data.PathSlowPoints, base.transform);
				data.BezierSlowPoints = Bezier.GetPathByPositions(worldPointsByLocal, data.FillNum);
			}
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
			if (data.PathSlowPoints == null)
			{
				data.PathSlowPoints = new Vector3[0];
				data.BezierSlowPoints = new Vector3[0];
			}
			else if (data.PathSlowPoints.Length != 0)
			{
				worldPointsByLocal = BaseElement.GetWorldPointsByLocal(data.PathSlowPoints, base.transform);
				data.BezierSlowPoints = Bezier.GetPathByPositions(worldPointsByLocal, data.FillNum);
			}
			return StructTranslatorUtility.ToByteArray(data);
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (data.PathPoints != null && data.PathPoints.Length > 2)
			{
				BaseElement.DrawWorldPath(Bezier.GetPathByPositions(BaseElement.GetWorldPointsByLocal(data.PathPoints, base.transform)), Color.red);
				DebugDrawFollow(data.PathPoints);
			}
			if (data.PathSlowPoints != null && data.PathSlowPoints.Length > 2)
			{
				BaseElement.DrawWorldPath(Bezier.GetPathByPositions(BaseElement.GetWorldPointsByLocal(data.PathSlowPoints, base.transform)), Color.red);
				DebugDrawFollow(data.PathSlowPoints);
			}
		}

		private void DebugDrawFollow(Vector3[] pathPoints)
		{
		}

		public CouplePathMoveData GetCouplePathMoveData()
		{
			return new CouplePathMoveData(data.BezierPoints, data.BezierSlowPoints, data.SpeedQuickCurve, data.SpeedSlowCurve, data.IfFollowDir, data.BelongType, m_uuId);
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_ThiefMovePathTrigger_DATA rD_ThiefMovePathTrigger_DATA = JsonUtility.FromJson<RD_ThiefMovePathTrigger_DATA>(rd_data as string);
			commonState = (CommonState)rD_ThiefMovePathTrigger_DATA.commonState;
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_ThiefMovePathTrigger_DATA
			{
				commonState = (int)commonState
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_ThiefMovePathTrigger_DATA rD_ThiefMovePathTrigger_DATA = Bson.ToObject<RD_ThiefMovePathTrigger_DATA>(rd_data);
			commonState = (CommonState)rD_ThiefMovePathTrigger_DATA.commonState;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_ThiefMovePathTrigger_DATA
			{
				commonState = (int)commonState
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
