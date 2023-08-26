using Foundation;

public sealed class GuideUiEventArgs : EventArgs<GuideUiEventArgs>
{
	public const int DefaultGuideUiIndex = -1;

	public int GuideUiIndex { get; private set; }

	protected override void OnRecycle()
	{
		GuideUiIndex = -1;
	}

	public GuideUiEventArgs Initialize(int index = -1)
	{
		GuideUiIndex = index;
		return this;
	}
}
