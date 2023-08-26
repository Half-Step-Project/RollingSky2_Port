using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class NormalSkateboardVehicle : BaseVehicle
	{
		public enum Status
		{
			eWait,
			eMoveToRole,
			eMove,
			eDepart
		}

		[Serializable]
		public struct TriggerData : IReadWriteBytes
		{
			public float MoveSpeed;

			public float CameraTargetScaler;

			public float BallSlideSpeed;

			public float InputNormalizeSpeed;

			public float InputSensitivity;

			public float BeginDistance;

			public Vector3 m_initializeModelPosition;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				MoveSpeed = bytes.GetSingle(ref startIndex);
				CameraTargetScaler = bytes.GetSingle(ref startIndex);
				BallSlideSpeed = bytes.GetSingle(ref startIndex);
				InputNormalizeSpeed = bytes.GetSingle(ref startIndex);
				InputSensitivity = bytes.GetSingle(ref startIndex);
				BeginDistance = bytes.GetSingle(ref startIndex);
				m_initializeModelPosition = bytes.GetVector3(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(CameraTargetScaler.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BallSlideSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(InputNormalizeSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(InputSensitivity.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
					memoryStream.WriteByteArray(m_initializeModelPosition.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public TriggerData data;

		private Status currentStatus;

		private float m_startRoleDistance;

		private Vector3 m_startModelPosition;

		private float m_moveToRoleDistance = -1f;

		private bool ifIgnoreRoleCollide;

		private bool m_isParticlePlaying;

		private RD_NormalSkateboardVehicle_DATA cachedDataVal;

		private float deltaX;

		public bool IsParticlePlaying
		{
			get
			{
				return m_isParticlePlaying;
			}
		}

		public override float VehicleRotateSpeed
		{
			get
			{
				return 3f;
			}
		}

		public override bool IfRebirthRecord
		{
			get
			{
				return true;
			}
		}

		public override void SetDefaultValue(object[] objs)
		{
			if (objs != null)
			{
				data.MoveSpeed = (float)objs[0];
				data.CameraTargetScaler = (float)objs[1];
				data.BallSlideSpeed = (float)objs[2];
				data.InputNormalizeSpeed = (float)objs[3];
				data.InputSensitivity = (float)objs[4];
				data.BeginDistance = (float)objs[5];
				data.m_initializeModelPosition = (Vector3)objs[6];
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			currentStatus = Status.eWait;
			ifIgnoreRoleCollide = false;
			modelPart.localPosition = data.m_initializeModelPosition;
			OnMoveToRoleStart();
		}

		public override void ResetElement()
		{
			base.ResetElement();
			currentStatus = Status.eWait;
			ifIgnoreRoleCollide = false;
			base.transform.localEulerAngles = StartLocalEuler;
			modelPart.localPosition = data.m_initializeModelPosition;
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if (!ifIgnoreRoleCollide && currentStatus == Status.eMoveToRole)
			{
				base.TriggerEnter(ball);
				if (modelPart != null)
				{
					modelPart.position = base.transform.position;
				}
				ResetInputParam();
				AddToBall(ball);
				BaseRole.theBall.BeginCloseTrail();
				ball.TriggerRolePlayAnim(BaseRole.AnimType.JumpOnBoatStandState);
				currentStatus = Status.eMove;
				PlayParticle();
				if ((bool)defaultAnim)
				{
					defaultAnim["anim01"].normalizedTime = 0f;
					defaultAnim.Play();
				}
			}
		}

		public override void UpdateElement()
		{
			if (currentStatus == Status.eWait)
			{
				if (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z >= data.BeginDistance)
				{
					currentStatus = Status.eMoveToRole;
				}
			}
			else if (currentStatus == Status.eMoveToRole)
			{
				OnMoveToRoleUpdate();
			}
			else if (currentStatus == Status.eMove)
			{
				OnMoveUpdate();
			}
		}

		public void OnMoveToRoleStart()
		{
			m_startModelPosition = modelPart.transform.position;
			m_startRoleDistance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - (base.groupTransform.InverseTransformPoint(base.transform.position).z + m_moveToRoleDistance);
		}

		public void OnMoveToRoleUpdate()
		{
			float num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - (base.groupTransform.InverseTransformPoint(base.transform.position).z + m_moveToRoleDistance);
			base.transform.position = new Vector3(BaseRole.BallPosition.x, base.transform.position.y, base.transform.position.z);
			float num2 = 1f - num / m_startRoleDistance;
			if (num2 >= 1f)
			{
				modelPart.transform.localPosition = Vector3.zero;
				OnMoveToRoleEnd();
			}
			else
			{
				modelPart.transform.position = Vector3.Lerp(m_startModelPosition, base.transform.position, num2);
			}
		}

		public void OnMoveUpdate()
		{
			BaseRole theBall = BaseRole.theBall;
			float inputPercent = InputController.InputPercent;
			float num = theBall.BallMoveOffset();
			if (!theBall.IfPlayBoatStandRoll())
			{
				if (Input.GetMouseButton(0))
				{
					float num2 = Mathf.Abs(inputPercent - num);
					if (inputPercent > num)
					{
						deltaX = num2 * theBall.SlideSpeed;
					}
					else if (inputPercent < num)
					{
						deltaX = num2 * (0f - theBall.SlideSpeed);
					}
					else
					{
						deltaX = 0f;
					}
				}
				else
				{
					deltaX /= 1.2f;
				}
			}
			else
			{
				deltaX = 0f;
			}
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			localEulerAngles.z = Mathf.Clamp(deltaX * -4f, -30f, 30f);
			base.transform.localEulerAngles = localEulerAngles;
		}

		public void OnMoveToRoleEnd()
		{
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
			data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}

		private void ResetInputParam()
		{
			BaseRole.theBall.ResetInputParam(data.BallSlideSpeed);
			CameraController.theCamera.ResetInputParam(data.CameraTargetScaler);
			InputController.instance.ResetInputParam(data.InputNormalizeSpeed, data.InputSensitivity);
		}

		public override void DepartFromBall(BaseRole ball, bool ifDestroy)
		{
			base.DepartFromBall(ball, ifDestroy);
			currentStatus = Status.eDepart;
			StopParticle();
		}

		public override void GiveRebirthTo(BaseRole ball)
		{
			ifIgnoreRoleCollide = true;
			base.TriggerEnter(ball);
			ResetInputParam();
			AddToBall(ball);
			currentStatus = Status.eMove;
		}

		public override void ForceSetTrans()
		{
			base.transform.eulerAngles = Vector3.zero;
			Transform parent = base.transform.parent;
			base.transform.parent = BaseRole.theBall.transform;
			base.transform.localPosition = Vector3.zero;
			base.transform.parent = parent;
		}

		protected override void PlayParticle()
		{
			base.PlayParticle();
			m_isParticlePlaying = true;
		}

		protected override void StopParticle()
		{
			base.StopParticle();
			m_isParticlePlaying = false;
		}

		public override void OnRaycastHit(bool isRaycast, RaycastHit hit, BaseRole ball)
		{
			base.OnRaycastHit(isRaycast, hit, ball);
			if (!isRaycast)
			{
				if (IsParticlePlaying)
				{
					StopParticle();
				}
			}
			else if (!IsParticlePlaying)
			{
				PlayParticle();
			}
		}

		public override void ResetBySavePointInfo(RebirthBoxData savePoint, BaseRole role)
		{
			modelPart.transform.localPosition = Vector3.zero;
			base.ResetBySavePointInfo(savePoint, role);
		}

		[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
		public override void RebirthReadData(object rd_data)
		{
			RD_NormalSkateboardVehicle_DATA rD_NormalSkateboardVehicle_DATA = JsonUtility.FromJson<RD_NormalSkateboardVehicle_DATA>(rd_data as string);
			currentStatus = rD_NormalSkateboardVehicle_DATA.currentStatus;
			defaultAnim.SetAnimData(rD_NormalSkateboardVehicle_DATA.defaultAnim, ProcessState.Pause);
			particles.SetParticlesData(rD_NormalSkateboardVehicle_DATA.particles, ProcessState.Pause);
			cachedDataVal = rD_NormalSkateboardVehicle_DATA;
			ifIgnoreRoleCollide = false;
			base.transform.localEulerAngles = StartLocalEuler;
			modelPart.transform.localPosition = data.m_initializeModelPosition;
		}

		[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
		public override object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_NormalSkateboardVehicle_DATA
			{
				defaultAnim = defaultAnim.GetAnimData(),
				particles = particles.GetParticlesData(),
				currentStatus = currentStatus
			});
		}

		[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
		public override void RebirthStartGame(object rd_data)
		{
			defaultAnim.SetAnimData(cachedDataVal.defaultAnim, ProcessState.Pause);
			particles.SetParticlesData(cachedDataVal.particles, ProcessState.Pause);
			cachedDataVal = null;
		}

		public override void RebirthReadByteData(byte[] rd_data)
		{
			RD_NormalSkateboardVehicle_DATA rD_NormalSkateboardVehicle_DATA = Bson.ToObject<RD_NormalSkateboardVehicle_DATA>(rd_data);
			currentStatus = rD_NormalSkateboardVehicle_DATA.currentStatus;
			defaultAnim.SetAnimData(rD_NormalSkateboardVehicle_DATA.defaultAnim, ProcessState.Pause);
			particles.SetParticlesData(rD_NormalSkateboardVehicle_DATA.particles, ProcessState.Pause);
			cachedDataVal = rD_NormalSkateboardVehicle_DATA;
			ifIgnoreRoleCollide = false;
			base.transform.localEulerAngles = StartLocalEuler;
			modelPart.transform.localPosition = data.m_initializeModelPosition;
		}

		public override byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_NormalSkateboardVehicle_DATA
			{
				defaultAnim = defaultAnim.GetAnimData(),
				particles = particles.GetParticlesData(),
				currentStatus = currentStatus
			});
		}

		public override void RebirthStartByteGame(byte[] rd_data)
		{
			defaultAnim.SetAnimData(cachedDataVal.defaultAnim, ProcessState.Pause);
			particles.SetParticlesData(cachedDataVal.particles, ProcessState.Pause);
			cachedDataVal = null;
		}

		public override void RebirthResetByRole(BaseRole role)
		{
			ifIgnoreRoleCollide = true;
			if (modelPart != null)
			{
				modelPart.transform.position = base.transform.position;
			}
			base.TriggerEnter(role);
			ResetInputParam();
			AddToBall(role);
			currentStatus = Status.eMove;
		}
	}
}
