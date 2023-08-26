using System;
using System.IO;
using Foundation;
using UnityEngine;

public class TransferForwardTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public Vector3 MoveDistance;

		public bool DefaultActive;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveDistance = bytes.GetVector3(ref startIndex);
			DefaultActive = bytes.GetBoolean(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(DefaultActive.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	private Vector3 beginPos;

	private Vector3 endPos;

	private bool ifActive;

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
		ifActive = data.DefaultActive;
		beginPos = StartLocalPos;
		data.MoveDistance.z = Mathf.Abs(data.MoveDistance.z);
		endPos = beginPos + data.MoveDistance;
	}

	public override void UpdateElement()
	{
		float distance = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - beginPos.z;
		PlayByPercent(GetPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		return distance;
	}

	public override void PlayByPercent(float percent)
	{
		if (ifActive)
		{
			if (percent < 0f)
			{
				base.transform.localPosition = beginPos;
			}
			else if (percent > Mathf.Abs(data.MoveDistance.z))
			{
				base.transform.localPosition = endPos;
			}
			else
			{
				base.transform.localPosition = beginPos + new Vector3(0f, data.MoveDistance.y / data.MoveDistance.z * percent, percent);
			}
		}
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		base.OnCollideBall(ball);
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
}
