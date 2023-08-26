using Foundation;

public class SelectSkillFireEventArgs : EventArgs<SelectSkillFireEventArgs>
{
	public bool mActive;

	public FairySkillsName[] mSkillNames;

	public SelectSkillFireEventArgs Initialize(bool active, params FairySkillsName[] skillNames)
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
