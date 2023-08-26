using System;
using System.IO;
using Foundation;
using UnityEngine;

public class HangingWinTile : BaseTile
{
	public enum WinTileState
	{
		Move,
		Hang
	}

	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float BeginDistance;

		public Vector3 DeltaPos;

		public float MoveScaler;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			DeltaPos = bytes.GetVector3(ref startIndex);
			MoveScaler = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(DeltaPos.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveScaler.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	private Vector3 TargetPos;

	private bool ifActive;

	private WinTileState currentState;

	private float timeCounter;

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
		TargetPos = StartPos;
		ifActive = false;
		currentState = WinTileState.Move;
		timeCounter = 0f;
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z - data.BeginDistance;
		PlayByPercent(GetPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		return 0f;
	}

	public override void PlayByPercent(float percent)
	{
		if (!ifActive)
		{
			return;
		}
		BaseRole theBall = BaseRole.theBall;
		if (currentState == WinTileState.Move)
		{
			timeCounter += Time.deltaTime;
			theBall.ForceMoveTo(Vector3.Lerp(theBall.transform.position, TargetPos + data.DeltaPos, timeCounter));
			if (timeCounter >= 1f)
			{
				timeCounter = 0f;
				currentState = WinTileState.Hang;
			}
		}
		else if (currentState == WinTileState.Hang)
		{
			timeCounter += Time.deltaTime;
			Vector3 position = theBall.transform.position;
			if ((int)timeCounter % 2 == 0)
			{
				position += data.MoveScaler * base.transform.up;
			}
			else
			{
				position -= data.MoveScaler * base.transform.up;
			}
			theBall.ForceMoveTo(position);
		}
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		ball.OnTileEnter(this);
		ball.ChangeToWin(ball.transform.position);
		ifActive = true;
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

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_HangingWinTile_DATA rD_HangingWinTile_DATA = JsonUtility.FromJson<RD_HangingWinTile_DATA>(rd_data as string);
		ifActive = rD_HangingWinTile_DATA.ifActive;
		currentState = rD_HangingWinTile_DATA.currentState;
		timeCounter = rD_HangingWinTile_DATA.timeCounter;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_HangingWinTile_DATA
		{
			ifActive = ifActive,
			currentState = currentState,
			timeCounter = timeCounter
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_HangingWinTile_DATA rD_HangingWinTile_DATA = Bson.ToObject<RD_HangingWinTile_DATA>(rd_data);
		ifActive = rD_HangingWinTile_DATA.ifActive;
		currentState = rD_HangingWinTile_DATA.currentState;
		timeCounter = rD_HangingWinTile_DATA.timeCounter;
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_HangingWinTile_DATA
		{
			ifActive = ifActive,
			currentState = currentState,
			timeCounter = timeCounter
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
