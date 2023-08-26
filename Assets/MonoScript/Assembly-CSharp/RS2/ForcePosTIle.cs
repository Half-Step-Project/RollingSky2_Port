using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ForcePosTIle : BaseTile
	{
		[Serializable]
		public struct TileData : IReadWriteBytes
		{
			public float Width;

			public float Height;

			public bool IfForcePos;

			public Vector3 ForcePos;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				Width = bytes.GetSingle(ref startIndex);
				Height = bytes.GetSingle(ref startIndex);
				IfForcePos = bytes.GetBoolean(ref startIndex);
				ForcePos = bytes.GetVector3(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(Width.GetBytes(), ref offset);
					memoryStream.WriteByteArray(Height.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfForcePos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ForcePos.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public TileData data;

		private BoxCollider colliderChild;

		private bool ifCollideRole;

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

		public override void Initialize()
		{
			base.Initialize();
			ifCollideRole = false;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			ifCollideRole = false;
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

		protected override void OnCollideBall(BaseRole ball)
		{
			base.OnCollideBall(ball);
			if (data.IfForcePos)
			{
				ifCollideRole = true;
			}
		}

		public override void UpdateElement()
		{
			if (ifCollideRole)
			{
				BaseRole.theBall.transform.position = data.ForcePos;
			}
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
	}
}
