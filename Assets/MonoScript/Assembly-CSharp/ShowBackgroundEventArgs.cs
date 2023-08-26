using Foundation;

public sealed class ShowBackgroundEventArgs : EventArgs<ShowBackgroundEventArgs>
{
	public const int DefaultBackIndex = -1;

	public int BackgroundIndex { get; private set; }

	protected override void OnRecycle()
	{
		BackgroundIndex = -1;
	}

	public ShowBackgroundEventArgs Initialize(int index = -1)
	{
		BackgroundIndex = index;
		return this;
	}
}
