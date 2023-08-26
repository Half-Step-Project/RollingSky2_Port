using Foundation;

namespace RS2
{
	public sealed class GameWorldThemeChangeEventArgs : EventArgs<GameWorldThemeChangeEventArgs>
	{
		public int ThemeIndex { get; private set; }

		public GameWorldThemeChangeEventArgs Initialize(int themeIndex)
		{
			ThemeIndex = themeIndex;
			return this;
		}

		protected override void OnRecycle()
		{
			ThemeIndex = 0;
		}
	}
}
