using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class RoleLineTrigger : BaseEnemy
	{
		[Serializable]
		public struct EnemyData : IReadWriteBytes
		{
			public float AlphaDistance;

			public float MaxAlpha;

			public Vector3 Position;

			public Quaternion Rotation;

			public Vector3 Scale;

			public Vector3 Center;

			public Vector3 Size;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				AlphaDistance = bytes.GetSingle(ref startIndex);
				MaxAlpha = bytes.GetSingle(ref startIndex);
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
					memoryStream.WriteByteArray(AlphaDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(MaxAlpha.GetBytes(), ref offset);
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

		public EnemyData data;

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

		public override void ResetElement()
		{
			base.ResetElement();
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<EnemyData>(info);
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
			data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
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

		public override void TriggerEnter(BaseRole ball)
		{
			TempLineAlphaEventArgs tempLineAlphaEventArgs = Mod.Reference.Acquire<TempLineAlphaEventArgs>();
			tempLineAlphaEventArgs.Initialize(data.AlphaDistance, data.MaxAlpha);
			Mod.Event.Fire(this, tempLineAlphaEventArgs);
		}

		public override void OnDrawGizmos()
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawLine(to: base.transform.position + new Vector3(0f, 0f, data.AlphaDistance), from: base.transform.position);
			Gizmos.color = color;
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

		public override void RebirthReadByteData(byte[] rd_data)
		{
		}

		public override byte[] RebirthWriteByteData()
		{
			return new byte[0];
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}
	}
}
