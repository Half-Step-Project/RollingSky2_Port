using UnityEngine;

public class TrapTriggerTile : BaseTrapTile
{
	private Vector3 beginPos;

	private Vector3 endPos;

	private Vector3 halfPos;

	private float halfSecondPercent;

	private float timeCounter;

	private Transform state1;

	private Transform state2;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
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
		beginPos = StartLocalPos;
		data.MoveDistance = Mathf.Abs(data.MoveDistance);
		data.MoveDistance2 = 0f - Mathf.Abs(data.MoveDistance2);
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
		timeCounter = 0f;
		commonState = CommonState.None;
	}

	public override void UpdateElement()
	{
		if (commonState != 0)
		{
			if (commonState == CommonState.Active)
			{
				OnTriggerPlay();
				return;
			}
			CommonState commonState2 = commonState;
			int num = 5;
		}
	}

	public override void OnTriggerPlay()
	{
		timeCounter += Time.deltaTime;
		float num = timeCounter * data.SpeedScaler;
		if (num >= 1f)
		{
			num = 1f;
			commonState = CommonState.End;
		}
		PlayByPercent(num);
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

	private void OnTriggerEnter(Collider collider)
	{
		if (commonState == CommonState.None && (bool)collider.gameObject.GetGameComponent<BaseTile>())
		{
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
}
