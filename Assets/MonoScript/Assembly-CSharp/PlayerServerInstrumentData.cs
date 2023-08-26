using System;

[Serializable]
public class PlayerServerInstrumentData
{
	public int id;

	public int level;

	public int hideLevel;

	public int slotIndex;

	public int playState = -1;

	public int lockState = -1;

	public int playCount;

	public void InitFromLocalData(PlayerLocalInstrumentData localData)
	{
		id = localData.m_Id;
		level = localData.Level;
		hideLevel = localData.m_HideLevel;
		slotIndex = localData.SlotIndex;
		playState = localData.PlayState;
		lockState = localData.LockState;
		playCount = localData.m_PlayCount;
	}
}
