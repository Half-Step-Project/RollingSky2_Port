using UnityEngine;

public class JumpInfo
{
	public Vector3 beginPos;

	public Vector3 endPos;

	public Vector3 groupBeginPos;

	public Vector3 groupEndPos;

	public Vector3 beginDir;

	public Vector3 endDir;

	public float targetQteDir;

	public Transform groupTransform;

	public float speedScaler;

	public JumpSpeedType speedType;

	public QTEDirection qteDir;

	public float limitTime;

	public float showTime;

	public bool ifAuto;

	public JumpInfo()
	{
	}

	public JumpInfo(Vector3 bd, Vector3 ed, Vector3 bp, Vector3 ep, float ss, JumpSpeedType jst, float tqd, QTEDirection qd, Transform gt, float lt, float st, bool ia = false)
	{
		beginDir = bd;
		endDir = ed;
		beginPos = bp;
		endPos = ep;
		groupTransform = gt;
		groupBeginPos = groupTransform.InverseTransformPoint(beginPos);
		groupEndPos = groupTransform.InverseTransformPoint(endPos);
		speedScaler = ss;
		speedType = jst;
		targetQteDir = tqd;
		qteDir = qd;
		limitTime = lt;
		ifAuto = ia;
		showTime = st;
	}
}
