using System;

[Serializable]
public class PlayerLocalMessionData
{
	public int messsionId = -1;

	public void InitFromServerData(PlayerServerMessionData serverData)
	{
		messsionId = serverData.messionId;
	}
}
