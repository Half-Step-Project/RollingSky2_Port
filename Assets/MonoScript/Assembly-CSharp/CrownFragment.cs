using System;
using System.IO;
using Foundation;
using RS2;
using UnityEngine;

public class CrownFragment : BaseAward, IAwardFragement, IAward
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

		[Label]
		public int needCount;

		[Label]
		public int completeSortID;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			IfAutoPlay = bytes.GetBoolean(ref startIndex);
			IfLoop = bytes.GetBoolean(ref startIndex);
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			RotateSpeed = bytes.GetSingle(ref startIndex);
			sortID = bytes.GetInt32(ref startIndex);
			needCount = bytes.GetInt32(ref startIndex);
			completeSortID = bytes.GetInt32(ref startIndex);
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
				memoryStream.WriteByteArray(needCount.GetBytes(), ref offset);
				memoryStream.WriteByteArray(completeSortID.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public AwardData data;

	protected Animation anim;

	protected Animation showAnim;

	protected Animation hideAnim;

	protected Transform model;

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
		model = base.transform.Find("model");
		if (showAnim == null)
		{
			Transform transform = base.transform.Find("model/showNode");
			if (transform != null)
			{
				showAnim = transform.gameObject.GetComponent<Animation>();
			}
		}
		if (hideAnim == null)
		{
			Transform transform2 = base.transform.Find("model/hideNode");
			if (transform2 != null)
			{
				hideAnim = transform2.gameObject.GetComponent<Animation>();
			}
		}
		if (showAnim != null)
		{
			showAnim.gameObject.SetActive(false);
		}
		if (hideAnim != null)
		{
			hideAnim.gameObject.SetActive(false);
		}
		InsideGameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<InsideGameDataModule>(DataNames.InsideGameDataModule);
		DropType completeDropType = GetCompleteDropType();
		int completeSortID = GetCompleteSortID();
		if (dataModule.IsShowForAwardByDropType(completeDropType, completeSortID))
		{
			anim = showAnim;
		}
		else
		{
			anim = hideAnim;
		}
		if (anim != null)
		{
			anim.gameObject.SetActive(true);
		}
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
			ball.GainCrownFragment(m_uuId);
			Mod.Event.FireNow(this, ChangeRoleIntroductionIdentifierEventArgs.Make(true, 16, data.needCount));
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

	public int GetNeedFragementCount()
	{
		return data.needCount;
	}

	public void SetNeedFragementCount(int count)
	{
		data.needCount = count;
	}

	public void SetCompleteSortID(int sortID)
	{
		data.completeSortID = sortID;
	}

	public int GetCompleteSortID()
	{
		return data.completeSortID;
	}

	public virtual DropType GetCompleteDropType()
	{
		return DropType.CROWN;
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
		return DropType.CROWNFRAGMENT;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override void RebirthReadDataForDrop(object rd_data)
	{
		base.RebirthReadDataForDrop(rd_data);
		model.gameObject.SetActive(false);
		commonState = CommonState.End;
	}

	public override void RebirthReadByteDataForDrop(byte[] rd_data)
	{
		base.RebirthReadByteDataForDrop(rd_data);
		model.gameObject.SetActive(false);
		commonState = CommonState.End;
	}
}
