using System;

[Serializable]
public class PlayerLocalGoodsStatisticalData
{
	public int goodsId;

	public double totalGetNum;

	public double totalUsedNum;

	public void InitFromServer(PlayerServerGoodsStatisticalData serverData)
	{
		goodsId = serverData.goodsId;
		totalGetNum = serverData.totalGetNum;
		totalUsedNum = serverData.totalUsedNum;
	}
}
