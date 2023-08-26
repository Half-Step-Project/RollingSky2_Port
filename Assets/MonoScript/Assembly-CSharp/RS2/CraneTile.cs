using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class CraneTile : BaseTile, IBrushTrigger, IRebirth
	{
		public enum State
		{
			Wait,
			Forward,
			ForwardFinished,
			Back,
			BackFinished
		}

		[Serializable]
		public struct ElementData : IReadWriteBytes
		{
			[Header("底座的旋转:")]
			public bool m_rotate;

			public Vector3 m_rotateFrom;

			public Vector3 m_rotateTo;

			public EaseUtility.EaseType m_rotateEaseType;

			[Header("吊车上下:")]
			public bool m_move;

			public Vector3 m_moveFrom;

			public Vector3 m_moveTo;

			public EaseUtility.EaseType m_moveEaseType;

			[Header("持续时间:")]
			public float m_duration;

			[HideInInspector]
			public Vector3 m_triggerBoxLocalPosition;

			[HideInInspector]
			public Quaternion m_triggerBoxLocalRotation;

			[HideInInspector]
			public Vector3 m_triggerBoxLocalScale;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				ReadBytes(bytes, ref startIndex);
			}

			public void ReadBytes(byte[] bytes, ref int startIndex)
			{
				m_rotate = bytes.GetBoolean(ref startIndex);
				m_rotateFrom = bytes.GetVector3(ref startIndex);
				m_rotateTo = bytes.GetVector3(ref startIndex);
				m_rotateEaseType = (EaseUtility.EaseType)bytes.GetInt32(ref startIndex);
				m_move = bytes.GetBoolean(ref startIndex);
				m_moveFrom = bytes.GetVector3(ref startIndex);
				m_moveTo = bytes.GetVector3(ref startIndex);
				m_moveEaseType = (EaseUtility.EaseType)bytes.GetInt32(ref startIndex);
				m_duration = bytes.GetSingle(ref startIndex);
				m_triggerBoxLocalPosition = bytes.GetVector3(ref startIndex);
				m_triggerBoxLocalRotation = bytes.GetQuaternion(ref startIndex);
				m_triggerBoxLocalScale = bytes.GetVector3(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(m_rotate.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_rotateFrom.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_rotateTo.GetBytes(), ref offset);
					memoryStream.WriteByteArray(((int)m_rotateEaseType).GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_move.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_moveFrom.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_moveTo.GetBytes(), ref offset);
					memoryStream.WriteByteArray(((int)m_moveEaseType).GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_duration.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_triggerBoxLocalPosition.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_triggerBoxLocalRotation.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_triggerBoxLocalScale.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public struct RebirthData
		{
			public State m_currentState;

			public float m_currentTime;

			public bool m_isTriggerControllerA;
		}

		public ElementData m_data;

		public State m_state;

		public float m_currentTime;

		private GameObject m_baseCenterObject;

		private GameObject m_ropeObject;

		private GameObject m_boardObject;

		private BoxCollider m_boardCollider;

		protected GameObject m_colliderTriggerBoxObject;

		private Animation m_triggerAnimation;

		protected Dictionary<string, GameObject> m_dics;

		private bool m_isFinished;

		private Vector3 m_vector3_01 = Vector3.zero;

		private Vector3 m_baseCenterLocalPosition;

		private Vector3 m_ropeLocalPosition;

		private Vector3 m_boardLocalPosition;

		private Vector3 m_ropeLocalScale;

		private Quaternion m_baseCenterLocalRotation;

		private Quaternion m_ropeLocalRotation;

		private Quaternion m_boardLocalRotation;

		private float m_p;

		private float m_distanceForRopeOrBoard;

		protected bool m_isTriggerControllerA;

		private ParticleSystem m_A;

		private ParticleSystem m_B;

		private bool m_singleTrigger = true;

		protected readonly string m_colliderDieInfo = "ColliderDie";

		protected readonly string m_colliderTriggerBoxInfo = "ColliderTriggerBox";

		protected RD_CraneTile_DATA m_rebirthData;

		public override float TileWidth
		{
			get
			{
				return 1.25f + BaseTile.RecycleHeightTolerance;
			}
		}

		public override float TileHeight
		{
			get
			{
				return 1.25f + BaseTile.RecycleHeightTolerance;
			}
		}

		public override Vector3 TileCenter
		{
			get
			{
				if (m_boardObject != null && m_boardCollider != null)
				{
					return m_boardCollider.transform.position;
				}
				return base.TileCenter;
			}
		}

		public override float RealPosY
		{
			get
			{
				if (m_boardObject != null && m_boardCollider != null)
				{
					return m_boardCollider.transform.position.y + m_boardCollider.center.y + m_boardCollider.size.y / 2f;
				}
				return base.transform.position.y;
			}
		}

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
			RecordingObjectsData();
			RefreshColliderData();
			m_currentTime = 0f;
			m_isFinished = false;
			m_state = State.Wait;
			m_isTriggerControllerA = false;
			SetTriggerController(true, true, true);
			if ((bool)m_B)
			{
				m_B.Stop();
				m_B.gameObject.SetActive(false);
			}
			UpdateTween(0f);
			m_singleTrigger = true;
		}

		public override void UpdateElement()
		{
			base.UpdateElement();
			if (m_state == State.Forward)
			{
				m_currentTime += Time.deltaTime;
			}
			if (m_state == State.Back)
			{
				m_currentTime -= Time.deltaTime;
			}
			if (m_state == State.Forward || m_state == State.Back)
			{
				m_isFinished = UpdateTween(m_currentTime);
				if (m_isFinished && m_state == State.Forward)
				{
					m_state = State.ForwardFinished;
				}
				if (m_isFinished && m_state == State.Back)
				{
					m_state = State.BackFinished;
				}
			}
		}

		private bool UpdateTween(float time)
		{
			m_p = time / m_data.m_duration;
			if (m_p >= 0f && m_p <= 1f)
			{
				if (m_data.m_rotate && m_baseCenterObject != null)
				{
					m_vector3_01.x = EaseUtility.SwitchEaseType(m_data.m_rotateEaseType, m_p, m_data.m_rotateFrom.x, m_data.m_rotateTo.x, 1f);
					m_vector3_01.y = EaseUtility.SwitchEaseType(m_data.m_rotateEaseType, m_p, m_data.m_rotateFrom.y, m_data.m_rotateTo.y, 1f);
					m_vector3_01.z = EaseUtility.SwitchEaseType(m_data.m_rotateEaseType, m_p, m_data.m_rotateFrom.z, m_data.m_rotateTo.z, 1f);
					m_baseCenterObject.transform.localEulerAngles = m_vector3_01;
				}
				if (m_data.m_move && m_boardObject != null)
				{
					m_vector3_01.x = EaseUtility.SwitchEaseType(m_data.m_moveEaseType, m_p, m_data.m_moveFrom.x, m_data.m_moveTo.x, 1f);
					m_vector3_01.y = EaseUtility.SwitchEaseType(m_data.m_moveEaseType, m_p, m_data.m_moveFrom.y, m_data.m_moveTo.y, 1f);
					m_vector3_01.z = EaseUtility.SwitchEaseType(m_data.m_moveEaseType, m_p, m_data.m_moveFrom.z, m_data.m_moveTo.z, 1f);
					m_boardObject.transform.localPosition = m_vector3_01;
					if (m_ropeObject != null)
					{
						m_distanceForRopeOrBoard = Vector3.Distance(m_boardObject.transform.position, m_ropeObject.transform.position);
						m_vector3_01.Set(1f, 1f, 1f);
						m_vector3_01.y = m_distanceForRopeOrBoard;
						m_ropeObject.transform.localScale = m_vector3_01;
					}
				}
				return false;
			}
			if (m_p <= 0f)
			{
				m_baseCenterObject.transform.localEulerAngles = m_data.m_rotateFrom;
				m_ropeObject.transform.localScale = m_data.m_moveFrom;
			}
			if (m_p >= 1f)
			{
				m_baseCenterObject.transform.localEulerAngles = m_data.m_rotateTo;
				m_ropeObject.transform.localScale = m_data.m_moveTo;
			}
			if (m_ropeObject != null)
			{
				m_distanceForRopeOrBoard = Vector3.Distance(m_boardObject.transform.position, m_ropeObject.transform.position);
				m_vector3_01.Set(1f, 1f, 1f);
				m_vector3_01.y = m_distanceForRopeOrBoard;
				m_ropeObject.transform.localScale = m_vector3_01;
			}
			return true;
		}

		public override void ResetElement()
		{
			base.ResetElement();
			m_currentTime = 0f;
			m_state = State.Wait;
			m_isTriggerControllerA = false;
			RefreshObjectsData();
			m_singleTrigger = true;
			if ((bool)m_B)
			{
				m_B.Stop();
				m_B.gameObject.SetActive(false);
			}
		}

		protected virtual void FindTileChindren()
		{
			if (m_dics == null)
			{
				m_dics = ViewTools.CollectAllGameObjects(base.gameObject);
			}
			if (m_baseCenterObject == null)
			{
				m_dics.TryGetValue("Theif_DiaoChe_001", out m_baseCenterObject);
			}
			if (m_ropeObject == null)
			{
				m_dics.TryGetValue("ShengZi", out m_ropeObject);
			}
			if (m_boardObject == null)
			{
				m_dics.TryGetValue("MuBan", out m_boardObject);
			}
			if (m_boardCollider == null && m_boardObject != null)
			{
				m_boardCollider = m_boardObject.transform.Find("ColliderGround").GetComponent<BoxCollider>();
			}
			if (m_colliderTriggerBoxObject == null)
			{
				m_dics.TryGetValue("ColliderTriggerBox", out m_colliderTriggerBoxObject);
			}
			if (m_triggerAnimation == null)
			{
				GameObject value = null;
				m_dics.TryGetValue("Thief_DiaoCheKaiGuan", out value);
				if (value != null)
				{
					m_triggerAnimation = value.GetComponent<Animation>();
				}
			}
			if (m_A == null)
			{
				GameObject value2 = null;
				m_dics.TryGetValue("glow_054", out value2);
				if (value2 != null)
				{
					m_A = value2.GetComponent<ParticleSystem>();
				}
			}
			if (m_B == null)
			{
				GameObject value3 = null;
				m_dics.TryGetValue("glow_050", out value3);
				if (value3 != null)
				{
					m_B = value3.GetComponent<ParticleSystem>();
				}
			}
		}

		protected virtual void RecordingObjectsData()
		{
			if (m_baseCenterObject != null)
			{
				m_baseCenterLocalPosition = m_baseCenterObject.transform.localPosition;
				m_baseCenterLocalRotation = m_baseCenterObject.transform.localRotation;
			}
			if (m_ropeObject != null)
			{
				m_ropeLocalPosition = m_ropeObject.transform.localPosition;
				m_ropeLocalRotation = m_ropeObject.transform.localRotation;
				m_ropeLocalScale = m_ropeObject.transform.localScale;
			}
			if (m_boardObject != null)
			{
				m_boardLocalPosition = m_boardObject.transform.localPosition;
				m_boardLocalRotation = m_boardObject.transform.localRotation;
			}
		}

		protected virtual void RefreshObjectsData()
		{
			if (m_baseCenterObject != null)
			{
				m_baseCenterObject.transform.localPosition = m_baseCenterLocalPosition;
				m_baseCenterObject.transform.localRotation = m_baseCenterLocalRotation;
			}
			if (m_ropeObject != null)
			{
				m_ropeObject.transform.localPosition = m_ropeLocalPosition;
				m_ropeObject.transform.localRotation = m_ropeLocalRotation;
				m_ropeObject.transform.localScale = m_ropeLocalScale;
			}
			if (m_boardObject != null)
			{
				m_boardObject.transform.localPosition = m_boardLocalPosition;
				m_boardObject.transform.localRotation = m_boardLocalRotation;
			}
		}

		protected void RefreshColliderData()
		{
			if (m_colliderTriggerBoxObject != null)
			{
				m_colliderTriggerBoxObject.transform.localPosition = m_data.m_triggerBoxLocalPosition;
				m_colliderTriggerBoxObject.transform.localRotation = m_data.m_triggerBoxLocalRotation;
				m_colliderTriggerBoxObject.transform.localScale = m_data.m_triggerBoxLocalScale;
			}
		}

		protected void SetTriggerController(bool isA, bool isNormalized = false, bool isPause = false)
		{
			if (isA)
			{
				if (m_triggerAnimation != null)
				{
					if (isNormalized)
					{
						m_triggerAnimation["anim01"].normalizedTime = 0f;
					}
					m_triggerAnimation.Play("anim01");
					if (isPause)
					{
						m_triggerAnimation.Sample();
						m_triggerAnimation.Stop();
					}
				}
				if (isPause)
				{
					if (m_A != null)
					{
						m_A.gameObject.SetActive(true);
						m_A.Play(true);
					}
				}
				else if (m_A != null)
				{
					m_A.Stop(true);
					m_A.gameObject.SetActive(false);
				}
				return;
			}
			if (m_triggerAnimation != null)
			{
				if (isNormalized)
				{
					m_triggerAnimation["anim02"].normalizedTime = 0f;
				}
				m_triggerAnimation.Play("anim02");
				if (isPause)
				{
					m_triggerAnimation.Sample();
					m_triggerAnimation.Stop();
				}
			}
			if (isPause)
			{
				if (m_A != null)
				{
					m_A.Stop(true);
					m_A.gameObject.SetActive(false);
				}
			}
			else if (m_A != null)
			{
				m_A.gameObject.SetActive(true);
				m_A.Play(true);
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
			if (m_dics == null)
			{
				m_dics = ViewTools.CollectAllGameObjects(base.gameObject);
			}
			if (m_colliderTriggerBoxObject == null)
			{
				m_dics.TryGetValue("ColliderTriggerBox", out m_colliderTriggerBoxObject);
			}
			if (m_colliderTriggerBoxObject != null)
			{
				m_data.m_triggerBoxLocalPosition = m_colliderTriggerBoxObject.transform.localPosition;
				m_data.m_triggerBoxLocalRotation = m_colliderTriggerBoxObject.transform.localRotation;
				m_data.m_triggerBoxLocalScale = m_colliderTriggerBoxObject.transform.localScale;
			}
			return JsonUtility.ToJson(m_data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			m_data = StructTranslatorUtility.ToStructure<ElementData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			if (m_dics == null)
			{
				m_dics = ViewTools.CollectAllGameObjects(base.gameObject);
			}
			if (m_colliderTriggerBoxObject == null)
			{
				m_dics.TryGetValue("ColliderTriggerBox", out m_colliderTriggerBoxObject);
			}
			if (m_colliderTriggerBoxObject != null)
			{
				m_data.m_triggerBoxLocalPosition = m_colliderTriggerBoxObject.transform.localPosition;
				m_data.m_triggerBoxLocalRotation = m_colliderTriggerBoxObject.transform.localRotation;
				m_data.m_triggerBoxLocalScale = m_colliderTriggerBoxObject.transform.localScale;
			}
			return StructTranslatorUtility.ToByteArray(m_data);
		}

		public override void CoupleTriggerEnter(BaseCouple couple, Collider collider)
		{
			if (!(collider.gameObject.name == m_colliderTriggerBoxInfo))
			{
				return;
			}
			if ((bool)m_B)
			{
				m_B.gameObject.SetActive(true);
				m_B.Play();
			}
			if (!m_isTriggerControllerA)
			{
				m_isTriggerControllerA = !m_isTriggerControllerA;
				SetTriggerController(m_isTriggerControllerA);
				if (m_state == State.Wait)
				{
					m_state = State.Forward;
				}
				else if (m_state == State.Forward || m_state == State.ForwardFinished)
				{
					m_state = State.Back;
				}
				else if (m_state == State.Back || m_state == State.BackFinished)
				{
					m_state = State.Forward;
				}
			}
		}

		public virtual void TriggerEnter(BaseRole ball, Collider collider)
		{
			if (m_boardCollider != null && collider == m_boardCollider)
			{
				TriggerEnter(ball);
			}
			else if (collider.gameObject.name == m_colliderDieInfo)
			{
				CrashBall(ball);
			}
			else
			{
				if (!(collider.gameObject.name == m_colliderTriggerBoxInfo))
				{
					return;
				}
				if (m_isTriggerControllerA)
				{
					if (m_singleTrigger)
					{
						CrashBall(ball);
					}
					return;
				}
				m_singleTrigger = false;
				m_isTriggerControllerA = !m_isTriggerControllerA;
				SetTriggerController(m_isTriggerControllerA);
				if (m_state == State.Wait)
				{
					m_state = State.Forward;
				}
				else if (m_state == State.Forward || m_state == State.ForwardFinished)
				{
					m_state = State.Back;
				}
				else if (m_state == State.Back || m_state == State.BackFinished)
				{
					m_state = State.Forward;
				}
			}
		}

		protected void CrashBall(BaseRole ball)
		{
			if ((bool)ball && !GameController.IfNotDeath)
			{
				ball.CrashBall();
			}
		}

		public bool IsRecordRebirth()
		{
			return true;
		}

		public object GetRebirthData(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			RebirthData rebirthData = default(RebirthData);
			rebirthData.m_currentState = m_state;
			rebirthData.m_currentTime = m_currentTime;
			rebirthData.m_isTriggerControllerA = m_isTriggerControllerA;
			return rebirthData;
		}

		public void ResetBySavePointData(object obj)
		{
			RebirthData rebirthData = (RebirthData)obj;
			m_currentTime = rebirthData.m_currentTime;
			m_state = rebirthData.m_currentState;
			m_isTriggerControllerA = rebirthData.m_isTriggerControllerA;
			SetTriggerController(rebirthData.m_isTriggerControllerA);
			UpdateTween(m_currentTime);
		}

		public void StartRunningForRebirthData(object obj)
		{
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_CraneTile_DATA
			{
				m_state = m_state,
				m_currentTime = m_currentTime,
				m_baseCenterObject = m_baseCenterObject.transform.GetTransData(),
				m_ropeObject = m_ropeObject.transform.GetTransData(),
				m_boardObject = m_boardObject.transform.GetTransData(),
				m_colliderTriggerBoxObject = m_colliderTriggerBoxObject.transform.GetTransData(),
				m_triggerAnimation = m_triggerAnimation.GetAnimData(),
				m_isFinished = (m_isFinished ? 1 : 0),
				m_isTriggerControllerA = (m_isTriggerControllerA ? 1 : 0),
				m_A = m_A.GetParticleData(),
				m_B = m_B.GetParticleData()
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			m_rebirthData = JsonUtility.FromJson<RD_CraneTile_DATA>(rd_data as string);
			m_state = m_rebirthData.m_state;
			m_currentTime = m_rebirthData.m_currentTime;
			m_baseCenterObject.transform.SetTransData(m_rebirthData.m_baseCenterObject);
			m_ropeObject.transform.SetTransData(m_rebirthData.m_ropeObject);
			m_boardObject.transform.SetTransData(m_rebirthData.m_boardObject);
			m_colliderTriggerBoxObject.transform.SetTransData(m_rebirthData.m_colliderTriggerBoxObject);
			m_triggerAnimation.SetAnimData(m_rebirthData.m_triggerAnimation, ProcessState.Pause);
			m_isFinished = m_rebirthData.m_isFinished == 1;
			m_isTriggerControllerA = m_rebirthData.m_isTriggerControllerA == 1;
			m_A.SetParticleData(m_rebirthData.m_A, ProcessState.Pause);
			m_B.SetParticleData(m_rebirthData.m_B, ProcessState.Pause);
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			if (m_rebirthData != null)
			{
				m_triggerAnimation.SetAnimData(m_rebirthData.m_triggerAnimation, ProcessState.UnPause);
				m_A.SetParticleData(m_rebirthData.m_A, ProcessState.UnPause);
				m_B.SetParticleData(m_rebirthData.m_B, ProcessState.UnPause);
				m_rebirthData = null;
			}
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			m_rebirthData = Bson.ToObject<RD_CraneTile_DATA>(rd_data);
			m_state = m_rebirthData.m_state;
			m_currentTime = m_rebirthData.m_currentTime;
			m_baseCenterObject.transform.SetTransData(m_rebirthData.m_baseCenterObject);
			m_ropeObject.transform.SetTransData(m_rebirthData.m_ropeObject);
			m_boardObject.transform.SetTransData(m_rebirthData.m_boardObject);
			m_colliderTriggerBoxObject.transform.SetTransData(m_rebirthData.m_colliderTriggerBoxObject);
			m_triggerAnimation.SetAnimData(m_rebirthData.m_triggerAnimation, ProcessState.Pause);
			m_isFinished = m_rebirthData.m_isFinished == 1;
			m_isTriggerControllerA = m_rebirthData.m_isTriggerControllerA == 1;
			m_A.SetParticleData(m_rebirthData.m_A, ProcessState.Pause);
			m_B.SetParticleData(m_rebirthData.m_B, ProcessState.Pause);
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_CraneTile_DATA
			{
				m_state = m_state,
				m_currentTime = m_currentTime,
				m_baseCenterObject = m_baseCenterObject.transform.GetTransData(),
				m_ropeObject = m_ropeObject.transform.GetTransData(),
				m_boardObject = m_boardObject.transform.GetTransData(),
				m_colliderTriggerBoxObject = m_colliderTriggerBoxObject.transform.GetTransData(),
				m_triggerAnimation = m_triggerAnimation.GetAnimData(),
				m_isFinished = (m_isFinished ? 1 : 0),
				m_isTriggerControllerA = (m_isTriggerControllerA ? 1 : 0),
				m_A = m_A.GetParticleData(),
				m_B = m_B.GetParticleData()
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			if (m_rebirthData != null)
			{
				m_triggerAnimation.SetAnimData(m_rebirthData.m_triggerAnimation, ProcessState.UnPause);
				m_A.SetParticleData(m_rebirthData.m_A, ProcessState.UnPause);
				m_B.SetParticleData(m_rebirthData.m_B, ProcessState.UnPause);
				m_rebirthData = null;
			}
		}
	}
}
