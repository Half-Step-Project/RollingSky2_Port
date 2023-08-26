using System;
using UnityEngine;

public class BaseVehicle : BaseTriggerBox
{
	public static string NodeModelPart = "model";

	public static string NodeEffectPart = "effect";

	public static string NodeRolePoint = "rolePoint";

	public static string AnimDieStr = "anim01";

	public static string AnimStopStr = "anim02";

	protected Transform effectPart;

	protected Transform modelPart;

	protected Transform rolePoint;

	protected Transform ballRole;

	protected Animation defaultAnim;

	protected ParticleSystem[] particles;

	public virtual bool CanJump
	{
		get
		{
			return true;
		}
	}

	public virtual float VehicleRotateSpeed
	{
		get
		{
			return -1f;
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
		modelPart = base.transform.Find(NodeModelPart);
		effectPart = base.transform.Find(NodeEffectPart);
		rolePoint = modelPart.Find(NodeRolePoint);
		defaultAnim = modelPart.GetComponentInChildren<Animation>();
		if ((bool)effectPart)
		{
			particles = effectPart.GetComponentsInChildren<ParticleSystem>();
			StopParticle();
		}
	}

	public virtual void OnGameStart()
	{
	}

	public override void ResetElement()
	{
		base.ResetElement();
		base.transform.localEulerAngles = StartLocalEuler;
		if ((bool)defaultAnim && defaultAnim.GetClip("anim01") != null)
		{
			defaultAnim.Play();
			defaultAnim["anim01"].normalizedTime = 0f;
			defaultAnim.Sample();
			defaultAnim.Stop();
		}
		StopParticle();
	}

	public virtual void AddToBall(BaseRole ball)
	{
		Transform parent = ball.ballModelObj.transform;
		ballRole = ball.ballRoleObj.transform;
		base.transform.parent = parent;
		base.transform.localPosition = Vector3.zero;
		base.transform.localEulerAngles = Vector3.zero;
		ballRole.parent = rolePoint;
		ballRole.localPosition = Vector3.zero;
		ballRole.localEulerAngles = Vector3.zero;
		ball.AddVehicle(this);
	}

	public virtual void DepartFromBall(BaseRole ball, bool ifDestroy)
	{
		ballRole.parent = ball.ballModelObj.transform;
		ballRole.localPosition = Vector3.zero;
		ballRole.localEulerAngles = Vector3.zero;
		if (base.transform.parent == ball.ballModelObj.transform)
		{
			base.transform.parent = base.groupTransform;
		}
		ballRole = null;
		StopParticle();
		if (ifDestroy && defaultAnim != null)
		{
			defaultAnim.wrapMode = WrapMode.ClampForever;
			defaultAnim[AnimStopStr].normalizedTime = 0f;
			defaultAnim.Play(AnimStopStr);
		}
	}

	public virtual void DieTogether(BaseRole ball)
	{
		if ((bool)defaultAnim && defaultAnim.GetClip(AnimDieStr) != null)
		{
			defaultAnim.wrapMode = WrapMode.Once;
			defaultAnim[AnimDieStr].normalizedTime = 0f;
			defaultAnim.Play(AnimDieStr);
		}
		StopParticle();
	}

	public virtual void GiveRebirthTo(BaseRole ball)
	{
	}

	public virtual void ForceSetTrans()
	{
	}

	public virtual void SetVehicleRotation(Transform roleModelPart, Vector3 forwardDir, float degree, float limitX)
	{
		ForceResetRoleX(roleModelPart);
		ForceSetForward(forwardDir);
		ForceSetTuringX(degree, limitX);
	}

	protected virtual void ForceResetRoleX(Transform roleModelPart)
	{
	}

	protected virtual void ForceSetForward(Vector3 forwardDir)
	{
	}

	protected virtual void ForceSetTuringX(float degree, float limitX)
	{
	}

	public virtual void OnRaycastHit(bool isRaycast, RaycastHit hit, BaseRole ball)
	{
	}

	protected virtual void PlayParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Play();
			}
		}
	}

	protected virtual void StopParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Stop();
			}
		}
	}

	public virtual void ResetBySavePointInfo(RebirthBoxData savePoint, BaseRole baseRole)
	{
		GiveRebirthTo(baseRole);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return "";
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
	}

	public override byte[] RebirthWriteByteData()
	{
		return null;
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}

	public virtual void RebirthResetByRole(BaseRole role)
	{
	}
}
