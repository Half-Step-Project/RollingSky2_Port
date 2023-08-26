using System;
using System.IO;
using Foundation;
using UnityEngine;

public class AnimEnemy : BaseEnemy
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public float BaseBallSpeed;

		public bool IfAutoPlay;

		public bool IfLoop;

		public float AudioPlayTime;

		public bool IfHideBegin;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			BaseBallSpeed = bytes.GetSingle(ref startIndex);
			IfAutoPlay = bytes.GetBoolean(ref startIndex);
			IfLoop = bytes.GetBoolean(ref startIndex);
			AudioPlayTime = bytes.GetSingle(ref startIndex);
			IfHideBegin = bytes.GetBoolean(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BaseBallSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfAutoPlay.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfLoop.GetBytes(), ref offset);
				memoryStream.WriteByteArray(AudioPlayTime.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfHideBegin.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public EnemyData data;

	[Range(0f, 1f)]
	public float DebugPercent;

	private float debugPercent;

	private Animation anim;

	private MeshRenderer[] meshRenders;

	private bool meshRendersEnable;

	private GameElementSoundPlayer soundEventPlayer;

	private RD_AnimEnemy_DATA m_rebirthData;

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
		commonState = CommonState.None;
		anim = GetComponentInChildren<Animation>();
		if (!anim)
		{
			return;
		}
		if (audioSource != null)
		{
			soundEventPlayer = anim.gameObject.GetComponent<GameElementSoundPlayer>();
			if (soundEventPlayer == null)
			{
				soundEventPlayer = anim.gameObject.AddComponent<GameElementSoundPlayer>();
				soundEventPlayer.gameElement = this;
				soundEventPlayer.RegistAudioEvent(anim.GetClip("anim01"), data.AudioPlayTime);
			}
		}
		if (data.BaseBallSpeed > 0f)
		{
			float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
			anim["anim01"].speed = speed;
			if (data.IfAutoPlay)
			{
				anim.Play();
			}
		}
		meshRenders = anim.GetComponentsInChildren<MeshRenderer>();
		ShowMeshRenders(meshRenders, !data.IfHideBegin);
		meshRendersEnable = !data.IfHideBegin;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		OnTriggerStop();
		ShowMeshRenders(meshRenders, !data.IfHideBegin);
		meshRendersEnable = !data.IfHideBegin;
	}

	public override void UpdateElement()
	{
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (num >= data.BeginDistance)
			{
				OnTriggerPlay();
				commonState = CommonState.Active;
			}
		}
		else if (commonState == CommonState.Active && num >= data.ResetDistance)
		{
			OnTriggerStop();
			commonState = CommonState.End;
		}
	}

	public override void OnTriggerPlay()
	{
		if ((bool)anim)
		{
			if (data.IfLoop)
			{
				anim.wrapMode = WrapMode.Loop;
			}
			else
			{
				anim.wrapMode = WrapMode.ClampForever;
			}
			anim["anim01"].normalizedTime = 0f;
			anim.Play();
		}
		ShowMeshRenders(meshRenders, true);
		meshRendersEnable = true;
		PlayParticle();
	}

	public override void OnTriggerStop()
	{
		if ((bool)anim)
		{
			anim.Play();
			anim["anim01"].normalizedTime = 0f;
			anim.Sample();
			anim.Stop();
		}
		StopParticle();
	}

	public override void SetBakeState()
	{
		anim = GetComponentInChildren<Animation>();
		if ((bool)anim && anim["anim_bake"] != null)
		{
			anim.Play("anim_bake");
			anim["anim_bake"].normalizedTime = 0.5f;
			anim.Sample();
			anim.Stop();
		}
	}

	public override void SetBaseState()
	{
		anim = GetComponentInChildren<Animation>();
		OnTriggerStop();
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<EnemyData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (debugPercent != DebugPercent)
		{
			if (anim == null)
			{
				anim = GetComponentInChildren<Animation>();
			}
			SetAnimPercent(anim, DebugPercent);
			debugPercent = DebugPercent;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_AnimEnemy_DATA>(rd_data as string);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		meshRendersEnable = m_rebirthData.meshRendersEnable;
		ShowMeshRenders(meshRenders, meshRendersEnable);
		commonState = m_rebirthData.commonState;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_AnimEnemy_DATA
		{
			anim = anim.GetAnimData(),
			commonState = commonState,
			meshRendersEnable = meshRendersEnable
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_AnimEnemy_DATA>(rd_data);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		meshRendersEnable = m_rebirthData.meshRendersEnable;
		ShowMeshRenders(meshRenders, meshRendersEnable);
		commonState = m_rebirthData.commonState;
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_AnimEnemy_DATA
		{
			anim = anim.GetAnimData(),
			commonState = commonState,
			meshRendersEnable = meshRendersEnable
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
