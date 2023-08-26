using RisingWin.Library;

public class ExampleCloudAchievements : CloudAchievements
{
	public override void AchievementReadAsyncProcess()
	{
		AnyCloudReadCallback();
	}

	private void AnyCloudReadCallback()
	{
		ProcessSuccess();
	}

	public override void AchievementWriteAsyncProcess()
	{
		AnyCloudWriteCallback();
	}

	private void AnyCloudWriteCallback()
	{
		ProcessSuccess();
	}
}
