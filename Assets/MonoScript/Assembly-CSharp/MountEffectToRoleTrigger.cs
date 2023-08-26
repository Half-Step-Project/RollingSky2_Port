using System;
using System.IO;
using Foundation;
using UnityEngine;

public class MountEffectToRoleTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public bool IfAnim;

		public float UnmountDistance;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			IfAnim = bytes.GetBoolean(ref startIndex);
			UnmountDistance = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(IfAnim.GetBytes(), ref offset);
				memoryStream.WriteByteArray(UnmountDistance.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TriggerData data;

	private Transform effectPart;

	private ParticleSystem[] particles;

	private Animation effectAnim;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		effectPart = base.transform.Find("effect");
		particles = effectPart.GetComponentsInChildren<ParticleSystem>();
		if (data.IfAnim)
		{
			effectAnim = effectPart.GetComponent<Animation>();
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (commonState == CommonState.None)
		{
			MountEffect(ball);
			PlayParticle();
			PlayAnimation();
			commonState = CommonState.Active;
		}
	}

	public override void UpdateElement()
	{
		if (commonState == CommonState.Active && base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z >= data.UnmountDistance)
		{
			StopAnimation();
			StopParticle();
			UnmountEffect();
			commonState = CommonState.End;
		}
	}

	public override void ResetElement()
	{
		StopAnimation();
		StopParticle();
		UnmountEffect();
		commonState = CommonState.None;
	}

	private void MountEffect(BaseRole ball)
	{
		if ((bool)effectPart)
		{
			ball.MountEffect(effectPart);
		}
	}

	private void UnmountEffect()
	{
		if ((bool)effectPart)
		{
			effectPart.parent = base.transform;
		}
	}

	protected void PlayAnimation()
	{
		if ((bool)effectAnim && data.IfAnim)
		{
			effectAnim.wrapMode = WrapMode.ClampForever;
			effectAnim["anim01"].normalizedTime = 0f;
			effectAnim.Play();
		}
	}

	protected void StopAnimation()
	{
		if ((bool)effectAnim && data.IfAnim)
		{
			effectAnim.Play();
			effectAnim.Sample();
			effectAnim.Stop();
		}
	}

	protected void PlayParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Play();
			}
		}
	}

	protected void StopParticle()
	{
		if (particles != null)
		{
			for (int i = 0; i < particles.Length; i++)
			{
				particles[i].Stop();
			}
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
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Vector3 position = base.gameObject.transform.position;
		Vector3 from = base.gameObject.transform.TransformPoint(new Vector3(0f, 0f, data.UnmountDistance));
		Gizmos.color = Color.red;
		Gizmos.DrawLine(from, position);
		Gizmos.color = Color.white;
	}
}
