using Foundation;

namespace RS2
{
	public class PlayerPropertyChangeEventArgs : EventArgs<PlayerPropertyChangeEventArgs>
	{
		public PlayerPropertyType ChangeType { get; private set; }

		public PlayerPropertyChangeEventArgs Initialize(PlayerPropertyType type)
		{
			ChangeType = type;
			return this;
		}

		protected override void OnRecycle()
		{
			ChangeType = PlayerPropertyType.NONE;
		}
	}
}
