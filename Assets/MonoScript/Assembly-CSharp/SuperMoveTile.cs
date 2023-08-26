using UnityEngine;

public class SuperMoveTile : BaseTile
{
	public Vector3 SourcePos;

	public Vector3 TargetPos;

	public float BeginDistance;

	public float SpeedScaler;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void DebugPlay(float distance)
	{
		distance -= TestDistance;
		PlayByPercent(GetPercent(distance));
	}

	public override void UpdateElement()
	{
		float distance = BaseRole.theBall.transform.position.z - base.transform.position.z + BeginDistance;
		PlayByPercent(GetPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Min(1f, Mathf.Max(-1f, distance * SpeedScaler));
	}

	public override void PlayByPercent(float percent)
	{
		if (percent < 0f)
		{
			base.transform.position = Vector3.Lerp(SourcePos, TargetPos, 1f + percent);
		}
		else
		{
			base.transform.position = TargetPos;
		}
	}
}
