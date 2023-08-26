using System;
using System.Text;

[Serializable]
public class SynChangeGoodsDataItem
{
	public int goodsId = -1;

	public SynDataType type = SynDataType.NONE;

	public double resultNum;

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("goodsId:").Append(goodsId).Append(":type:")
			.Append(type)
			.Append(":resultNum:")
			.Append(resultNum);
		return stringBuilder.ToString();
	}
}
