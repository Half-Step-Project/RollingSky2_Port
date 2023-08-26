using System;
using System.IO;
using Foundation;
using RS2;
using UnityEngine;

public class CrownFromFragment : BaseAward, IAwardComplete, IAward
{
	[Serializable]
	public struct AwardData : IReadWriteBytes
	{
		public bool IfLoop;

		public float ResetDistance;

		public float RotateSpeed;

		public int NeedFragmentCount;

		public float EffectiveRegion;

		[Label]
		public int sortID;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			IfLoop = bytes.GetBoolean(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			RotateSpeed = bytes.GetSingle(ref startIndex);
			NeedFragmentCount = bytes.GetInt32(ref startIndex);
			EffectiveRegion = bytes.GetSingle(ref startIndex);
			sortID = bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(IfLoop.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(RotateSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(NeedFragmentCount.GetBytes(), ref offset);
				memoryStream.WriteByteArray(EffectiveRegion.GetBytes(), ref offset);
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

	protected Transform particle;

	protected bool enable;

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
		model = base.transform.Find("model");
		particle = base.transform.Find("particle");
		if (model.gameObject.activeSelf)
		{
			model.gameObject.SetActive(false);
		}
		if (colider.gameObject.activeSelf)
		{
			colider.gameObject.SetActive(false);
		}
		enable = true;
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
	}

	public override void ResetElement()
	{
		base.ResetElement();
		OnTriggerStop();
		commonState = CommonState.None;
		model.gameObject.SetActive(false);
		colider.gameObject.SetActive(false);
		StopParticle();
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.IfLoop = (bool)objs[0];
			data.ResetDistance = (float)objs[1];
			data.RotateSpeed = (int)objs[2];
			data.NeedFragmentCount = (int)objs[3];
			data.EffectiveRegion = (int)objs[4];
		}
	}

	public override void UpdateElement()
	{
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (BaseRole.theBall.GainedCrownFragment >= data.NeedFragmentCount && num >= data.EffectiveRegion)
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
		if (num >= 10f && enable)
		{
			BaseRole.theBall.ResetCrownFragmentCount();
			Mod.Event.FireNow(this, ChangeRoleIntroductionIdentifierEventArgs.Make(false, 16));
			enable = false;
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
			BaseRole.theBall.ResetCrownFragmentCount();
			Mod.Event.FireNow(this, ChangeRoleIntroductionIdentifierEventArgs.Make(false, 16));
			enable = false;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override void RebirthReadDataForDrop(object rd_data)
	{
		model.gameObject.SetActive(false);
		colider.gameObject.SetActive(false);
		commonState = CommonState.End;
	}

	public override void RebirthReadByteDataForDrop(byte[] rd_data)
	{
		model.gameObject.SetActive(false);
		colider.gameObject.SetActive(false);
		commonState = CommonState.End;
	}

	public override void OnTriggerPlay()
	{
		model.gameObject.SetActive(true);
		colider.gameObject.SetActive(true);
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

	public override void OnDrawGizmos()
	{
		Color color = Gizmos.color;
		Gizmos.color = Color.red;
		Vector3 position = base.transform.position;
		Gizmos.DrawLine(position, new Vector3(position.x, position.y, position.z + data.EffectiveRegion));
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
		return true;
	}

	public int GetHaveFragmentCount()
	{
		return data.NeedFragmentCount;
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_CrownFromFragment_DATA rD_CrownFromFragment_DATA = JsonUtility.FromJson<RD_CrownFromFragment_DATA>(rd_data as string);
		model.SetTransData(rD_CrownFromFragment_DATA.model);
		particle.SetTransData(rD_CrownFromFragment_DATA.particle);
		enable = rD_CrownFromFragment_DATA.enable;
		if (colider != null)
		{
			colider.gameObject.SetActive(rD_CrownFromFragment_DATA.coliderActive);
		}
		if (BaseRole.theBall.GainedCrownFragment >= data.NeedFragmentCount)
		{
			OnTriggerPlay();
			commonState = CommonState.Active;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_CrownFromFragment_DATA rD_CrownFromFragment_DATA = new RD_CrownFromFragment_DATA();
		rD_CrownFromFragment_DATA.anim = anim.GetAnimData();
		rD_CrownFromFragment_DATA.model = model.GetTransData();
		rD_CrownFromFragment_DATA.particle = particle.GetTransData();
		rD_CrownFromFragment_DATA.enable = enable;
		if (colider != null)
		{
			rD_CrownFromFragment_DATA.coliderActive = colider.gameObject.activeSelf;
		}
		return JsonUtility.ToJson(rD_CrownFromFragment_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_CrownFromFragment_DATA rD_CrownFromFragment_DATA = Bson.ToObject<RD_CrownFromFragment_DATA>(rd_data);
		model.SetTransData(rD_CrownFromFragment_DATA.model);
		particle.SetTransData(rD_CrownFromFragment_DATA.particle);
		enable = rD_CrownFromFragment_DATA.enable;
		if (colider != null)
		{
			colider.gameObject.SetActive(rD_CrownFromFragment_DATA.coliderActive);
		}
		if (BaseRole.theBall.GainedCrownFragment >= data.NeedFragmentCount)
		{
			OnTriggerPlay();
			commonState = CommonState.Active;
		}
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_CrownFromFragment_DATA rD_CrownFromFragment_DATA = new RD_CrownFromFragment_DATA();
		rD_CrownFromFragment_DATA.anim = anim.GetAnimData();
		rD_CrownFromFragment_DATA.model = model.GetTransData();
		rD_CrownFromFragment_DATA.particle = particle.GetTransData();
		rD_CrownFromFragment_DATA.enable = enable;
		if (colider != null)
		{
			rD_CrownFromFragment_DATA.coliderActive = colider.gameObject.activeSelf;
		}
		return Bson.ToBson(rD_CrownFromFragment_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
