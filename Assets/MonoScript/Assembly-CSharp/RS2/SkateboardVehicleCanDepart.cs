namespace RS2
{
	public class SkateboardVehicleCanDepart : NormalSkateboardVehicle
	{
		public override bool CanJump
		{
			get
			{
				return false;
			}
		}
	}
}
