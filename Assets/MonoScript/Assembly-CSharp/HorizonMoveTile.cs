using System;
using System.IO;
using Foundation;
using UnityEngine;

public class HorizonMoveTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float BeginDistance;

		public float MoveSpeed;

		public int Index;

		public bool IfRoot;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			Index = bytes.GetInt32(ref startIndex);
			IfRoot = bytes.GetBoolean(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(Index.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfRoot.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public static readonly int GRID_WIDTH = 21;

	public static readonly float HALF_GRID_WIDTH = (float)GRID_WIDTH / 2f;

	public static readonly int ROAD_SIZE = 5;

	public static readonly int HALF_ROAD_SIZE = ROAD_SIZE / 2;

	public TileData data;

	private float deltaRad;

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
		deltaRad = 0f;
		switch (data.Index)
		{
		case 0:
			deltaRad = -(float)Math.PI / 2f;
			break;
		case 1:
			deltaRad = -(float)Math.PI / 6f;
			break;
		case 2:
			deltaRad = 0f;
			break;
		case 3:
			deltaRad = (float)Math.PI / 6f;
			break;
		case 4:
			deltaRad = (float)Math.PI / 2f;
			break;
		}
	}

	public override void UpdateElement()
	{
		if (data.IfRoot)
		{
			float distance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z - data.BeginDistance;
			PlayByPercent(GetPercent(distance));
		}
	}

	public override float GetPercent(float distance)
	{
		return distance;
	}

	public override void PlayByPercent(float percent)
	{
		float num = Mathf.Sin(percent * data.MoveSpeed / (float)Math.PI + deltaRad) * 2f;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = HALF_GRID_WIDTH + num;
		base.transform.localPosition = localPosition;
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
		RD_HorizonMoveTile_DATA rD_HorizonMoveTile_DATA = JsonUtility.FromJson<RD_HorizonMoveTile_DATA>(rd_data as string);
		base.transform.SetTransData(rD_HorizonMoveTile_DATA.transform);
		if (!data.IfRoot)
		{
			base.transform.localPosition = rD_HorizonMoveTile_DATA.transform.position;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_HorizonMoveTile_DATA rD_HorizonMoveTile_DATA = new RD_HorizonMoveTile_DATA();
		rD_HorizonMoveTile_DATA.transform = base.transform.GetTransData();
		if (!data.IfRoot)
		{
			rD_HorizonMoveTile_DATA.transform.position = base.transform.localPosition;
		}
		return JsonUtility.ToJson(rD_HorizonMoveTile_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_HorizonMoveTile_DATA rD_HorizonMoveTile_DATA = Bson.ToObject<RD_HorizonMoveTile_DATA>(rd_data);
		base.transform.SetTransData(rD_HorizonMoveTile_DATA.transform);
		if (!data.IfRoot)
		{
			base.transform.localPosition = rD_HorizonMoveTile_DATA.transform.position;
		}
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_HorizonMoveTile_DATA rD_HorizonMoveTile_DATA = new RD_HorizonMoveTile_DATA();
		rD_HorizonMoveTile_DATA.transform = base.transform.GetTransData();
		if (!data.IfRoot)
		{
			rD_HorizonMoveTile_DATA.transform.position = base.transform.localPosition;
		}
		return Bson.ToBson(rD_HorizonMoveTile_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
