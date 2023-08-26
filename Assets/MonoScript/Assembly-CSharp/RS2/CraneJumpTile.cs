using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CraneJumpTile : CraneTile
	{
		[Serializable]
		public struct JumpData : IReadWriteBytes
		{
			public float JumpDistance;

			public float JumpHeight;

			public bool IfShowTrail;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ReadBytes(bytes, ref startIndex);
			}

			public void ReadBytes(byte[] bytes, ref int startIndex)
			{
				JumpDistance = bytes.GetSingle(ref startIndex);
				JumpHeight = bytes.GetSingle(ref startIndex);
				IfShowTrail = bytes.GetBoolean(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
					memoryStream.WriteByteArray(IfShowTrail.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		[Header("跳到的数据:")]
		public JumpData m_jumpData;

		[Header("测试专用预览:")]
		[Range(0f, 2f)]
		public float TestPercent;

		public float TestBaseSpeed = 7.25f;

		public float TestTime;

		private GameObject m_jumpObject;

		private GameObject m_jumpModelObject;

		private ParticleSystem m_jumpEffect;

		private Collider m_jumpCollider;

		private AudioSource m_jumpAudio;

		private RD_CraneJumpTile_DATA m_jumpRebirthData;

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
			m_jumpObject.gameObject.SetActive(false);
		}

		public override void ResetElement()
		{
			base.ResetElement();
		}

		public override string Write()
		{
			return base.Write() + "&" + JsonUtility.ToJson(m_jumpData);
		}

		public override void Read(string info)
		{
			string[] array = info.Split('&');
			m_data = JsonUtility.FromJson<ElementData>(array[0]);
			m_jumpData = JsonUtility.FromJson<JumpData>(array[1]);
		}

		public override void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			m_data = default(ElementData);
			m_data.ReadBytes(bytes, ref startIndex);
			m_jumpData = default(JumpData);
			m_jumpData.ReadBytes(bytes, ref startIndex);
		}

		public override byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(m_data.WriteBytes(), ref offset);
				memoryStream.WriteByteArray(m_jumpData.WriteBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}

		public override void SetDefaultValue(object[] objs)
		{
			base.SetDefaultValue(objs);
			m_jumpData = (JumpData)objs[1];
		}

		public override void CoupleTriggerEnter(BaseCouple couple, Collider collider)
		{
			base.CoupleTriggerEnter(couple, collider);
			if (collider.gameObject.name == m_colliderTriggerBoxInfo && m_isTriggerControllerA)
			{
				m_jumpObject.SetActive(true);
			}
		}

		public override void TriggerEnter(BaseRole ball, Collider collider)
		{
			base.TriggerEnter(ball, collider);
			if (collider.gameObject.name == m_colliderTriggerBoxInfo && m_isTriggerControllerA)
			{
				m_jumpObject.SetActive(true);
			}
			if (!(m_jumpCollider != null) || !(collider == m_jumpCollider) || !ball)
			{
				return;
			}
			if (ball.IfJumpingDown)
			{
				ball.CallEndJump(m_jumpObject.transform.position.y, true);
			}
			if (!ball.IfJumping)
			{
				ball.CallBeginJump(m_jumpObject.transform.position, m_jumpObject.transform.position + m_jumpData.JumpDistance * ball.transform.forward, ball.transform.forward, m_jumpData.JumpHeight, BaseRole.JumpType.Super, m_jumpData.IfShowTrail);
				if (m_jumpAudio != null && GameController.Instance.GetPlayerDataModule.IsSoundPlayOn())
				{
					m_jumpAudio.Play();
				}
				if (m_jumpEffect != null)
				{
					m_jumpEffect.Play();
				}
				if (m_jumpModelObject != null)
				{
					m_jumpModelObject.SetActive(false);
				}
			}
			if (ball.IfDropping)
			{
				Log.Error("Don't use jumpDistance Tile with DropTile");
			}
		}

		protected override void FindTileChindren()
		{
			base.FindTileChindren();
			if (m_dics != null)
			{
				if (m_jumpObject == null)
				{
					m_jumpObject = m_dics["jumpObject"];
				}
				if (m_jumpModelObject == null)
				{
					m_jumpModelObject = m_dics["jumpModelObject"];
				}
				if (m_jumpEffect == null)
				{
					GameObject gameObject = m_dics["jumpEffect"];
					m_jumpEffect = gameObject.GetComponentInChildren<ParticleSystem>();
				}
				if (m_jumpCollider == null)
				{
					GameObject gameObject2 = m_dics["jumpCollider"];
					m_jumpCollider = gameObject2.GetComponent<Collider>();
				}
				if (m_jumpAudio == null)
				{
					GameObject gameObject3 = m_dics["jumpAudio"];
					m_jumpAudio = gameObject3.GetComponent<AudioSource>();
				}
			}
		}

		protected override void RecordingObjectsData()
		{
			base.RecordingObjectsData();
			if (m_jumpEffect != null)
			{
				m_jumpEffect.Stop();
			}
			if (m_jumpModelObject != null)
			{
				m_jumpModelObject.SetActive(true);
			}
		}

		protected override void RefreshObjectsData()
		{
			base.RefreshObjectsData();
			if (m_jumpEffect != null)
			{
				m_jumpEffect.Stop();
			}
			if (m_jumpModelObject != null)
			{
				m_jumpModelObject.SetActive(true);
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			RD_CraneJumpTile_DATA rD_CraneJumpTile_DATA = new RD_CraneJumpTile_DATA();
			rD_CraneJumpTile_DATA.m_jumpObject = m_jumpObject.transform.GetTransData();
			rD_CraneJumpTile_DATA.m_jumpModelObject = m_jumpModelObject.transform.GetTransData();
			rD_CraneJumpTile_DATA.m_jumpEffect = m_jumpEffect.GetParticleData();
			rD_CraneJumpTile_DATA.m_colliderTriggerBoxObject = m_colliderTriggerBoxObject.transform.GetTransData();
			rD_CraneJumpTile_DATA.m_rebirthData = (string)base.RebirthWriteData();
			if (colider != null)
			{
				rD_CraneJumpTile_DATA.colider = colider.transform.GetTransData();
			}
			return JsonUtility.ToJson(rD_CraneJumpTile_DATA);
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			m_jumpRebirthData = JsonUtility.FromJson<RD_CraneJumpTile_DATA>(rd_data as string);
			base.RebirthReadData((object)m_jumpRebirthData.m_rebirthData);
			m_jumpObject.transform.SetTransData(m_jumpRebirthData.m_jumpObject);
			m_jumpModelObject.transform.SetTransData(m_jumpRebirthData.m_jumpModelObject);
			m_jumpEffect.SetParticleData(m_jumpRebirthData.m_jumpEffect, ProcessState.Pause);
			m_colliderTriggerBoxObject.transform.SetTransData(m_jumpRebirthData.m_colliderTriggerBoxObject);
			if (colider != null)
			{
				colider.transform.SetTransData(m_jumpRebirthData.colider);
			}
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			if (m_jumpRebirthData != null)
			{
				base.RebirthStartGame((object)m_jumpRebirthData.m_rebirthData);
				m_jumpEffect.SetParticleData(m_jumpRebirthData.m_jumpEffect, ProcessState.UnPause);
				m_jumpRebirthData = null;
			}
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			m_jumpRebirthData = Bson.ToObject<RD_CraneJumpTile_DATA>(rd_data);
			base.RebirthReadByteData(m_jumpRebirthData.m_rebirthBytesData);
			m_jumpObject.transform.SetTransData(m_jumpRebirthData.m_jumpObject);
			m_jumpModelObject.transform.SetTransData(m_jumpRebirthData.m_jumpModelObject);
			m_jumpEffect.SetParticleData(m_jumpRebirthData.m_jumpEffect, ProcessState.Pause);
			m_colliderTriggerBoxObject.transform.SetTransData(m_jumpRebirthData.m_colliderTriggerBoxObject);
			if (colider != null)
			{
				colider.transform.SetTransData(m_jumpRebirthData.colider);
			}
		}

		public override byte[] RebirthWriteByteData()
		{
			RD_CraneJumpTile_DATA rD_CraneJumpTile_DATA = new RD_CraneJumpTile_DATA();
			rD_CraneJumpTile_DATA.m_jumpObject = m_jumpObject.transform.GetTransData();
			rD_CraneJumpTile_DATA.m_jumpModelObject = m_jumpModelObject.transform.GetTransData();
			rD_CraneJumpTile_DATA.m_jumpEffect = m_jumpEffect.GetParticleData();
			rD_CraneJumpTile_DATA.m_colliderTriggerBoxObject = m_colliderTriggerBoxObject.transform.GetTransData();
			rD_CraneJumpTile_DATA.m_rebirthBytesData = base.RebirthWriteByteData();
			if (colider != null)
			{
				rD_CraneJumpTile_DATA.colider = colider.transform.GetTransData();
			}
			return Bson.ToBson(rD_CraneJumpTile_DATA);
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			if (m_jumpRebirthData != null)
			{
				base.RebirthReadByteData(m_jumpRebirthData.m_rebirthBytesData);
				m_jumpEffect.SetParticleData(m_jumpRebirthData.m_jumpEffect, ProcessState.UnPause);
				m_jumpRebirthData = null;
			}
		}
	}
}
