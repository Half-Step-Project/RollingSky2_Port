using System;
using Foundation;
using UnityEngine;
using User.TileMap;

public class BaseTile : BaseElement
{
	public static readonly float RecycleWidthTolerance = 0.3f;

	public static readonly float RecycleHeightTolerance = 0.3f;

	public static readonly float DefaultBorderSize = 0.5f;

	public float TestDistance;

	public int currentRowIndex;

	public virtual bool IfDroppable
	{
		get
		{
			return false;
		}
	}

	public override TileObjectType GetTileObjectType
	{
		get
		{
			return TileObjectType.Tile;
		}
	}

	public virtual Vector3 TileCenter
	{
		get
		{
			return base.transform.position;
		}
	}

	public virtual float TileWidth
	{
		get
		{
			return DefaultBorderSize + RecycleWidthTolerance;
		}
	}

	public virtual float TileHeight
	{
		get
		{
			return DefaultBorderSize + RecycleHeightTolerance;
		}
	}

	public virtual float TileMagin
	{
		get
		{
			return 0f;
		}
	}

	public virtual float RealPosY
	{
		get
		{
			return base.transform.position.y;
		}
	}

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	protected virtual void OnCollideBall(BaseRole ball)
	{
		ball.OnTileEnter(this);
		if (ball.IfJumpingDown)
		{
			ball.CallEndJump(RealPosY);
		}
		if (ball.IfDropping)
		{
			ball.CallEndDrop(RealPosY);
		}
	}

	public virtual void OnExitBall(BaseRole ball)
	{
		ball.OnTileExit(this);
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball)
		{
			OnCollideBall(ball);
		}
	}

	public virtual void TriggerExit(BaseRole ball)
	{
		if ((bool)ball)
		{
			OnExitBall(ball);
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_BaseTile_DATA rD_BaseTile_DATA = JsonUtility.FromJson<RD_BaseTile_DATA>(rd_data as string);
		base.RebirthReadData(rD_BaseTile_DATA.baseData);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_BaseTile_DATA
		{
			baseData = (base.RebirthWriteData() as string)
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_BaseTile_DATA rD_BaseTile_DATA = Bson.ToObject<RD_BaseTile_DATA>(rd_data);
		base.RebirthReadByteData(rD_BaseTile_DATA.baseBytesData);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_BaseTile_DATA
		{
			baseBytesData = base.RebirthWriteByteData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
