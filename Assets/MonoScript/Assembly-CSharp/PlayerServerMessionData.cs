using System;

[Serializable]
public class PlayerServerMessionData
{
	public int messionId;

	public void InitFromLocalData(PlayerLocalMessionData messionData)
	{
		messionId = messionData.messsionId;
	}
}
