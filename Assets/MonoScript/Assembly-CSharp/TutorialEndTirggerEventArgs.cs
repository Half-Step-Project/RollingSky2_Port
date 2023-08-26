using Foundation;

public class TutorialEndTirggerEventArgs : EventArgs<TutorialEndTirggerEventArgs>
{
	public bool IfFreezing;

	public TutorialEndTirggerEventArgs Initialize(bool freeze)
	{
		IfFreezing = freeze;
		return this;
	}

	protected override void OnRecycle()
	{
		IfFreezing = false;
	}
}
