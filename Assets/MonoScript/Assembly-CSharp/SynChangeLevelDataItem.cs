using System;
using System.Text;

[Serializable]
public class SynChangeLevelDataItem
{
	public int levelId = -1;

	public SynDataType type = SynDataType.NONE;

	public PlayerServerLevelData result;

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("goodsId:").Append(levelId).Append(":type:")
			.Append(type);
		return stringBuilder.ToString();
	}
}
