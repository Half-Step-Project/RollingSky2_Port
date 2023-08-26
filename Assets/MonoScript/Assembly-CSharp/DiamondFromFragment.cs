using System;
using Foundation;
using RS2;
using UnityEngine;

public class DiamondFromFragment : CrownFromFragment
{
	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		if (model.gameObject.activeSelf)
		{
			ball.GainDiamond(m_uuId, data.sortID);
			model.gameObject.SetActive(false);
			PlayParticle();
			PlaySoundEffect();
			commonState = CommonState.End;
			BaseRole.theBall.ResetDiamondFragmentCount();
			Mod.Event.FireNow(this, ChangeRoleIntroductionIdentifierEventArgs.Make(false, 8));
			enable = false;
		}
	}

	public override void UpdateElement()
	{
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (BaseRole.theBall.GainedDiamondFragment >= data.NeedFragmentCount && num >= data.EffectiveRegion)
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
		if (num >= 1f && enable)
		{
			BaseRole.theBall.ResetDiamondFragmentCount();
			Mod.Event.FireNow(this, ChangeRoleIntroductionIdentifierEventArgs.Make(false, 8));
			enable = false;
		}
	}

	public override DropType GetDropType()
	{
		return DropType.DIAMOND;
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
		if (BaseRole.theBall.GainedDiamondFragment >= data.NeedFragmentCount)
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
		if (BaseRole.theBall.GainedDiamondFragment >= data.NeedFragmentCount)
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
}
