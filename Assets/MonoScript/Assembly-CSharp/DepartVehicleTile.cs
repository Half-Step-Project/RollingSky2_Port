public class DepartVehicleTile : BaseTile
{
	protected override void OnCollideBall(BaseRole ball)
	{
		base.OnCollideBall(ball);
		if ((bool)ball)
		{
			ball.DepartVehicle();
		}
	}
}
