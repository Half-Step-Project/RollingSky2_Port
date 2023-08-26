public class RailBornTrigger : BaseTriggerBox
{
	public override void Initialize()
	{
		base.Initialize();
		Railway.theRailway.transform.position = StartPos;
		CameraController.theCamera.PlayAnim("Idle");
	}
}
