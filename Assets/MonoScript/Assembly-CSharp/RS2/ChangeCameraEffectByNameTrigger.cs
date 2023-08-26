using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ChangeCameraEffectByNameTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct ShowData
		{
			public string name;

			public bool isShow;

			public void ReadBytes(byte[] bytes, ref int startIndex)
			{
				name = bytes.GetStringWithSize(ref startIndex);
				isShow = bytes.GetBoolean(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(name.GetBytesWithSize(), ref offset);
					memoryStream.WriteByteArray(isShow.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		[Serializable]
		public struct Data : IReadWriteBytes
		{
			public ShowData[] datas;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				int @int = bytes.GetInt32(ref startIndex);
				datas = new ShowData[@int];
				for (int i = 0; i < @int; i++)
				{
					datas[i].ReadBytes(bytes, ref startIndex);
				}
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					int value = datas.Length;
					memoryStream.WriteByteArray(value.GetBytes(), ref offset);
					ShowData[] array = datas;
					foreach (ShowData showData in array)
					{
						memoryStream.WriteByteArray(showData.WriteBytes(), ref offset);
					}
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public Data data;

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
			commonState = CommonState.None;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			commonState = CommonState.None;
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (commonState == CommonState.None)
			{
				Mod.Event.Fire(this, Mod.Reference.Acquire<CameraParticlePlayByNameEventArgs>().Initialize(data));
				commonState = CommonState.Active;
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<Data>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<Data>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}
	}
}
