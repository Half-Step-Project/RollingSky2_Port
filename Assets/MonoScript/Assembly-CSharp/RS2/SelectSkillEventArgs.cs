using Foundation;

namespace RS2
{
	public sealed class SelectSkillEventArgs : EventArgs<SelectSkillEventArgs>
	{
		public bool mActive;

		public FairySkillsName[] mSkillNames;

		public SelectSkillEventArgs Initialize(bool active, params FairySkillsName[] skillNames)
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
