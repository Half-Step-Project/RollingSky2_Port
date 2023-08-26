using Foundation;

namespace RS2
{
	public sealed class RoleChangeMoodEventArgs : EventArgs<RoleChangeMoodEventArgs>
	{
		public enum TargetType
		{
			Role,
			Couple
		}

		public BaseRole.MoodFaceType MoodFaceType { get; private set; }

		public TargetType Target { get; private set; }

		public RoleChangeMoodEventArgs Initialize(TargetType target, BaseRole.MoodFaceType moodFace)
		{
			Target = target;
			MoodFaceType = moodFace;
			return this;
		}

		protected override void OnRecycle()
		{
			Target = TargetType.Role;
			MoodFaceType = BaseRole.MoodFaceType.None;
		}
	}
}
