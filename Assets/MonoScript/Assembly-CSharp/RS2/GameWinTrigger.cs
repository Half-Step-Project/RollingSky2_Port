using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class GameWinTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public bool IfForcePos;

			public Vector3 ForcePos;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				IfForcePos = bytes.GetBoolean(ref startIndex);
				ForcePos = bytes.GetVector3(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(IfForcePos.GetBytes(), ref offset);
					memoryStream.WriteByteArray(ForcePos.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public TriggerData data;

		private bool ifCollide;

		public override void Initialize()
		{
			base.Initialize();
			ifCollide = false;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			ifCollide = false;
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (!ifCollide)
			{
				ifCollide = true;
				Vector3 pos = (data.IfForcePos ? data.ForcePos : ball.transform.position);
				ball.ChangeToWin(pos);
			}
		}

		public override void UpdateElement()
		{
			if (data.IfForcePos && ifCollide)
			{
				BaseRole.theBall.transform.position = data.ForcePos;
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<TriggerData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			if (bytes != null)
			{
				data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
			}
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}
	}
}
