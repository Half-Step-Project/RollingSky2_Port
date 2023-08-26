using System;
using System.IO;
using Foundation;
using UnityEngine;

public class DropDieTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public Vector3 Position;

		public Quaternion Rotation;

		public Vector3 Scale;

		public Vector3 Center;

		public Vector3 Size;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			Position = bytes.GetVector3(ref startIndex);
			Rotation = bytes.GetQuaternion(ref startIndex);
			Scale = bytes.GetVector3(ref startIndex);
			Center = bytes.GetVector3(ref startIndex);
			Size = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(Position.GetBytes(), ref offset);
				memoryStream.WriteByteArray(Rotation.GetBytes(), ref offset);
				memoryStream.WriteByteArray(Scale.GetBytes(), ref offset);
				memoryStream.WriteByteArray(Center.GetBytes(), ref offset);
				memoryStream.WriteByteArray(Size.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TriggerData data;

	private BoxCollider colliderChild;

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
		colliderChild = base.transform.Find("collider").GetComponent<BoxCollider>();
		colliderChild.transform.localPosition = data.Position;
		colliderChild.transform.localRotation = data.Rotation;
		colliderChild.transform.localScale = data.Scale;
		colliderChild.center = data.Center;
		colliderChild.size = data.Size;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		ball.BeginDropDie();
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TriggerData>(info);
		if (colliderChild == null)
		{
			colliderChild = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		colliderChild.transform.localPosition = data.Position;
		colliderChild.transform.localRotation = data.Rotation;
		colliderChild.transform.localScale = data.Scale;
		colliderChild.center = data.Center;
		colliderChild.size = data.Size;
	}

	public override string Write()
	{
		if (colliderChild == null)
		{
			colliderChild = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		data.Position = colliderChild.transform.localPosition;
		data.Rotation = colliderChild.transform.localRotation;
		data.Scale = colliderChild.transform.localScale;
		data.Center = colliderChild.center;
		data.Size = colliderChild.size;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		if (colliderChild == null)
		{
			colliderChild = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		colliderChild.transform.localPosition = data.Position;
		colliderChild.transform.localRotation = data.Rotation;
		colliderChild.transform.localScale = data.Scale;
		colliderChild.center = data.Center;
		colliderChild.size = data.Size;
	}

	public override byte[] WriteBytes()
	{
		if (colliderChild == null)
		{
			colliderChild = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		data.Position = colliderChild.transform.localPosition;
		data.Rotation = colliderChild.transform.localRotation;
		data.Scale = colliderChild.transform.localScale;
		data.Center = colliderChild.center;
		data.Size = colliderChild.size;
		return StructTranslatorUtility.ToByteArray(data);
	}
}
