using System;

[Serializable]
public class PlayerServerGoodsStatisticalData
{
	public int goodsId;

	public double totalGetNum;

	public double totalUsedNum;

	public void InitFromLocal(PlayerLocalGoodsStatisticalData localData)
	{
		goodsId = localData.goodsId;
		totalGetNum = localData.totalGetNum;
		totalUsedNum = localData.totalUsedNum;
	}
}
