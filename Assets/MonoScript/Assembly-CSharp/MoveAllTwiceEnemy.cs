using System;
using System.IO;
using Foundation;
using UnityEngine;

public class MoveAllTwiceEnemy : BaseEnemy
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public EnemyDirection MoveDirection;

		public float MoveDistance;

		public float BeginDistance;

		public float SpeedScaler;

		public float SecondPercent;

		public float MoveDistance2;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveDirection = (EnemyDirection)bytes.GetInt32(ref startIndex);
			MoveDistance = bytes.GetSingle(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
			SpeedScaler = bytes.GetSingle(ref startIndex);
			SecondPercent = bytes.GetSingle(ref startIndex);
			MoveDistance2 = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(((int)MoveDirection).GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
				memoryStream.WriteByteArray(SecondPercent.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveDistance2.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	private const float Half_Pi = (float)Math.PI / 2f;

	public EnemyData data;

	private Vector3 beginPos;

	private Vector3 endPos;

	private Vector3 halfPos;

	private float halfSecondPercent;

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
		beginPos = StartLocalPos;
		data.MoveDistance = Mathf.Abs(data.MoveDistance);
		data.SecondPercent = Mathf.Abs(data.SecondPercent);
		if (data.MoveDirection == EnemyDirection.Down)
		{
			endPos = beginPos + new Vector3(0f, 0f - data.MoveDistance, 0f);
			halfPos = endPos + new Vector3(0f, 0f - data.MoveDistance2, 0f);
		}
		else if (data.MoveDirection == EnemyDirection.Up)
		{
			endPos = beginPos + new Vector3(0f, data.MoveDistance, 0f);
			halfPos = endPos + new Vector3(0f, data.MoveDistance2, 0f);
		}
		else if (data.MoveDirection == EnemyDirection.Left)
		{
			endPos = beginPos + new Vector3(0f - data.MoveDistance, 0f, 0f);
			halfPos = endPos + new Vector3(0f - data.MoveDistance2, 0f, 0f);
		}
		else if (data.MoveDirection == EnemyDirection.Right)
		{
			endPos = beginPos + new Vector3(data.MoveDistance, 0f, 0f);
			halfPos = endPos + new Vector3(data.MoveDistance2, 0f, 0f);
		}
		else if (data.MoveDirection == EnemyDirection.Forward)
		{
			endPos = beginPos + new Vector3(0f, 0f, data.MoveDistance);
			halfPos = endPos + new Vector3(0f, 0f, data.MoveDistance2);
		}
		else if (data.MoveDirection == EnemyDirection.Backward)
		{
			endPos = beginPos + new Vector3(0f, 0f, 0f - data.MoveDistance);
			halfPos = endPos + new Vector3(0f, 0f, 0f - data.MoveDistance2);
		}
		data.SpeedScaler = Mathf.Abs(data.SpeedScaler);
		if ((double)data.SecondPercent <= 0.01)
		{
			Debug.Log("Second Percent is too small" + data.SecondPercent);
			data.SecondPercent = 0.01f;
		}
		halfSecondPercent = data.SecondPercent + (1f - data.SecondPercent) / 2f;
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z + data.BeginDistance;
		PlayByPercent(GetPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Min(1f, Mathf.Max(-1f, distance * data.SpeedScaler));
	}

	public override void PlayByPercent(float percent)
	{
		if (percent < 0f)
		{
			if (percent <= data.SecondPercent - 1f)
			{
				percent = (percent - -1f) / (data.SecondPercent - 1f - -1f);
				percent *= (float)Math.PI / 2f;
				base.transform.localPosition = Vector3.Lerp(beginPos, endPos, 1f - Mathf.Sin(percent + (float)Math.PI / 2f));
			}
			else if (percent <= halfSecondPercent - 1f)
			{
				percent = (percent - (data.SecondPercent - 1f)) / (halfSecondPercent - 1f - (data.SecondPercent - 1f));
				base.transform.localPosition = Vector3.Lerp(endPos, halfPos, percent);
			}
			else
			{
				percent = (percent - (halfSecondPercent - 1f)) / (0f - (halfSecondPercent - 1f));
				base.transform.localPosition = Vector3.Lerp(halfPos, endPos, percent);
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
