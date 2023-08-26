public class WinBeforeFinishTrigger : BaseTriggerBox
{
	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		CameraController.theCamera.ChangeStateToWinStop();
		ball.IfWinBeforeFinish = true;
	}
}
