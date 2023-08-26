public class NormalMidground : BaseMidground
{
	public override bool CanRecycle
	{
		get
		{
			return false;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}
}
