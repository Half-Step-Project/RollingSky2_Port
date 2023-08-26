using Foundation;
using RS2;

public sealed class ResultPageDataModule : IDataModule
{
	private static readonly int RESET_CAN_SHOW_TIPS = 10;

	private static readonly int RESET_CAN_SHOW_PERCENT = 25;

	private ProgressManager progressManager;

	public int Progress
	{
		get
		{
			int curLevelId = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
			return progressManager.GetLastProgress(curLevelId);
		}
	}

	public int LevelId
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId;
		}
	}

	public int DiamondNum
	{
		get
		{
			return progressManager.GetLastDiamondNum(Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
		}
	}

	public int CrownNum
	{
		get
		{
			return progressManager.GetLastCrownNum(Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule).CurLevelId);
		}
	}

	public int RemainBalls
	{
		get
		{
			return (int)Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule).GetPlayGoodsNum(1);
		}
	}

	public int StarLevel
	{
		get
		{
			return MonoSingleton<GameTools>.Instacne.ComputerLevelStar(LevelId, Progress, DiamondNum, CrownNum);
		}
	}

	public int MaxDiamondNum
	{
		get
		{
			GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
			return dataModule.GetLevelTableById(dataModule.CurLevelId).Diamonds;
		}
	}

	public int MaxCrownNum
	{
		get
		{
			GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
			return dataModule.GetLevelTableById(dataModule.CurLevelId).Crowns;
		}
	}

	public string LevelTitle
	{
		get
		{
			GameDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
			int title_id = dataModule.GetLevelTableById(dataModule.CurLevelId).Title_id;
			return Mod.Localization.GetInfoById(title_id);
		}
	}

	public int MaxProgress
	{
		get
		{
			PlayerLocalLevelData playerLevelData = PlayerDataModule.Instance.GetPlayerLevelData(LevelId);
			if (playerLevelData != null)
			{
				return playerLevelData.MaxProgress;
			}
			return 0;
		}
	}

	public LevelFinishedType FinishedType { get; set; }

	public ResultPageDataModule()
	{
		Init();
	}

	public DataNames GetName()
	{
		return DataNames.ResultPageDataModule;
	}

	public void Init()
	{
		progressManager = Singleton<ProgressManager>.Instance;
	}

	public void ResetShowTutorial(int progress)
	{
		PlayerDataModule dataModule = Singleton<DataModuleManager>.Instance.GetDataModule<PlayerDataModule>(DataNames.PlayerDataModule);
		if (dataModule.GetCanShowTutorialTips() && progress >= RESET_CAN_SHOW_TIPS)
		{
			dataModule.ForbidShowTutorialTips();
		}
		if (dataModule.GetCanShowTutorial() && progress >= RESET_CAN_SHOW_PERCENT)
		{
			dataModule.ForbidShowTutorial();
		}
	}
}
