using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class ChangeRoleSkinTrigger : BaseTriggerBox
	{
		[Serializable]
		public struct ElementData : IReadWriteBytes
		{
			public int m_skinIndex;

			public bool m_isLerp;

			public string m_shaderBlendFieldName;

			public string m_shaderYFieldName;

			public float m_time;

			public bool m_isFinishedToOtherSkin;

			public int m_finishedSkinIndex;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				m_skinIndex = bytes.GetInt32(ref startIndex);
				m_isLerp = bytes.GetBoolean(ref startIndex);
				m_shaderBlendFieldName = bytes.GetStringWithSize(ref startIndex);
				m_shaderYFieldName = bytes.GetStringWithSize(ref startIndex);
				m_time = bytes.GetSingle(ref startIndex);
				m_isFinishedToOtherSkin = bytes.GetBoolean(ref startIndex);
				m_finishedSkinIndex = bytes.GetInt32(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(m_skinIndex.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_isLerp.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_shaderBlendFieldName.GetBytesWithSize(), ref offset);
					memoryStream.WriteByteArray(m_shaderYFieldName.GetBytesWithSize(), ref offset);
					memoryStream.WriteByteArray(m_time.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_isFinishedToOtherSkin.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_finishedSkinIndex.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public ElementData m_data;

		private bool m_isPlaying;

		private bool m_isFinished;

		private float m_currentTime;

		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			m_isPlaying = false;
			m_isFinished = false;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			m_isPlaying = false;
			m_isFinished = false;
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (BaseRole.theBall.m_roleSkinController != null)
			{
				BaseRole.theBall.ChangeRoleSkinByIndex(m_data.m_skinIndex);
				m_currentTime = 0f;
				m_isPlaying = true;
				m_isFinished = false;
			}
		}

		public override void UpdateElement()
		{
			if (!m_isPlaying || !m_data.m_isLerp || m_isFinished)
			{
				return;
			}
			m_currentTime += Time.deltaTime;
			float num = m_currentTime / m_data.m_time;
			if (num > 1f)
			{
				if (m_data.m_isFinishedToOtherSkin)
				{
					BaseRole.theBall.ChangeRoleSkinByIndex(m_data.m_finishedSkinIndex);
				}
				else
				{
					BaseRole.theBall.m_roleSkinController.LerpRangeFloatForShaderFieldName(m_data.m_skinIndex, m_data.m_shaderBlendFieldName, 1f, m_data.m_shaderYFieldName, BaseRole.theBall.transform.position.y);
				}
				m_isFinished = true;
			}
			else
			{
				BaseRole.theBall.m_roleSkinController.LerpRangeFloatForShaderFieldName(m_data.m_skinIndex, m_data.m_shaderBlendFieldName, num, m_data.m_shaderYFieldName, BaseRole.theBall.transform.position.y);
			}
		}

		public override void SetDefaultValue(object[] objs)
		{
			m_data = (ElementData)objs[0];
		}

		public override void Read(string info)
		{
			m_data = JsonUtility.FromJson<ElementData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(m_data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			m_data = StructTranslatorUtility.ToStructure<ElementData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(m_data);
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_ChangeRoleSkinTrigger_DATA
			{
				m_isPlaying = (m_isPlaying ? 1 : 0),
				m_isFinished = (m_isFinished ? 1 : 0),
				m_currentTime = m_currentTime
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_ChangeRoleSkinTrigger_DATA rD_ChangeRoleSkinTrigger_DATA = JsonUtility.FromJson<RD_ChangeRoleSkinTrigger_DATA>(rd_data as string);
			m_isPlaying = rD_ChangeRoleSkinTrigger_DATA.m_isPlaying == 1;
			m_isFinished = rD_ChangeRoleSkinTrigger_DATA.m_isFinished == 1;
			m_currentTime = rD_ChangeRoleSkinTrigger_DATA.m_currentTime;
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_ChangeRoleSkinTrigger_DATA rD_ChangeRoleSkinTrigger_DATA = Bson.ToObject<RD_ChangeRoleSkinTrigger_DATA>(rd_data);
			m_isPlaying = rD_ChangeRoleSkinTrigger_DATA.m_isPlaying == 1;
			m_isFinished = rD_ChangeRoleSkinTrigger_DATA.m_isFinished == 1;
			m_currentTime = rD_ChangeRoleSkinTrigger_DATA.m_currentTime;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_ChangeRoleSkinTrigger_DATA
			{
				m_isPlaying = (m_isPlaying ? 1 : 0),
				m_isFinished = (m_isFinished ? 1 : 0),
				m_currentTime = m_currentTime
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
