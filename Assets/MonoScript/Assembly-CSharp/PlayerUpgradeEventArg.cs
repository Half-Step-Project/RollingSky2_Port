using Foundation;

public sealed class PlayerUpgradeEventArg : EventArgs<PlayerUpgradeEventArg>
{
	public static PlayerUpgradeEventArg Make()
	{
		return Mod.Reference.Acquire<PlayerUpgradeEventArg>();
	}

	protected override void OnRecycle()
	{
	}
}
