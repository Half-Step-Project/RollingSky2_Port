using System;
using System.IO;
using Foundation;
using UnityEngine;

public class VerticalMoveTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float BeginDistance;

		public float MoveDistance;

		public float MoveSpeed;

		public int Index;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			MoveDistance = bytes.GetSingle(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			Index = bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(Index.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	private float deltaRad;

	private float dirScaler = 1f;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		deltaRad = -(float)Math.PI / 2f;
		if (data.MoveSpeed < 0f)
		{
			dirScaler = -1f;
		}
		else
		{
			dirScaler = 1f;
		}
	}

	public override void UpdateElement()
	{
		float distance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(StartPos).z - data.BeginDistance;
		PlayByPercent(GetPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		return distance;
	}

	public override void PlayByPercent(float percent)
	{
		float num = percent * data.MoveSpeed;
		float num2 = data.MoveDistance * 0.5f + Mathf.Sin(num / (float)Math.PI + deltaRad) * data.MoveDistance * 0.5f;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = StartLocalPos.z + dirScaler * num2;
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
}
