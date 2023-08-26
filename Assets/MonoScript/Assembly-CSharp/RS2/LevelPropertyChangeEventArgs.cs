using Foundation;

namespace RS2
{
	public class LevelPropertyChangeEventArgs : EventArgs<LevelPropertyChangeEventArgs>
	{
		public int LevelId { get; private set; }

		public LevelPropertyType ChangeType { get; private set; }

		public LevelPropertyChangeEventArgs Initialize(int id, LevelPropertyType type)
		{
			LevelId = id;
			ChangeType = type;
			return this;
		}

		protected override void OnRecycle()
		{
			ChangeType = LevelPropertyType.NONE;
			LevelId = -1;
		}
	}
}
