using System;
using System.IO;
using Foundation;
using UnityEngine;

public class CombinationPianoKeyJumpTile : CombinationPianoKeyTile
{
	[Serializable]
	public struct JumpData : IReadWriteBytes
	{
		public float JumpDistance;

		public float JumpHeight;

		public bool IfShowTrail;

		public float DefaultEmission;

		public float TargetEmission;

		public float EmissionScaler;

		public void ReadBytes(byte[] bytes, ref int startIndex)
		{
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			IfShowTrail = bytes.GetBoolean(ref startIndex);
			DefaultEmission = bytes.GetSingle(ref startIndex);
			TargetEmission = bytes.GetSingle(ref startIndex);
			EmissionScaler = bytes.GetSingle(ref startIndex);
		}

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			ReadBytes(bytes, ref startIndex);
		}

		public byte[] WriteBytes(ref int offset)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfShowTrail.GetBytes(), ref offset);
				memoryStream.WriteByteArray(DefaultEmission.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TargetEmission.GetBytes(), ref offset);
				memoryStream.WriteByteArray(EmissionScaler.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}

		public byte[] WriteBytes()
		{
			int offset = 0;
			return WriteBytes(ref offset);
		}
	}

	[Header("跳到的数据:")]
	public JumpData m_jumpData;

	[Header("测试专用预览:")]
	[Range(0f, 2f)]
	public float TestPercent;

	public float TestBaseSpeed = 7.25f;

	public float TestTime;

	private float collidePos;

	private Material m_jumpMat;

	private ParticleSystem m_particleSystem;

	private Animation m_animation;

	private RD_CombinationPianoKeyJumpTile_DATA m_rebirthData;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override string Write()
	{
		return base.Write() + "&" + JsonUtility.ToJson(m_jumpData);
	}

	public override void Read(string info)
	{
		string[] array = info.Split('&');
		m_data = JsonUtility.FromJson<Data>(array[0]);
		m_jumpData = JsonUtility.FromJson<JumpData>(array[1]);
	}

	public override void ReadBytes(byte[] bytes)
	{
		int startIndex = 0;
		m_data.ReadBytes(bytes, ref startIndex);
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

	public override void Initialize()
	{
		base.Initialize();
		collidePos = 10000f;
		if (m_jumpMat == null)
		{
			m_jumpMat = m_writeKeyObject.GetComponent<MeshRenderer>().material;
		}
		if (m_particleSystem == null)
		{
			m_particleSystem = base.gameObject.GetComponentInChildren<ParticleSystem>();
		}
		if (m_animation == null)
		{
			m_animation = base.gameObject.GetComponentInChildren<Animation>();
		}
	}

	public override void UpdateElement()
	{
		base.UpdateElement();
		if (Application.isPlaying)
		{
			float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - collidePos;
			ChangeEmissionByPercent(GetEmissionPercent(distance));
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		if (m_particleSystem != null)
		{
			m_particleSystem.Stop(true);
		}
		if (m_animation != null)
		{
			m_animation.Play();
			foreach (AnimationState item in m_animation)
			{
				item.normalizedTime = 0f;
			}
			m_animation.Sample();
			m_animation.Stop();
		}
		collidePos = 10000f;
	}

	public override void TriggerEnter(BaseRole ball, Collider collider)
	{
		if (m_writeKeyCollider != null && collider == m_writeKeyCollider)
		{
			if ((bool)ball)
			{
				ball.OnTileEnter(this);
				if (ball.IfJumpingDown)
				{
					ball.CallEndJump(base.transform.position.y, true);
				}
				if (!ball.IfJumping)
				{
					ball.CallBeginJump(base.transform.position, base.transform.position + m_jumpData.JumpDistance * ball.transform.forward, ball.transform.forward, m_jumpData.JumpHeight, BaseRole.JumpType.Super, m_jumpData.IfShowTrail);
					collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
					PlaySoundEffect();
				}
				if (ball.IfDropping)
				{
					Log.Error("Don't use jumpDistance Tile with DropTile");
				}
				if (m_particleSystem != null)
				{
					m_particleSystem.Play(true);
				}
				if (m_animation != null)
				{
					m_animation.Play();
				}
			}
			TriggerEnter(ball);
		}
		else if (m_blackKeyCollider != null && collider == m_blackKeyCollider)
		{
			CrashBall(ball);
		}
	}

	private float GetEmissionPercent(float distance)
	{
		return Mathf.Min(1f, Mathf.Max(0f, distance * m_jumpData.EmissionScaler));
	}

	private void ChangeEmissionByPercent(float percent)
	{
		if (m_jumpMat != null && percent > 0f && percent < 1f && (bool)m_jumpMat && m_jumpData.DefaultEmission >= -1f)
		{
			m_jumpMat.SetFloat("_Emmission", Mathf.Lerp(m_jumpData.DefaultEmission, m_jumpData.TargetEmission, percent));
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_CombinationPianoKeyJumpTile_DATA rD_CombinationPianoKeyJumpTile_DATA = JsonUtility.FromJson<RD_CombinationPianoKeyJumpTile_DATA>(rd_data as string);
		if (m_jumpMat != null)
		{
			m_jumpMat.SetFloat("_Emmission", rD_CombinationPianoKeyJumpTile_DATA.m_jumpMat);
		}
		m_particleSystem.SetParticleData(rD_CombinationPianoKeyJumpTile_DATA.m_particleSystem, ProcessState.Pause);
		m_animation.SetAnimData(rD_CombinationPianoKeyJumpTile_DATA.m_animation, ProcessState.Pause);
		m_axisObject.transform.SetTransData(rD_CombinationPianoKeyJumpTile_DATA.m_axisObject);
		if (m_slipperyObject != null)
		{
			m_slipperyObject.transform.SetTransData(rD_CombinationPianoKeyJumpTile_DATA.m_slipperyObject);
		}
		if (m_writeKeyObject != null)
		{
			m_writeKeyObject.transform.SetTransData(rD_CombinationPianoKeyJumpTile_DATA.m_writeKeyObject);
		}
		if (m_blackKeyObject != null)
		{
			m_blackKeyObject.transform.SetTransData(rD_CombinationPianoKeyJumpTile_DATA.m_blackKeyObject);
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		m_rebirthData = new RD_CombinationPianoKeyJumpTile_DATA();
		if (m_jumpMat != null)
		{
			m_rebirthData.m_jumpMat = m_jumpMat.GetFloat("_Emmission");
		}
		m_rebirthData.m_particleSystem = m_particleSystem.GetParticleData();
		m_rebirthData.m_animation = m_animation.GetAnimData();
		m_rebirthData.m_axisObject = m_axisObject.transform.GetTransData();
		if (m_slipperyObject != null)
		{
			m_rebirthData.m_slipperyObject = m_slipperyObject.transform.GetTransData();
		}
		if (m_writeKeyObject != null)
		{
			m_rebirthData.m_writeKeyObject = m_writeKeyObject.transform.GetTransData();
		}
		if (m_blackKeyObject != null)
		{
			m_rebirthData.m_blackKeyObject = m_blackKeyObject.transform.GetTransData();
		}
		return JsonUtility.ToJson(m_rebirthData);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		if (m_rebirthData != null)
		{
			m_particleSystem.SetParticleData(m_rebirthData.m_particleSystem, ProcessState.UnPause);
			m_animation.SetAnimData(m_rebirthData.m_animation, ProcessState.UnPause);
			m_rebirthData = null;
		}
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_CombinationPianoKeyJumpTile_DATA rD_CombinationPianoKeyJumpTile_DATA = Bson.ToObject<RD_CombinationPianoKeyJumpTile_DATA>(rd_data);
		if (m_jumpMat != null)
		{
			m_jumpMat.SetFloat("_Emmission", rD_CombinationPianoKeyJumpTile_DATA.m_jumpMat);
		}
		m_particleSystem.SetParticleData(rD_CombinationPianoKeyJumpTile_DATA.m_particleSystem, ProcessState.Pause);
		m_animation.SetAnimData(rD_CombinationPianoKeyJumpTile_DATA.m_animation, ProcessState.Pause);
		m_axisObject.transform.SetTransData(rD_CombinationPianoKeyJumpTile_DATA.m_axisObject);
		if (m_slipperyObject != null)
		{
			m_slipperyObject.transform.SetTransData(rD_CombinationPianoKeyJumpTile_DATA.m_slipperyObject);
		}
		if (m_writeKeyObject != null)
		{
			m_writeKeyObject.transform.SetTransData(rD_CombinationPianoKeyJumpTile_DATA.m_writeKeyObject);
		}
		if (m_blackKeyObject != null)
		{
			m_blackKeyObject.transform.SetTransData(rD_CombinationPianoKeyJumpTile_DATA.m_blackKeyObject);
		}
	}

	public override byte[] RebirthWriteByteData()
	{
		m_rebirthData = new RD_CombinationPianoKeyJumpTile_DATA();
		if (m_jumpMat != null)
		{
			m_rebirthData.m_jumpMat = m_jumpMat.GetFloat("_Emmission");
		}
		m_rebirthData.m_particleSystem = m_particleSystem.GetParticleData();
		m_rebirthData.m_animation = m_animation.GetAnimData();
		m_rebirthData.m_axisObject = m_axisObject.transform.GetTransData();
		if (m_slipperyObject != null)
		{
			m_rebirthData.m_slipperyObject = m_slipperyObject.transform.GetTransData();
		}
		if (m_writeKeyObject != null)
		{
			m_rebirthData.m_writeKeyObject = m_writeKeyObject.transform.GetTransData();
		}
		if (m_blackKeyObject != null)
		{
			m_rebirthData.m_blackKeyObject = m_blackKeyObject.transform.GetTransData();
		}
		return Bson.ToBson(m_rebirthData);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (m_rebirthData != null)
		{
			m_particleSystem.SetParticleData(m_rebirthData.m_particleSystem, ProcessState.UnPause);
			m_animation.SetAnimData(m_rebirthData.m_animation, ProcessState.UnPause);
			m_rebirthData = null;
		}
	}
}
