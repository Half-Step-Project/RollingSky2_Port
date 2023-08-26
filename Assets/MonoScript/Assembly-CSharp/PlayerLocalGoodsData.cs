using System;
using UnityEngine;

[Serializable]
public class PlayerLocalGoodsData : IDataToLocal
{
	[SerializeField]
	private int goodsId = -1;

	[SerializeField]
	private double num;

	private CryVDouble m_EncryNum = 0.0;

	public int GoodsId
	{
		get
		{
			return goodsId;
		}
		set
		{
			goodsId = value;
		}
	}

	public double Num
	{
		get
		{
			return m_EncryNum;
		}
		set
		{
			m_EncryNum = value;
		}
	}

	public override string ToString()
	{
		return "goodsId:" + GoodsId + ",num:" + Num;
	}

	public void InitFormServerData(PlayerServerGoodsData serverData)
	{
		if (serverData != null)
		{
			GoodsId = serverData.goodsId;
			Num = serverData.num;
		}
	}

	public void InitFromLocal()
	{
		m_EncryNum = num;
	}

	public void SaveToLocal()
	{
		num = m_EncryNum;
	}
}
