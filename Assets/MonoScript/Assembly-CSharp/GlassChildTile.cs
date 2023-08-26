public class GlassChildTile : BaseGlassTile
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
		base.TriggerEnter(ball);
		if (currentState == GlassState.Wait && ParentKey < 0)
		{
			currentState = GlassState.Active;
		}
	}
}
