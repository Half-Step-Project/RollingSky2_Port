using System;
using Foundation;
using RS2;

public class DiamondFragment : CrownFragment
{
	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	protected override void OnCollideBall(BaseRole ball)
	{
		if (model.gameObject.activeSelf)
		{
			ball.GainDiamondFragment(m_uuId);
			Mod.Event.FireNow(this, ChangeRoleIntroductionIdentifierEventArgs.Make(true, 8, data.needCount));
			model.gameObject.SetActive(false);
			PlayParticle();
			PlaySoundEffect();
			commonState = CommonState.End;
		}
	}

	public override DropType GetCompleteDropType()
	{
		return DropType.DIAMOND;
	}

	public override DropType GetDropType()
	{
		return DropType.DIAMONDFRAGMENT;
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
