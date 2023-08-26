public class PathToMoveByUnpredictableCrownAwardTrigger : PathToMoveByUnpredictableDiamondAwardTrigger
{
	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	protected override void OnGainAward(BaseRole ball)
	{
		ball.GainCrown(m_uuId, m_data.sortID);
	}

	public override DropType GetDropType()
	{
		return DropType.CROWN;
	}
}
