using Foundation;

namespace RS2
{
	public sealed class SelectSkillLightupEventArgs : EventArgs<SelectSkillLightupEventArgs>
	{
		public bool mActive;

		public FairySkillsName[] mSkillNames;

		public SelectSkillLightupEventArgs Initialize(bool active, params FairySkillsName[] skillNames)
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
