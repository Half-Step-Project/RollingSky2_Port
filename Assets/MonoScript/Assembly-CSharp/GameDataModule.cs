using Foundation;
using RS2;

public sealed class GameDataModule : IDataModule
{
	private int m_CurLevelId = 1;

	public int CurLevelId
	{
		get
		{
			return m_CurLevelId;
		}
		set
		{
			m_CurLevelId = value;
		}
	}

	public DataNames GetName()
	{
		return DataNames.GameDataModule;
	}

	public void Reset()
	{
		m_CurLevelId = 1;
	}

	public int GetLevelNum()
	{
		return Mod.DataTable.Get<Levels_levelTable>().Records.Length;
	}

	public Levels_levelTable GetLevelTableById(int id)
	{
		return Mod.DataTable.Get<Levels_levelTable>()[id];
	}

	public LevelTarget_levelTargetTable GetTargetTableById(int id)
	{
		return Mod.DataTable.Get<LevelTarget_levelTargetTable>()[id];
	}

	public Award_awardTable GetAwardTableById(int id)
	{
		return Mod.DataTable.Get<Award_awardTable>()[id];
	}

	public Goods_goodsTable GetGoodsTableById(int id)
	{
		return Mod.DataTable.Get<Goods_goodsTable>()[id];
	}

	public GoodsTeam_goodsTeamTable GetGoodsTeamTableById(int id)
	{
		return Mod.DataTable.Get<GoodsTeam_goodsTeamTable>()[id];
	}

	public LevelUnLock_table GetLevelUnLockById(int id)
	{
		return Mod.DataTable.Get<LevelUnLock_table>()[id];
	}

	public LevelProgressAward_table GetLevelProgressAwardByPercent(int percent)
	{
		return Mod.DataTable.Get<LevelProgressAward_table>()[percent];
	}

	public int GetLevelSetMaxDiamonds(int levelId)
	{
		Levels_levelTable levelTableById = GetLevelTableById(levelId);
		if (levelTableById != null)
		{
			return levelTableById.Diamonds;
		}
		return 0;
	}

	public int GetLevelSetMaxCrowns(int levelId)
	{
		Levels_levelTable levelTableById = GetLevelTableById(levelId);
		if (levelTableById != null)
		{
			return levelTableById.Crowns;
		}
		return 0;
	}
}
