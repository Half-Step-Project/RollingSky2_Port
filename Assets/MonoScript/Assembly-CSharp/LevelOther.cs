public class LevelOther : BaseLevel
{
	public LevelOther(int levelID)
		: base(levelID)
	{
	}

	public override void OnInitialize()
	{
		GotoShowGameStartView();
	}

	public override void OnClickGameStateButton()
	{
		GotoStartGame();
		GotoPlayBGMusic();
	}

	public override void OnGameEnd()
	{
	}

	public override void OnGameRebirthReset()
	{
	}

	public override void OnGameReset()
	{
	}

	public override void DestroyLocal()
	{
	}
}
