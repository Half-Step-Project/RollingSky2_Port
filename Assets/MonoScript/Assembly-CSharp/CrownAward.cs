using System;
using System.IO;
using Foundation;
using UnityEngine;

public class CrownAward : BaseAward, IAwardComplete, IAward
{
	[Serializable]
	public struct AwardData : IReadWriteBytes
	{
		public bool IfAutoPlay;

		public bool IfLoop;

		public float BeginDistance;

		public float ResetDistance;

		public float RotateSpeed;

		[Label]
		public int sortID;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			IfAutoPlay = bytes.GetBoolean(ref startIndex);
			IfLoop = bytes.GetBoolean(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			RotateSpeed = bytes.GetSingle(ref startIndex);
			sortID = bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(IfAutoPlay.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfLoop.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(RotateSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(sortID.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public AwardData data;

	protected Animation anim;

	protected Transform model;

	private RD_CrownAward_DATA m_rebirthData;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		commonState = CommonState.None;
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		DropType dropType = GetDropType();
		bool flag = dataModule.IsShowForAwardByDropType(dropType, data.sortID);
		anim = (flag ? animShowNode : animHideNode);
		if ((bool)modelShowNode)
		{
			modelShowNode.SetActive(flag);
		}
		if ((bool)modelHideNode)
		{
			modelHideNode.SetActive(!flag);
		}
		model = base.transform.Find("model");
		if (data.IfAutoPlay)
		{
			OnTriggerPlay();
			commonState = CommonState.Active;
		}
	}

	public override void ResetElement()
	{
		base.ResetElement();
		OnTriggerStop();
		commonState = CommonState.None;
		if (model != null)
		{
			model.gameObject.SetActive(true);
		}
		StopParticle();
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.IfAutoPlay = (bool)objs[0];
			data.IfLoop = (bool)objs[1];
			data.BeginDistance = (float)objs[2];
			data.ResetDistance = (float)objs[3];
			data.RotateSpeed = (float)objs[4];
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
			base.transform.Rotate(Vector3.up * data.RotateSpeed);
			if (num >= data.ResetDistance)
			{
				OnTriggerStop();
				commonState = CommonState.End;
			}
		}
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		if (model.gameObject.activeSelf)
		{
			ball.GainCrown(m_uuId, data.sortID);
			model.gameObject.SetActive(false);
			PlayParticle();
			PlaySoundEffect();
			commonState = CommonState.End;
		}
	}

	public override void OnTriggerPlay()
	{
		if ((bool)anim)
		{
			anim.gameObject.SetActive(true);
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
		PlayAnim(anim, false);
		StopParticle();
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<AwardData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<AwardData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_CrownAward_DATA>(rd_data as string);
		base.RebirthReadData((object)m_rebirthData.baseData);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		model.SetTransData(m_rebirthData.model);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_CrownAward_DATA
		{
			baseData = (base.RebirthWriteData() as string),
			anim = anim.GetAnimData(),
			model = model.GetTransData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		if (m_rebirthData != null)
		{
			anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override void RebirthReadDataForDrop(object rd_data)
	{
		base.RebirthReadDataForDrop(rd_data);
		model.gameObject.SetActive(false);
		commonState = CommonState.End;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_CrownAward_DATA>(rd_data);
		base.RebirthReadByteData(m_rebirthData.baseBytesData);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		model.SetTransData(m_rebirthData.model);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_CrownAward_DATA
		{
			baseBytesData = base.RebirthWriteByteData(),
			anim = anim.GetAnimData(),
			model = model.GetTransData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		if (m_rebirthData != null)
		{
			anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		}
		m_rebirthData = null;
	}

	public override void RebirthReadByteDataForDrop(byte[] rd_data)
	{
		base.RebirthReadByteDataForDrop(rd_data);
		model.gameObject.SetActive(false);
		commonState = CommonState.End;
	}

	public int GetAwardSortID()
	{
		return data.sortID;
	}

	public void SetAwardSortID(int id)
	{
		data.sortID = id;
	}

	public virtual DropType GetDropType()
	{
		return DropType.CROWN;
	}

	public bool IsHaveFragment()
	{
		return false;
	}

	public int GetHaveFragmentCount()
	{
		return 0;
	}
}
