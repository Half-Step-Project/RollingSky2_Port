using System;
using System.IO;
using Foundation;
using UnityEngine;

public class AlpacaVehicle : BaseVehicle
{
	public enum AlpacaState
	{
		Wait,
		StartAnim,
		Move,
		Depart
	}

	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float CameraTargetScaler;

		public float BallSlideSpeed;

		public float InputNormalizeSpeed;

		public float InputSensitivity;

		public float BeginDistance;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			CameraTargetScaler = bytes.GetSingle(ref startIndex);
			BallSlideSpeed = bytes.GetSingle(ref startIndex);
			InputNormalizeSpeed = bytes.GetSingle(ref startIndex);
			InputSensitivity = bytes.GetSingle(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(CameraTargetScaler.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BallSlideSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(InputNormalizeSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(InputSensitivity.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	private const string WAIT_ANIM = "Ready";

	private const string START_ANIM = "ReadyGo";

	private const string MOVE_ANIM = "Run";

	private const string DEPART_ANIM = "anim03";

	private const string DisappearNodeName = "DisappearEffect";

	public AlpacaState CurrentState;

	private ParticleSystem disappearEffect;

	private bool ifIgnoreRoleCollide;

	private float deltaX;

	private RD_AlpacaVehicle_DATA cachedDataVal;

	public TriggerData data;

	public override bool CanJump
	{
		get
		{
			return false;
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
		CurrentState = AlpacaState.Wait;
		ifIgnoreRoleCollide = false;
		Transform transform = base.transform.Find("DisappearEffect");
		if (transform != null)
		{
			disappearEffect = transform.GetComponentInChildren<ParticleSystem>();
			if ((bool)disappearEffect)
			{
				disappearEffect.Stop();
			}
		}
		if (!defaultAnim)
		{
			return;
		}
		foreach (AnimationState item in defaultAnim)
		{
			item.speed = Railway.theRailway.SpeedForward / 6f;
		}
		defaultAnim.gameObject.active = true;
		defaultAnim["Ready"].normalizedTime = 0f;
		defaultAnim.Play("Ready");
	}

	public override void OnGameStart()
	{
		if (CurrentState == AlpacaState.StartAnim)
		{
			if ((bool)defaultAnim)
			{
				defaultAnim["Run"].normalizedTime = 0f;
				defaultAnim.Play("Run");
				BaseRole.theBall.TriggerRolePlayAnim(BaseRole.AnimType.RideYangTuo);
			}
			PlayParticle();
			CurrentState = AlpacaState.Move;
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		if ((bool)defaultAnim)
		{
			defaultAnim.gameObject.SetActive(true);
		}
		CurrentState = AlpacaState.Wait;
		ifIgnoreRoleCollide = false;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (!ifIgnoreRoleCollide && CurrentState != AlpacaState.Depart && !ball.IfOnVehicle)
		{
			ResetInputParam();
			AddToBall(ball);
			BaseRole.theBall.BeginCloseTrail();
		}
	}

	public override void InitElement()
	{
		if (CurrentState == AlpacaState.Wait)
		{
			if ((bool)defaultAnim)
			{
				defaultAnim["ReadyGo"].normalizedTime = 0f;
				defaultAnim.Play("ReadyGo");
			}
			CurrentState = AlpacaState.StartAnim;
		}
	}

	public override void UpdateElement()
	{
		if (CurrentState != AlpacaState.StartAnim && CurrentState == AlpacaState.Move)
		{
			OnMoveUpdate();
		}
	}

	public override void DepartFromBall(BaseRole ball, bool ifDestroy)
	{
		if (CurrentState == AlpacaState.Move)
		{
			base.DepartFromBall(ball, ifDestroy);
			CurrentState = AlpacaState.Depart;
			if ((bool)defaultAnim)
			{
				defaultAnim.gameObject.SetActive(false);
			}
			if ((bool)disappearEffect)
			{
				disappearEffect.Play();
			}
		}
	}

	private void ResetInputParam()
	{
		BaseRole.theBall.ResetInputParam(data.BallSlideSpeed);
		CameraController.theCamera.ResetInputParam(data.CameraTargetScaler);
		InputController.instance.ResetInputParam(data.InputNormalizeSpeed, data.InputSensitivity);
	}

	public override void ForceSetTrans()
	{
		base.transform.eulerAngles = Vector3.zero;
		Transform parent = base.transform.parent;
		base.transform.parent = BaseRole.theBall.transform;
		base.transform.localPosition = Vector3.zero;
		base.transform.parent = parent;
	}

	public override void GiveRebirthTo(BaseRole ball)
	{
		ifIgnoreRoleCollide = true;
		base.TriggerEnter(ball);
		ResetInputParam();
		AddToBall(ball);
		CurrentState = AlpacaState.Move;
	}

	private void OnMoveUpdate()
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
		localEulerAngles.y = Mathf.Clamp(deltaX * 4f, -30f, 30f);
		base.transform.localEulerAngles = localEulerAngles;
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

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_AlpacaVehicle_DATA rD_AlpacaVehicle_DATA = Bson.ToObject<RD_AlpacaVehicle_DATA>(rd_data);
		CurrentState = rD_AlpacaVehicle_DATA.currentState;
		rD_AlpacaVehicle_DATA.defaultAnim.animTime = 0f;
		string animaName = rD_AlpacaVehicle_DATA.defaultAnim.animaName;
		rD_AlpacaVehicle_DATA.defaultAnim.animaName = "Ready";
		defaultAnim.SetAnimData(rD_AlpacaVehicle_DATA.defaultAnim, ProcessState.UnPause);
		particles.SetParticlesData(rD_AlpacaVehicle_DATA.particles, ProcessState.Pause);
		effectPart.gameObject.SetActive(false);
		rD_AlpacaVehicle_DATA.defaultAnim.animaName = animaName;
		cachedDataVal = rD_AlpacaVehicle_DATA;
		ifIgnoreRoleCollide = false;
		base.transform.localEulerAngles = StartLocalEuler;
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_AlpacaVehicle_DATA
		{
			defaultAnim = defaultAnim.GetAnimData(),
			particles = particles.GetParticlesData(),
			currentState = CurrentState
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		defaultAnim.SetAnimData(cachedDataVal.defaultAnim, ProcessState.UnPause);
		particles.SetParticlesData(cachedDataVal.particles, ProcessState.UnPause);
		effectPart.gameObject.SetActive(true);
		cachedDataVal = null;
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_AlpacaVehicle_DATA rD_AlpacaVehicle_DATA = JsonUtility.FromJson<RD_AlpacaVehicle_DATA>(rd_data as string);
		CurrentState = rD_AlpacaVehicle_DATA.currentState;
		defaultAnim.SetAnimData(rD_AlpacaVehicle_DATA.defaultAnim, ProcessState.Pause);
		particles.SetParticlesData(rD_AlpacaVehicle_DATA.particles, ProcessState.Pause);
		cachedDataVal = rD_AlpacaVehicle_DATA;
		ifIgnoreRoleCollide = false;
		base.transform.localEulerAngles = StartLocalEuler;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_AlpacaVehicle_DATA
		{
			defaultAnim = defaultAnim.GetAnimData(),
			particles = particles.GetParticlesData(),
			currentState = CurrentState
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		defaultAnim.SetAnimData(cachedDataVal.defaultAnim, ProcessState.Pause);
		particles.SetParticlesData(cachedDataVal.particles, ProcessState.Pause);
		cachedDataVal = null;
	}

	public override void RebirthResetByRole(BaseRole role)
	{
		ifIgnoreRoleCollide = true;
		base.TriggerEnter(role);
		ResetInputParam();
		AddToBall(role);
		CurrentState = AlpacaState.Move;
	}
}
