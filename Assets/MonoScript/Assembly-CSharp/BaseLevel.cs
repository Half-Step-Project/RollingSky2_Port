using System.Collections;
using Foundation;
using RS2;
using UnityEngine;

public abstract class BaseLevel
{
	public int m_levelID;

	public LevelData m_levelData;

	public BaseLevel(int levelID)
	{
		m_levelID = levelID;
	}

	public abstract void OnInitialize();

	public abstract void OnClickGameStateButton();

	public abstract void OnGameEnd();

	public abstract void OnGameReset();

	public abstract void OnGameRebirthReset();

	public abstract void DestroyLocal();

	protected void GotoShowGameStartView()
	{
		Mod.Event.FireNow(this, Mod.Reference.Acquire<ShowGameStartEventArgs>().Initialize(false, true));
	}

	protected void GotoStopAllCoroutines()
	{
		CoroutineManager.DestroyCoroutine(CoroutineManagerType.BASELEVEL);
	}

	protected void GotoStartGame()
	{
		GameStartEventArgs gameStartEventArgs = Mod.Reference.Acquire<GameStartEventArgs>();
		gameStartEventArgs.Initialize(GameStartEventArgs.GameStartType.Normal);
		Mod.Event.Fire(this, gameStartEventArgs);
	}

	protected void GotoPlayBGMusic(float delay = 0f, float time = 0f)
	{
		if (delay > 0f)
		{
			CoroutineManager.CreateCoroutine(CoroutineManagerType.BASELEVEL, WaitPlayBGMusic(delay, time));
		}
		else
		{
			Mod.Event.FireNow(this, Mod.Reference.Acquire<BgMusicPlayEventArgs>().Initialize(time));
		}
	}

	private IEnumerator WaitPlayBGMusic(float delay = 0f, float time = 0f)
	{
		yield return new WaitForSeconds(delay);
		Mod.Event.FireNow(this, Mod.Reference.Acquire<BgMusicPlayEventArgs>().Initialize(time));
	}
}
