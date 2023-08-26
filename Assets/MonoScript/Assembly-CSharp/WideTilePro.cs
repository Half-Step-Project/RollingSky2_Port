using System;
using System.IO;
using Foundation;
using UnityEngine;

public class WideTilePro : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float Width;

		public float Height;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			Width = bytes.GetSingle(ref startIndex);
			Height = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(Width.GetBytes(), ref offset);
				memoryStream.WriteByteArray(Height.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	private BoxCollider colliderChild;

	public override float TileWidth
	{
		get
		{
			return data.Width * 0.5f + BaseTile.RecycleWidthTolerance;
		}
	}

	public override float TileHeight
	{
		get
		{
			return data.Height * 0.5f + BaseTile.RecycleHeightTolerance;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void LateInitialize()
	{
		base.LateInitialize();
		colliderChild = base.transform.Find("collider").GetComponent<BoxCollider>();
		Vector3 size = colliderChild.size;
		size.x = data.Width;
		size.z = data.Height;
		colliderChild.size = size;
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
	}

	public override string Write()
	{
		if (colliderChild == null)
		{
			colliderChild = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		data.Width = colliderChild.size.x;
		data.Height = colliderChild.size.z;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		if (colliderChild == null)
		{
			colliderChild = base.transform.Find("collider").GetComponent<BoxCollider>();
		}
		data.Width = colliderChild.size.x;
		data.Height = colliderChild.size.z;
		return StructTranslatorUtility.ToByteArray(data);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return string.Empty;
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
	}

	public override byte[] RebirthWriteByteData()
	{
		return null;
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
