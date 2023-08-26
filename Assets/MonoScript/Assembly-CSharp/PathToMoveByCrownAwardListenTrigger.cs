using Foundation;
using RS2;

public class PathToMoveByCrownAwardListenTrigger : PathToMoveByDiamondAwardListenTrigger
{
	protected override void OnGainAward(BaseRole ball)
	{
		ball.GainCrown(m_uuId, m_data.sortID);
	}

	public override DropType GetDropType()
	{
		return DropType.CROWN;
	}

	protected override void SubscribeEvent()
	{
		Mod.Event.Subscribe(EventArgs<PathToMoveByCrownAwardEventArgs>.EventId, OnListenTriggerEvent);
	}

	protected override void UnSubscribeEvent()
	{
		Mod.Event.Unsubscribe(EventArgs<PathToMoveByCrownAwardEventArgs>.EventId, OnListenTriggerEvent);
	}

	private void OnListenTriggerEvent(object sender, EventArgs e)
	{
		PathToMoveByCrownAwardEventArgs pathToMoveByCrownAwardEventArgs = e as PathToMoveByCrownAwardEventArgs;
		if (pathToMoveByCrownAwardEventArgs != null && pathToMoveByCrownAwardEventArgs.m_groupID == m_data.m_groupID)
		{
			m_isPlaying = true;
		}
	}
}
