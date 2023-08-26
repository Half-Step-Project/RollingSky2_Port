using System;
using System.IO;
using Foundation;
using UnityEngine;

public class SuperStarJumpTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public float BeginPercent;

		public float BaseBallSpeed;

		public float JumpDistance;

		public float JumpHeight;

		public float MoveSpeed;

		public bool IfShowTrail;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			BeginPercent = bytes.GetSingle(ref startIndex);
			BaseBallSpeed = bytes.GetSingle(ref startIndex);
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			IfShowTrail = bytes.GetBoolean(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginPercent.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BaseBallSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfShowTrail.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public static readonly string NodetriggerEffect = "triggerEffect";

	public static readonly string NodeLeftEffect = "effect_1";

	public static readonly string NodeRightEffect = "effect_2";

	public TileData data;

	private Animation anim;

	private ParticleSystem leftEffect;

	private ParticleSystem rightEffect;

	private RD_SuperStarJumpTile_DATA m_rebirthData;

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
			data.BeginPercent = (float)objs[2];
			data.BaseBallSpeed = (float)objs[3];
			data.JumpDistance = (float)objs[4];
			data.JumpHeight = (float)objs[5];
			data.MoveSpeed = (float)objs[6];
			data.IfShowTrail = (bool)objs[7];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		commonState = CommonState.None;
		anim = GetComponentInChildren<Animation>();
		if ((bool)anim && data.BaseBallSpeed > 0f)
		{
			anim.wrapMode = WrapMode.Loop;
			float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
			anim["anim01"].speed = speed;
		}
		Transform obj = base.transform.Find("model");
		Transform transform = obj.Find(NodetriggerEffect + "/" + NodeLeftEffect);
		if ((bool)transform)
		{
			leftEffect = transform.GetComponentInChildren<ParticleSystem>();
		}
		transform = obj.Find(NodetriggerEffect + "/" + NodeRightEffect);
		if ((bool)transform)
		{
			rightEffect = transform.GetComponentInChildren<ParticleSystem>();
		}
	}

	public override void ResetElement()
	{
		if ((bool)anim)
		{
			anim.Play();
			anim["anim01"].normalizedTime = 0f;
			anim.Sample();
			anim.Stop();
		}
		if ((bool)leftEffect)
		{
			leftEffect.Stop();
		}
		if ((bool)rightEffect)
		{
			rightEffect.Stop();
		}
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
		else if (commonState == CommonState.Active)
		{
			if (num >= data.ResetDistance)
			{
				ResetElement();
				commonState = CommonState.End;
			}
		}
		else if (num < data.BeginDistance)
		{
			commonState = CommonState.None;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (!ball || ball.IfDie)
		{
			return;
		}
		ball.OnTileEnter(this);
		Vector3 position = leftEffect.transform.position;
		Vector3 position2 = rightEffect.transform.position;
		float sqrMagnitude = (ball.transform.position - position).sqrMagnitude;
		float sqrMagnitude2 = (ball.transform.position - position2).sqrMagnitude;
		if (sqrMagnitude < sqrMagnitude2)
		{
			if ((bool)leftEffect)
			{
				leftEffect.Play();
			}
		}
		else if ((bool)rightEffect)
		{
			rightEffect.Play();
		}
		PlaySoundEffect();
		if (ball.IfJumpingDown)
		{
			ball.CallEndJump(base.transform.position.y, true);
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.Super);
		}
		else if (!ball.IfJumping)
		{
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.Super);
		}
		else if (ball.IfDropping)
		{
			ball.CallEndDrop(base.transform.position.y);
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.Super);
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
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_SuperStarJumpTile_DATA>(rd_data as string);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		commonState = m_rebirthData.commonState;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_SuperStarJumpTile_DATA
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
		m_rebirthData = Bson.ToObject<RD_SuperStarJumpTile_DATA>(rd_data);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		commonState = m_rebirthData.commonState;
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_SuperStarJumpTile_DATA
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
