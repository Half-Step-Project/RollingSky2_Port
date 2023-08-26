using System;
using Foundation;
using UnityEngine;

public class TrapRootTile : BaseTrapTile
{
	private Vector3 beginPos;

	private Vector3 endPos;

	private Vector3 halfPos;

	private float halfSecondPercent;

	private float collidePos;

	private Transform state1;

	private Transform state2;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		collidePos = 10000f;
		beginPos = StartLocalPos;
		data.MoveDistance = Mathf.Abs(data.MoveDistance);
		data.MoveDistance2 = 0f - Mathf.Abs(data.MoveDistance2);
		if (state1 == null || state2 == null)
		{
			state1 = base.transform.Find("model/state1");
			state2 = base.transform.Find("model/state2");
		}
		if ((bool)state1)
		{
			state1.gameObject.SetActive(true);
		}
		if ((bool)state2)
		{
			state2.gameObject.SetActive(false);
		}
		if (data.MoveDirection == TileDirection.Down)
		{
			endPos = beginPos + new Vector3(0f, 0f - data.MoveDistance, 0f);
			halfPos = endPos + new Vector3(0f, 0f - data.MoveDistance2, 0f);
		}
		else if (data.MoveDirection == TileDirection.Up)
		{
			endPos = beginPos + new Vector3(0f, data.MoveDistance, 0f);
			halfPos = endPos + new Vector3(0f, data.MoveDistance2, 0f);
		}
		else if (data.MoveDirection == TileDirection.Left)
		{
			endPos = beginPos + new Vector3(0f - data.MoveDistance, 0f, 0f);
			halfPos = endPos + new Vector3(0f - data.MoveDistance2, 0f, 0f);
		}
		else if (data.MoveDirection == TileDirection.Right)
		{
			endPos = beginPos + new Vector3(data.MoveDistance, 0f, 0f);
			halfPos = endPos + new Vector3(data.MoveDistance2, 0f, 0f);
		}
		else if (data.MoveDirection == TileDirection.Forward)
		{
			endPos = beginPos + new Vector3(0f, 0f, data.MoveDistance);
			halfPos = endPos + new Vector3(0f, 0f, data.MoveDistance2);
		}
		else if (data.MoveDirection == TileDirection.Backward)
		{
			endPos = beginPos + new Vector3(0f, 0f, 0f - data.MoveDistance);
			halfPos = endPos + new Vector3(0f, 0f, 0f - data.MoveDistance2);
		}
		data.SpeedScaler = Mathf.Abs(data.SpeedScaler);
		if ((double)data.SecondPercent <= 0.01)
		{
			Debug.Log("Second Percent is too small" + data.SecondPercent);
			data.SecondPercent = 0.01f;
		}
		halfSecondPercent = data.SecondPercent + (1f - data.SecondPercent) / 2f;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		commonState = CommonState.None;
	}

	public override void UpdateElement()
	{
		if (commonState == CommonState.None)
		{
			return;
		}
		if (commonState == CommonState.Active)
		{
			float distance = 0f;
			if ((bool)base.groupTransform)
			{
				distance = (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - collidePos) / data.MoveDistance;
			}
			PlayByPercent(GetPercent(distance));
		}
		else
		{
			CommonState commonState2 = commonState;
			int num = 5;
		}
	}

	public override float GetPercent(float distance)
	{
		return distance * data.SpeedScaler;
	}

	public override void PlayByPercent(float percent)
	{
		if (percent <= data.SecondPercent)
		{
			percent /= data.SecondPercent;
			base.transform.localPosition = Vector3.Lerp(beginPos, endPos, percent);
		}
		else if (percent <= halfSecondPercent)
		{
			percent = (percent - data.SecondPercent) / (halfSecondPercent - data.SecondPercent);
			base.transform.localPosition = Vector3.Lerp(endPos, halfPos, percent);
		}
		else if (percent < 1f)
		{
			percent = (percent - halfSecondPercent) / (1f - halfSecondPercent);
			base.transform.localPosition = Vector3.Lerp(halfPos, endPos, percent);
		}
		else
		{
			base.transform.localPosition = endPos;
			commonState = CommonState.End;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (!ball)
		{
			return;
		}
		OnCollideBall(ball);
		if (commonState == CommonState.None)
		{
			collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
			commonState = CommonState.Active;
			if ((bool)state1)
			{
				state1.gameObject.SetActive(false);
			}
			if ((bool)state2)
			{
				state2.gameObject.SetActive(true);
			}
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		RD_TrapRootTile_DATA rD_TrapRootTile_DATA = JsonUtility.FromJson<RD_TrapRootTile_DATA>(rd_data as string);
		collidePos = rD_TrapRootTile_DATA.collidePos;
		state1.SetTransData(rD_TrapRootTile_DATA.state1);
		state2.SetTransData(rD_TrapRootTile_DATA.state2);
		base.transform.SetTransData(rD_TrapRootTile_DATA.trans);
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return JsonUtility.ToJson(new RD_TrapRootTile_DATA
		{
			collidePos = collidePos,
			trans = base.transform.GetTransData(),
			state1 = state1.GetTransData(),
			state2 = state2.GetTransData()
		});
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		RD_TrapRootTile_DATA rD_TrapRootTile_DATA = Bson.ToObject<RD_TrapRootTile_DATA>(rd_data);
		collidePos = rD_TrapRootTile_DATA.collidePos;
		state1.SetTransData(rD_TrapRootTile_DATA.state1);
		state2.SetTransData(rD_TrapRootTile_DATA.state2);
		base.transform.SetTransData(rD_TrapRootTile_DATA.trans);
	}

	public override byte[] RebirthWriteByteData()
	{
		return Bson.ToBson(new RD_TrapRootTile_DATA
		{
			collidePos = collidePos,
			trans = base.transform.GetTransData(),
			state1 = state1.GetTransData(),
			state2 = state2.GetTransData()
		});
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
