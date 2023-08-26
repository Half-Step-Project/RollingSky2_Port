using System;

[Serializable]
public class LevelMetaTableData
{
	public int SeriesId = -1;

	public int ChapterId = -1;

	public int SectionId = -1;

	public int LevelId = -1;

	public int DifficultDrgee = -1;

	public bool IsShowInSelect;

	public LevelMetaTableData()
	{
	}

	public LevelMetaTableData(int seriesId, int chapterId, int sectionId, int levelId, int difficultDrgee, bool isShowInSelectd)
	{
		SeriesId = seriesId;
		ChapterId = chapterId;
		SectionId = sectionId;
		LevelId = levelId;
		DifficultDrgee = difficultDrgee;
		IsShowInSelect = isShowInSelectd;
	}

	public void InitFromOther(LevelMetaTableData other)
	{
		if (other != null)
		{
			SeriesId = other.SeriesId;
			ChapterId = other.ChapterId;
			SectionId = other.SectionId;
			LevelId = other.LevelId;
			DifficultDrgee = other.DifficultDrgee;
		}
	}
}
