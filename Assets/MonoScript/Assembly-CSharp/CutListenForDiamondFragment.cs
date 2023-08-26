using Foundation;
using RS2;

public class CutListenForDiamondFragment : CutListenForCrownFragment
{
	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (mModel.gameObject.activeSelf)
		{
			ball.GainDiamondFragment(m_uuId);
			Mod.Event.FireNow(this, ChangeRoleIntroductionIdentifierEventArgs.Make(true, 8, mData.needCount));
			OnSwitchState(State.Trigger);
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
}
