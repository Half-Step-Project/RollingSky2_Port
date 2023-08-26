public class UnrecycleMidground : BaseMidground
{
	public override bool CanRecycle
	{
		get
		{
			return false;
		}
	}
}
