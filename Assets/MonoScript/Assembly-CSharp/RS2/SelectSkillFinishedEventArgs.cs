using Foundation;

namespace RS2
{
	public sealed class SelectSkillFinishedEventArgs : EventArgs<SelectSkillFinishedEventArgs>
	{
		public bool mActive;

		public FairySkillsName[] mSkillNames;

		public SelectSkillFinishedEventArgs Initialize(bool active, params FairySkillsName[] skillNames)
		{
			mActive = active;
			mSkillNames = skillNames;
			return this;
		}

		protected override void OnRecycle()
		{
			mActive = false;
			mSkillNames = null;
		}
	}
}
