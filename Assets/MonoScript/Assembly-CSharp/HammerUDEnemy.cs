using System;
using System.IO;
using Foundation;
using UnityEngine;

public class HammerUDEnemy : BaseEnemy
{
	public enum eInOut
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

		public eInOut Inout;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			SpeedScaler = bytes.GetSingle(ref startIndex);
			Inout = (eInOut)bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(SpeedScaler.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)Inout).GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	private const float Half_Pi = (float)Math.PI / 2f;

	public EnemyData data;

	private float deltaRad;

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
		switch (data.Inout)
		{
		case eInOut.left:
			deltaRad = -(float)Math.PI / 2f;
			break;
		case eInOut.middle:
			deltaRad = 0f;
			break;
		case eInOut.right:
			deltaRad = (float)Math.PI / 2f;
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
		float num = Mathf.Sin(percent * data.SpeedScaler / (float)Math.PI + deltaRad);
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		float num2 = (localEulerAngles.z = 90f + num * 90f);
		base.transform.localEulerAngles = localEulerAngles;
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
