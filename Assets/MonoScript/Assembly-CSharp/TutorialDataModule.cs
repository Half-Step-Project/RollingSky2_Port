public sealed class TutorialDataModule : IDataModule
{
	public static readonly int MAX_TIME_COUNT = 4;

	public TutorialDataModule()
	{
		Init();
	}

	public DataNames GetName()
	{
		return DataNames.TutorialDataModule;
	}

	private void Init()
	{
	}

	public bool IfShowTutorial(int LevelId)
	{
		return true;
	}

	public float GetCountDownTime(int levelId)
	{
		return Singleton<WorldConfigureController>.Instance.GetStartInfoByLevel().m_StartAnimTime;
	}
}
