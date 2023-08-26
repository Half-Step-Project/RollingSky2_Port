using System;
using System.IO;
using Foundation;
using UnityEngine;

public class HammerLREnemy : BaseEnemy
{
	public enum eLeftRight
	{
		left,
		middle,
		right
	}

	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float BeginDistance;

		public float SpeedScaler;

		public eLeftRight PosLeftRight;

		public eLeftRight DirLeftRight;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			SpeedScaler = bytes.GetSingle(ref startIndex);
			PosLeftRight = (eLeftRight)bytes.GetInt32(ref startIndex);
			DirLeftRight = (eLeftRight)bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)PosLeftRight).GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)DirLeftRight).GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	private const float Half_Pi = (float)Math.PI / 2f;

	public EnemyData data;

	private float deltaRad;

	private float dirValue = 1f;

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
		switch (data.PosLeftRight)
		{
		case eLeftRight.left:
			deltaRad = -(float)Math.PI / 2f;
			break;
		case eLeftRight.middle:
			deltaRad = 0f;
			break;
		case eLeftRight.right:
			deltaRad = (float)Math.PI / 2f;
			break;
		}
		switch (data.DirLeftRight)
		{
		case eLeftRight.left:
			dirValue = 1f;
			break;
		case eLeftRight.right:
			dirValue = -1f;
			break;
		}
		data.SpeedScaler = Mathf.Abs(data.SpeedScaler);
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z + data.BeginDistance;
		PlayByPercent(GetPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		return distance;
	}

	public override void PlayByPercent(float percent)
	{
		float num = percent * data.SpeedScaler;
		float num2 = Mathf.Sin(num / (float)Math.PI + deltaRad);
		float num3 = Mathf.Sin(num / (float)Math.PI + deltaRad + 0.01f);
		Vector3 eulerAngles = base.transform.eulerAngles;
		float num4 = 0f;
		num4 = ((!(num2 <= num3)) ? (270f - num2 * 90f) : (90f + num2 * 90f));
		eulerAngles.y = num4 * dirValue;
		base.transform.localEulerAngles = eulerAngles;
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
