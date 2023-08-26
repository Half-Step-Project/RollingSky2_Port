using System;
using System.IO;
using Foundation;
using UnityEngine;

public class NormalDropEnemy : BaseEnemy
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float DropDistance;

		public float DropHeight;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			DropDistance = bytes.GetSingle(ref startIndex);
			DropHeight = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(DropDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(DropHeight.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public EnemyData data;

	[Range(0f, 2f)]
	public float TestPercent;

	public float TestBaseSpeed = 7.25f;

	public float TestTime;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.DropDistance = (float)objs[0];
			data.DropHeight = (float)objs[1];
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball && !ball.IfDie && !ball.IfJumping && !ball.IfDropping)
		{
			Vector3 position = base.transform.position;
			Vector3 endPos = base.transform.position + data.DropDistance * ball.transform.forward;
			Vector3 forward = ball.transform.forward;
			ball.CallBeginDrop(position, endPos, forward, data.DropHeight);
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

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Color color = Gizmos.color;
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		Gizmos.color = color;
		Vector3 position = base.transform.position;
		Vector3 forward = base.transform.forward;
		Vector3 vector = base.transform.position;
		int num = Mathf.Max(3, Mathf.CeilToInt(data.DropDistance * 2.5f));
		float num2 = 0.5f;
		Vector3[] array = new Vector3[num + 1];
		array[0] = vector;
		color = Gizmos.color;
		Gizmos.color = Color.yellow;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector2 = vector + forward * num2;
			float num3 = 0f - Vector3.Dot(position - vector2, forward);
			float y = -1f * (num3 * num3) * data.DropHeight / (data.DropDistance * data.DropDistance);
			array[i + 1] = vector2 + new Vector3(0f, y, 0f);
			Gizmos.DrawLine(array[i], array[i + 1]);
			vector = vector2;
		}
		Gizmos.color = color;
		float num4 = data.DropDistance * TestPercent;
		float num5 = -1f * (num4 * num4) * data.DropHeight / (data.DropDistance * data.DropDistance);
		Vector3 center = position;
		center.z += num4;
		center.y += num5;
		Gizmos.DrawCube(center, Vector3.one);
		TestTime = num4 / TestBaseSpeed;
		int num6 = Mathf.FloorToInt(TestTime);
		TestTime = (float)num6 + (TestTime - (float)num6) * 0.6f;
	}
}
