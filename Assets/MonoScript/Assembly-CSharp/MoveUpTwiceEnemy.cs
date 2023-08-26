using System;
using System.IO;
using Foundation;
using UnityEngine;

public class MoveUpTwiceEnemy : BaseEnemy
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float MoveDistance1;

		public float MoveDistance2;

		public float WaitDistance;

		public float BeginDistance;

		public float SpeedScaler;

		public EnemyDirection MoveDirection;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveDistance1 = bytes.GetSingle(ref startIndex);
			MoveDistance2 = bytes.GetSingle(ref startIndex);
			WaitDistance = bytes.GetSingle(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
			SpeedScaler = bytes.GetSingle(ref startIndex);
			MoveDirection = (EnemyDirection)bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(MoveDistance1.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveDistance2.GetBytes(), ref offset);
				memoryStream.WriteByteArray(WaitDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)MoveDirection).GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	private const float Half_Pi = (float)Math.PI / 2f;

	public EnemyData data;

	private float totalLength;

	private Vector3 beginPos;

	private Vector3 midPos;

	private Vector3 endPos;

	private float movePercent;

	private float waitPercent;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z + data.BeginDistance;
		PlayByPercent(GetPercent(distance));
	}

	public override void Initialize()
	{
		base.Initialize();
		beginPos = StartLocalPos;
		if (data.MoveDirection == EnemyDirection.Down)
		{
			data.MoveDistance1 = 0f - Mathf.Abs(data.MoveDistance1);
			data.MoveDistance2 = 0f - Mathf.Abs(data.MoveDistance2);
		}
		else if (data.MoveDirection == EnemyDirection.Up)
		{
			data.MoveDistance1 = Mathf.Abs(data.MoveDistance1);
			data.MoveDistance2 = Mathf.Abs(data.MoveDistance2);
		}
		midPos = beginPos + new Vector3(0f, data.MoveDistance1, 0f);
		endPos = midPos + new Vector3(0f, data.MoveDistance2, 0f);
		data.SpeedScaler = Mathf.Abs(data.SpeedScaler);
		totalLength = data.MoveDistance1 + data.MoveDistance2 + data.WaitDistance;
		movePercent = data.MoveDistance1 / totalLength;
		waitPercent = (data.MoveDistance1 + data.WaitDistance) / totalLength;
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Min(1f, Mathf.Max(-1f, distance * data.SpeedScaler));
	}

	public override void PlayByPercent(float percent)
	{
		if (percent < 0f)
		{
			if (percent <= movePercent - 1f)
			{
				percent = (percent - -1f) / (movePercent - 1f - -1f);
				percent *= (float)Math.PI / 2f;
				base.transform.localPosition = Vector3.Lerp(beginPos, midPos, 1f - Mathf.Sin(percent + (float)Math.PI / 2f));
			}
			else if (percent <= waitPercent - 1f)
			{
				base.transform.localPosition = midPos;
			}
			else
			{
				percent = (percent - (waitPercent - 1f)) / (0f - (waitPercent - 1f));
				percent *= (float)Math.PI / 2f;
				base.transform.localPosition = Vector3.Lerp(midPos, endPos, 1f - Mathf.Sin(percent + (float)Math.PI / 2f));
			}
		}
		else
		{
			base.transform.localPosition = endPos;
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<EnemyData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}
}
