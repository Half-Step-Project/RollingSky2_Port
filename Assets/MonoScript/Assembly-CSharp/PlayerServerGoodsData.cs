using System;

[Serializable]
public class PlayerServerGoodsData
{
	public int goodsId = -1;

	public double num;

	public override string ToString()
	{
		return "id:" + goodsId + ",num:" + num;
	}

	public void InitFromLocalData(PlayerLocalGoodsData localData)
	{
		if (localData != null)
		{
			goodsId = localData.GoodsId;
			num = localData.Num;
		}
	}
}
