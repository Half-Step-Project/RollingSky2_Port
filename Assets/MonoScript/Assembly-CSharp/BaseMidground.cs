using System;
using User.TileMap;

public class BaseMidground : BaseElement
{
	public override TileObjectType GetTileObjectType
	{
		get
		{
			return TileObjectType.Midground;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return "";
	}
}
