using Foundation;
using RS2;
using UnityEngine;

public class BaseLevelFactory
{
	private GameDataModule GetGameDataModule
	{
		get
		{
			return Singleton<DataModuleManager>.Instance.GetDataModule<GameDataModule>(DataNames.GameDataModule);
		}
	}

	public BaseLevel CreateBaseLevel(int levelId)
	{
		BaseLevel baseLevel = null;
		switch (levelId)
		{
		case 1:
			baseLevel = new LevelOther(1);
			break;
		case 2:
			baseLevel = new LevelOther(2);
			break;
		case 3:
			baseLevel = new LevelOther(3);
			break;
		case 4:
			baseLevel = new LevelOther(4);
			break;
		case 5:
			baseLevel = new LevelOther(5);
			break;
		default:
			baseLevel = new LevelOther(levelId);
			break;
		}
		if (baseLevel != null)
		{
			string levelDataAsset = AssetUtility.GetLevelDataAsset(Mod.DataTable.Get<Levels_levelTable>()[levelId].LevelScriptable);
			LevelData levelData = LevelResources.theResource.GetLevelResource(levelDataAsset) as LevelData;
			if (levelData != null)
			{
				baseLevel.m_levelData = Object.Instantiate(levelData);
			}
		}
		return baseLevel;
	}

	public BaseLevel CreateBaseLevelByCurrentLevelID()
	{
		return CreateBaseLevel(GetGameDataModule.CurLevelId);
	}
}
