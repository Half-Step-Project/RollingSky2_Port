using System.Collections.Generic;

public class LevelGoalItemData
{
	public string targetDesc;

	public int targetId = -1;

	public int targetNum;

	public int type;

	public int startLevels;

	public int levelId = -1;

	public List<int> awardList = new List<int>();

	public void Clear()
	{
		targetDesc = "";
		targetId = -1;
		targetNum = -1;
		type = -1;
		awardList.Clear();
		startLevels = 0;
		levelId = -1;
	}
}
