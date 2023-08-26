using System;
using System.Collections.Generic;

[Serializable]
public class PlayerServerLevelData
{
	public int levelId = -1;

	public int maxProgress;

	public int maxDiamondNum;

	public int maxCrownNum;

	public int lockState = -100;

	public int buyOutRebirth;

	public int isPerfect;

	public int finishLevelCount;

	public int playLevelCount;

	public List<int> crownCollect = new List<int>();

	public List<int> diamondsCollect = new List<int>();

	public void InitFromLocalData(PlayerLocalLevelData localData)
	{
		if (localData != null)
		{
			levelId = localData.levelId;
			maxProgress = localData.MaxProgress;
			maxDiamondNum = localData.MaxDiamondNum;
			maxCrownNum = localData.MaxCrownNum;
			lockState = localData.LockState;
			buyOutRebirth = localData.BuyOutRebirth;
			isPerfect = localData.IsPerfect;
			finishLevelCount = localData.FinishLevelCount;
			playLevelCount = localData.PlayLevelCount;
			crownCollect.Clear();
			crownCollect.AddRange(localData.CrownCollect);
			diamondsCollect.Clear();
			diamondsCollect.AddRange(localData.DiamondsCollect);
		}
	}
}
