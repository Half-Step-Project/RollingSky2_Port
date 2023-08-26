using System;
using System.IO;
using Foundation;
using UnityEngine;

public class TransferTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float MoveDistance;

		public bool DefaultActive;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveDistance = bytes.GetSingle(ref startIndex);
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

	private float m_speed = 5f;

	private bool isCollision;

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
		isCollision = data.DefaultActive;
		beginPos = StartLocalPos;
		endPos = beginPos + new Vector3(0f, 0f, 0f - data.MoveDistance);
	}

	public override void UpdateElement()
	{
		if (isCollision)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.z = base.transform.parent.InverseTransformPoint(BaseRole.BallPosition).z - m_speed * Time.deltaTime;
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, localPosition, 0.5f);
			if (base.transform.localPosition.z <= endPos.z)
			{
				base.transform.localPosition = endPos;
				isCollision = false;
			}
		}
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		base.OnCollideBall(ball);
		isCollision = true;
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
