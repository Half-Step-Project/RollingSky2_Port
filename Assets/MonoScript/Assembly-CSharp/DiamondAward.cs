using System;

public class DiamondAward : CrownAward
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
			ball.GainDiamond(m_uuId, data.sortID);
			model.gameObject.SetActive(false);
			PlayParticle();
			PlaySoundEffect();
			commonState = CommonState.End;
		}
	}

	public override DropType GetDropType()
	{
		return DropType.DIAMOND;
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
