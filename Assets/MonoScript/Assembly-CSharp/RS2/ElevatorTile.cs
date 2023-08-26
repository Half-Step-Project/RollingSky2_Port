using System;
using System.IO;
using Foundation;
using UnityEngine;
using UnityExpansion;

namespace RS2
{
	public class ElevatorTile : BaseTile
	{
		[Serializable]
		public struct TileData : IReadWriteBytes
		{
			public float MoveUpDistance;

			public float MoveDownDistance;

			public float SpeedScaler;

			public bool IfMoveUp;

			public int Index;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				MoveUpDistance = bytes.GetSingle(ref startIndex);
				MoveDownDistance = bytes.GetSingle(ref startIndex);
				SpeedScaler = bytes.GetSingle(ref startIndex);
				IfMoveUp = bytes.GetBoolean(ref startIndex);
				Index = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(MoveUpDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(MoveDownDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfMoveUp.GetBytes(), ref offset);
					memoryStream.WriteByteArray(Index.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		private const float Half_Pi = (float)Math.PI / 2f;

		public TileData data;

		private Vector3 collidePos;

		private Vector3 targetUpPos;

		private Vector3 targetDownPos;

		public override float TileWidth
		{
			get
			{
				return 1.6f;
			}
		}

		public override float TileHeight
		{
			get
			{
				return 1.6f;
			}
		}

		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		public override void SetDefaultValue(object[] objs)
		{
			data.MoveUpDistance = (float)objs[0];
			data.MoveDownDistance = (float)objs[1];
			data.SpeedScaler = (float)objs[2];
			data.Index = (int)objs[3];
		}

		public override void Initialize()
		{
			base.Initialize();
			collidePos = Vector3.zero;
			commonState = CommonState.None;
			targetUpPos = base.transform.localPosition + new Vector3(0f, data.MoveUpDistance, 0f);
			targetDownPos = base.transform.localPosition - new Vector3(0f, data.MoveDownDistance, 0f);
		}

		public override void ResetElement()
		{
			collidePos = Vector3.zero;
			commonState = CommonState.None;
			base.transform.localPosition = StartLocalPos;
		}

		public void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<ElevatorMoveEventArgs>.EventId, OnElevatorMoveEvt);
		}

		public void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<ElevatorMoveEventArgs>.EventId, OnElevatorMoveEvt);
		}

		public override void UpdateElement()
		{
			float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(collidePos).z;
			if (commonState == CommonState.None)
			{
				return;
			}
			if (commonState == CommonState.Active)
			{
				float percent = GetPercent(distance);
				PlayByPercent(percent);
				if (percent >= 1f)
				{
					commonState = CommonState.End;
				}
			}
			else
			{
				CommonState commonState2 = commonState;
				int num = 5;
			}
		}

		public override float GetPercent(float distance)
		{
			return Mathf.Min(1f, Mathf.Max(-1f, distance * data.SpeedScaler));
		}

		public override void PlayByPercent(float percent)
		{
			if (!(percent < 0f))
			{
				Vector3 b = ((!data.IfMoveUp) ? targetDownPos : targetUpPos);
				base.transform.localPosition = Vector3.Lerp(StartLocalPos, b, percent);
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TileData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<TileData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}

		private void OnElevatorMoveEvt(object sender, Foundation.EventArgs e)
		{
			ElevatorMoveEventArgs elevatorMoveEventArgs = e as ElevatorMoveEventArgs;
			if (elevatorMoveEventArgs == null || commonState != 0)
			{
				return;
			}
			if (elevatorMoveEventArgs.ValidIndex != -1)
			{
				commonState = CommonState.Active;
				data.IfMoveUp = elevatorMoveEventArgs.IfUp;
				collidePos = elevatorMoveEventArgs.BeginPos;
				return;
			}
			int num = point.m_y - elevatorMoveEventArgs.TriggerPoint.m_y;
			if (elevatorMoveEventArgs.GridTrans == base.groupTransform && num >= 0 && (float)num <= elevatorMoveEventArgs.ValidDistance)
			{
				commonState = CommonState.Active;
				data.IfMoveUp = elevatorMoveEventArgs.IfUp;
				collidePos = elevatorMoveEventArgs.BeginPos;
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_ElevatorTile_DATA rD_ElevatorTile_DATA = JsonUtility.FromJson<RD_ElevatorTile_DATA>(rd_data as string);
			commonState = rD_ElevatorTile_DATA.commonState;
			data.IfMoveUp = rD_ElevatorTile_DATA.IfMoveUp;
			collidePos = rD_ElevatorTile_DATA.collidePos.ToVector3();
			base.transform.SetTransData(rD_ElevatorTile_DATA.trans);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_ElevatorTile_DATA
			{
				commonState = commonState,
				IfMoveUp = data.IfMoveUp,
				collidePos = collidePos.ToMyVector3(),
				trans = base.transform.GetTransData()
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_ElevatorTile_DATA rD_ElevatorTile_DATA = Bson.ToObject<RD_ElevatorTile_DATA>(rd_data);
			commonState = rD_ElevatorTile_DATA.commonState;
			data.IfMoveUp = rD_ElevatorTile_DATA.IfMoveUp;
			collidePos = rD_ElevatorTile_DATA.collidePos.ToVector3();
			base.transform.SetTransData(rD_ElevatorTile_DATA.trans);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_ElevatorTile_DATA
			{
				commonState = commonState,
				IfMoveUp = data.IfMoveUp,
				collidePos = collidePos.ToMyVector3(),
				trans = base.transform.GetTransData()
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
