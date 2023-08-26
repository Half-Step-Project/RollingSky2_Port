using System;
using System.IO;
using Foundation;
using UnityEngine;

public class AnimTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public float BaseBallSpeed;

		public bool IfAutoPlay;

		public bool IfLoop;

		public Vector3 BeginPos;

		public bool IfResetBorder;

		public int TileWidth;

		public int TileHeight;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			BaseBallSpeed = bytes.GetSingle(ref startIndex);
			IfAutoPlay = bytes.GetBoolean(ref startIndex);
			IfLoop = bytes.GetBoolean(ref startIndex);
			BeginPos = bytes.GetVector3(ref startIndex);
			IfResetBorder = bytes.GetBoolean(ref startIndex);
			TileWidth = bytes.GetInt32(ref startIndex);
			TileHeight = bytes.GetInt32(ref startIndex);
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
				memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfResetBorder.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TileWidth.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TileHeight.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	private Animation anim;

	private RD_AnimTile_DATA cacheData;

	[Range(0f, 1f)]
	public float DebugPercent;

	private float debugPercent;

	public override float TileWidth
	{
		get
		{
			if (!data.IfResetBorder)
			{
				return base.TileWidth;
			}
			return (float)data.TileWidth * 0.5f + BaseTile.RecycleWidthTolerance;
		}
	}

	public override float TileHeight
	{
		get
		{
			if (!data.IfResetBorder)
			{
				return base.TileHeight;
			}
			return (float)data.TileHeight * 0.5f + BaseTile.RecycleHeightTolerance;
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
		commonState = CommonState.None;
		anim = GetComponentInChildren<Animation>();
		if ((bool)anim && data.BaseBallSpeed > 0f)
		{
			if (data.IfLoop)
			{
				anim.wrapMode = WrapMode.Loop;
			}
			else
			{
				anim.wrapMode = WrapMode.Default;
			}
			float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
			anim["anim01"].speed = speed;
			if (data.IfAutoPlay)
			{
				anim["anim01"].normalizedTime = 0f;
				anim.Play();
			}
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
		else if (commonState == CommonState.Active)
		{
			if (num >= data.ResetDistance)
			{
				OnTriggerStop();
				commonState = CommonState.End;
			}
		}
		else if (num < data.BeginDistance)
		{
			commonState = CommonState.None;
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

	private void OnDrawGizmos()
	{
		Transform transform = base.transform.Find("triggerPoint");
		if ((bool)transform)
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.green;
			Gizmos.DrawCube(transform.position, new Vector3(1f, 0.1f, 0.1f));
			Gizmos.color = color;
		}
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
		RD_AnimTile_DATA rD_AnimTile_DATA = JsonUtility.FromJson<RD_AnimTile_DATA>(rd_data as string);
		anim.SetAnimData(rD_AnimTile_DATA.anim, ProcessState.Pause);
		commonState = rD_AnimTile_DATA.commonState;
		cacheData = rD_AnimTile_DATA;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_AnimTile_DATA
		{
			anim = anim.GetAnimData(),
			commonState = commonState
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		anim.SetAnimData(cacheData.anim, ProcessState.UnPause);
		cacheData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_AnimTile_DATA rD_AnimTile_DATA = Bson.ToObject<RD_AnimTile_DATA>(rd_data);
		anim.SetAnimData(rD_AnimTile_DATA.anim, ProcessState.Pause);
		commonState = rD_AnimTile_DATA.commonState;
		cacheData = rD_AnimTile_DATA;
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_AnimTile_DATA
		{
			anim = anim.GetAnimData(),
			commonState = commonState
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		anim.SetAnimData(cacheData.anim, ProcessState.UnPause);
		cacheData = null;
	}
}
