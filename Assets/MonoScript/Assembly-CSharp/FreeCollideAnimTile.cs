using System;
using System.IO;
using Foundation;
using UnityEngine;

public class FreeCollideAnimTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float TileWidth;

		public float TileHeight;

		public float BeginDistance;

		public float ResetDistance;

		public float BaseBallSpeed;

		public Vector3 BeginPos;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			TileWidth = bytes.GetSingle(ref startIndex);
			TileHeight = bytes.GetSingle(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			BaseBallSpeed = bytes.GetSingle(ref startIndex);
			BeginPos = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(TileWidth.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TileHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BaseBallSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public static readonly string AnimName1 = "anim01";

	public static readonly string AnimName2 = "anim02";

	public TileData data;

	private Animation anim;

	protected Transform effectChild;

	protected ParticleSystem[] particles;

	private bool ifCollidePlay;

	private RD_FreeCollideAnimTile_DATA m_rebirthData;

	public override float TileWidth
	{
		get
		{
			return data.TileWidth * 0.5f + BaseTile.RecycleWidthTolerance;
		}
	}

	public override float TileHeight
	{
		get
		{
			return data.TileHeight * 0.5f + BaseTile.RecycleHeightTolerance;
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
			data.TileWidth = (float)objs[0];
			data.TileHeight = (float)objs[1];
			data.BeginDistance = (float)objs[2];
			data.ResetDistance = (float)objs[3];
			data.BaseBallSpeed = (float)objs[4];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		ifCollidePlay = false;
		commonState = CommonState.None;
		anim = GetComponentInChildren<Animation>();
		if ((bool)anim && data.BaseBallSpeed > 0f)
		{
			float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
			if ((bool)anim.GetClip(AnimName1))
			{
				anim[AnimName1].speed = speed;
			}
			if ((bool)anim.GetClip(AnimName2))
			{
				ifCollidePlay = true;
				anim[AnimName2].speed = speed;
			}
		}
		if (effectChild == null)
		{
			effectChild = base.transform.Find("effect");
			if (effectChild == null)
			{
				effectChild = base.transform.Find("model/effect");
			}
			if ((bool)effectChild)
			{
				particles = effectChild.GetComponentsInChildren<ParticleSystem>();
				PlayParticle(particles, false);
			}
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		PlayAnim(anim, false);
		PlayParticle(particles, false);
	}

	public override void UpdateElement()
	{
		Transform transform = base.groupTransform;
		float num = transform.InverseTransformPoint(BaseRole.BallPosition).z - transform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (num >= data.BeginDistance)
			{
				PlayAnim(anim, AnimName1, true);
				PlayParticle(particles, true);
				commonState = CommonState.Active;
			}
		}
		else if ((commonState == CommonState.Active || commonState == CommonState.InActive) && num >= data.ResetDistance)
		{
			PlayAnim(anim, false);
			PlayParticle(particles, false);
			commonState = CommonState.End;
		}
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		base.OnCollideBall(ball);
		if (commonState == CommonState.Active)
		{
			if (ifCollidePlay)
			{
				PlayAnim(anim, AnimName2, true);
			}
			commonState = CommonState.InActive;
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
	}

	public override string Write()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return StructTranslatorUtility.ToByteArray(data);
	}

	public void OnDrawGizmos()
	{
		Transform transform = base.transform.Find("triggerPoint");
		if ((bool)transform)
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.green;
			Gizmos.DrawCube(transform.position, new Vector3(1f, 0.1f, 0.1f));
			Gizmos.color = color;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_FreeCollideAnimTile_DATA>(rd_data as string);
		commonState = (CommonState)m_rebirthData.commonState;
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
		ifCollidePlay = m_rebirthData.ifCollidePlay;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_FreeCollideAnimTile_DATA rD_FreeCollideAnimTile_DATA = new RD_FreeCollideAnimTile_DATA();
		rD_FreeCollideAnimTile_DATA.commonState = (int)commonState;
		rD_FreeCollideAnimTile_DATA.anim = anim.GetAnimData();
		if (particles != null)
		{
			rD_FreeCollideAnimTile_DATA.particles = particles.GetParticlesData();
		}
		rD_FreeCollideAnimTile_DATA.ifCollidePlay = ifCollidePlay;
		return JsonUtility.ToJson(rD_FreeCollideAnimTile_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_FreeCollideAnimTile_DATA>(rd_data);
		commonState = (CommonState)m_rebirthData.commonState;
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.Pause);
		ifCollidePlay = m_rebirthData.ifCollidePlay;
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_FreeCollideAnimTile_DATA rD_FreeCollideAnimTile_DATA = new RD_FreeCollideAnimTile_DATA();
		rD_FreeCollideAnimTile_DATA.commonState = (int)commonState;
		rD_FreeCollideAnimTile_DATA.anim = anim.GetAnimData();
		if (particles != null)
		{
			rD_FreeCollideAnimTile_DATA.particles = particles.GetParticlesData();
		}
		rD_FreeCollideAnimTile_DATA.ifCollidePlay = ifCollidePlay;
		return Bson.ToBson(rD_FreeCollideAnimTile_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		particles.SetParticlesData(m_rebirthData.particles, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
