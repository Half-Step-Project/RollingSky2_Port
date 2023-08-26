public class StopPathToMoveTrigger : BaseTriggerBox
{
	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball)
		{
			ball.ChangeToMove();
		}
	}
}
