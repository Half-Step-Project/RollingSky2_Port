using System;
using System.IO;
using Foundation;
using UnityEngine;

public class SuperStarEnemy : BaseEnemy
{
	[Serializable]
	public struct EnemyData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public float BaseBallSpeed;

		public float BeginPercent;

		public float AudioPlayTime;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			BaseBallSpeed = bytes.GetSingle(ref startIndex);
			BeginPercent = bytes.GetSingle(ref startIndex);
			AudioPlayTime = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BaseBallSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginPercent.GetBytes(), ref offset);
				memoryStream.WriteByteArray(AudioPlayTime.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public EnemyData data;

	private Animation anim;

	private GameElementSoundPlayer soundEventPlayer;

	private RD_SuperStarEnemy_DATA m_rebirthData;

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
			data.BeginDistance = (float)objs[0];
			data.ResetDistance = (float)objs[1];
			data.BaseBallSpeed = (float)objs[2];
			data.BeginPercent = (float)objs[3];
			data.AudioPlayTime = (float)objs[4];
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
		anim.wrapMode = WrapMode.Loop;
		if (data.BaseBallSpeed > 0f)
		{
			float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
			anim["anim01"].speed = speed;
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		OnTriggerStop();
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
			anim.Play();
			anim["anim01"].normalizedTime = data.BeginPercent;
			anim.Sample();
		}
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

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_SuperStarEnemy_DATA>(rd_data as string);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		commonState = m_rebirthData.commonState;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_SuperStarEnemy_DATA
		{
			anim = anim.GetAnimData(),
			commonState = commonState
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
		m_rebirthData = Bson.ToObject<RD_SuperStarEnemy_DATA>(rd_data);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		commonState = m_rebirthData.commonState;
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_SuperStarEnemy_DATA
		{
			anim = anim.GetAnimData(),
			commonState = commonState
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
