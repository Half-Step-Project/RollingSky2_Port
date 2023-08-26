using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class SwingingRopeTriggerBox : BaseTriggerBox, IBrushTrigger
	{
		[Serializable]
		public struct ElementData : IReadWriteBytes
		{
			public float m_defaultProgress;

			public float m_ropeLength;

			public float m_roleHeight;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				m_defaultProgress = bytes.GetSingle(ref startIndex);
				m_ropeLength = bytes.GetSingle(ref startIndex);
				m_roleHeight = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(m_defaultProgress.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_ropeLength.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_roleHeight.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public ElementData m_data;

		[HideInInspector]
		public GameObject m_axisObject;

		[HideInInspector]
		public Collider m_collider;

		[HideInInspector]
		public bool m_isPlaying;

		private float m_from = 90f;

		private float m_to = -90f;

		[Range(0f, 1f)]
		public float m_progress = 0.5f;

		private Vector3 m_relativePos;

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
			FindTileChindren();
			UpdateRotation(m_data.m_defaultProgress);
			m_collider.enabled = true;
			m_isPlaying = false;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			m_collider.enabled = true;
			m_isPlaying = false;
		}

		public override void UpdateElement()
		{
			base.UpdateElement();
			if (m_isPlaying && BaseRole.theBall != null && m_axisObject != null)
			{
				if (BaseRole.theBall.CurrentState == BaseRole.BallState.SwingingRope)
				{
					m_relativePos = BaseRole.theBall.transform.position - m_axisObject.transform.position;
					m_axisObject.transform.rotation = Quaternion.LookRotation(m_relativePos) * Quaternion.Euler(-90f, 0f, 0f);
				}
				else
				{
					m_isPlaying = false;
				}
			}
		}

		protected void FindTileChindren()
		{
			if (m_axisObject == null)
			{
				m_axisObject = base.gameObject.transform.Find("axisObject").gameObject;
			}
			if (m_collider == null)
			{
				m_collider = base.gameObject.transform.Find("axisObject/TriggerBox").gameObject.GetComponent<Collider>();
			}
		}

		protected void UpdateRotation(float progress)
		{
			if (m_axisObject != null)
			{
				m_axisObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(new Vector3(m_from, 0f, 0f)), Quaternion.Euler(new Vector3(m_to, 0f, 0f)), progress);
			}
		}

		public void TriggerEnter(BaseRole ball, Collider collider)
		{
			if (ball == null)
			{
				return;
			}
			if (collider == m_collider)
			{
				m_collider.enabled = false;
				m_isPlaying = true;
				if (ball != null)
				{
					SwingingRipeData swingingRipeData = new SwingingRipeData();
					swingingRipeData.m_axisPosition = m_axisObject.transform.position;
					swingingRipeData.m_radius = m_data.m_ropeLength + m_data.m_roleHeight;
					ball.ChangeToSwingingRope(swingingRipeData);
				}
			}
			else if (!GameController.IfNotDeath)
			{
				ball.CrashBall();
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
			return JsonUtility.ToJson(new RD_SwingingRopeTriggerBox_DATA
			{
				m_axisObject = m_axisObject.transform.GetTransData(),
				m_isPlaying = (m_isPlaying ? 1 : 0),
				m_colliderEnable = (m_collider.enabled ? 1 : 0)
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_SwingingRopeTriggerBox_DATA rD_SwingingRopeTriggerBox_DATA = JsonUtility.FromJson<RD_SwingingRopeTriggerBox_DATA>(rd_data as string);
			m_axisObject.transform.SetTransData(rD_SwingingRopeTriggerBox_DATA.m_axisObject);
			m_isPlaying = rD_SwingingRopeTriggerBox_DATA.m_isPlaying == 1;
			m_collider.enabled = rD_SwingingRopeTriggerBox_DATA.m_colliderEnable == 1;
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_SwingingRopeTriggerBox_DATA rD_SwingingRopeTriggerBox_DATA = Bson.ToObject<RD_SwingingRopeTriggerBox_DATA>(rd_data);
			m_axisObject.transform.SetTransData(rD_SwingingRopeTriggerBox_DATA.m_axisObject);
			m_isPlaying = rD_SwingingRopeTriggerBox_DATA.m_isPlaying == 1;
			m_collider.enabled = rD_SwingingRopeTriggerBox_DATA.m_colliderEnable == 1;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_SwingingRopeTriggerBox_DATA
			{
				m_axisObject = m_axisObject.transform.GetTransData(),
				m_isPlaying = (m_isPlaying ? 1 : 0),
				m_colliderEnable = (m_collider.enabled ? 1 : 0)
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
