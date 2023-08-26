using System.Collections.Generic;
using RS2;

public class LevelUnLockData
{
	public int relation = -1;

	public int id = -1;

	public LevelLockType lockType;

	public int lockTypeId = -1;

	public int unLockNum = -1;

	public List<int> linkLevels = new List<int>();

	public int desc = -1;

	public void InitFromTable(LevelUnLock_table table)
	{
		id = table.Id;
		lockType = (LevelLockType)table.LockType;
		lockTypeId = table.LockTypeId;
		unLockNum = table.UnLockNum;
		linkLevels = MonoSingleton<GameTools>.Instacne.StringToIntList(table.LinkLevel);
		relation = table.Relation;
		int desc2 = table.Desc;
	}
}
