using UnityEngine;

public class CouplePathMoveData
{
	public Vector3[] BezierPoints;

	public Vector3[] BezierSlowPoints;

	public AnimationCurve SpeedQuickCurve;

	public AnimationCurve SpeedSlowCurve;

	public TreasureBelongType BelongType;

	public bool IfFollowDir;

	public int triggerUuid = -1;

	public CouplePathMoveData(Vector3[] bezierPoints, Vector3[] bezierSlowPoints, AnimationCurve quickCurve, AnimationCurve slowCurve, bool followDir, TreasureBelongType belongType, int uuid = -1)
	{
		ResetData(bezierPoints, bezierSlowPoints, quickCurve, slowCurve, followDir, belongType, uuid);
	}

	public void Clear()
	{
		BezierPoints = null;
		SpeedQuickCurve = null;
		SpeedSlowCurve = null;
		triggerUuid = -1;
	}

	public void ResetData(Vector3[] bezierPoints, Vector3[] bezierSlowPoints, AnimationCurve quickCurve, AnimationCurve slowCurve, bool followDir, TreasureBelongType belongType, int uuid)
	{
		BezierPoints = bezierPoints;
		BezierSlowPoints = bezierSlowPoints;
		SpeedQuickCurve = quickCurve;
		SpeedSlowCurve = slowCurve;
		IfFollowDir = followDir;
		BelongType = belongType;
		triggerUuid = uuid;
	}
}
