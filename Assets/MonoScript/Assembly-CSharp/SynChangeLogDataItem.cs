using System;
using System.Text;

[Serializable]
public class SynChangeLogDataItem
{
	public int goodsId = -1;

	public SynDataType type = SynDataType.NONE;

	public long changeNum;

	public ulong timeStamp;

	public AssertChangeType changeType = AssertChangeType.NONE;

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("goodsId:").Append(goodsId).Append(":type:")
			.Append(type)
			.Append(":changeNum:")
			.Append(changeNum)
			.Append(":timeStamp:")
			.Append(timeStamp)
			.Append(":changeType:")
			.Append(changeType);
		return stringBuilder.ToString();
	}
}
