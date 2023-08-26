using System;
using UnityEngine;

public class FallTile : BaseTile
{
	[Serializable]
	public struct TileData
	{
		public TileDirection MoveDirection;

		public float MoveDistance;

		public float BeginDistance;

		public float GameG;
	}

	public TileData data;

	private Vector3 beginPos;

	private Vector3 endPos;

	private bool playStart;

	public override void Initialize()
	{
		base.Initialize();
		beginPos = StartLocalPos;
		if (data.MoveDirection == TileDirection.Down)
		{
			data.MoveDistance = 0f - Mathf.Abs(data.MoveDistance);
		}
		else if (data.MoveDirection == TileDirection.Up)
		{
			data.MoveDistance = Mathf.Abs(data.MoveDistance);
		}
		endPos = beginPos + new Vector3(0f, data.MoveDistance, 0f);
	}

	public override void UpdateElement()
	{
		float magnitude = (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition) - base.groupTransform.InverseTransformPoint(base.transform.position)).magnitude;
		if (magnitude < data.BeginDistance)
		{
			playStart = true;
		}
		if (playStart)
		{
			PlayByPercent(magnitude);
		}
	}

	public override void PlayByPercent(float distance)
	{
		if (distance < 10f)
		{
			base.transform.localPosition += Vector3.down * 0.5f * data.GameG * Time.deltaTime * 10f * Time.deltaTime * 10f;
		}
		if (base.transform.localPosition.y <= endPos.y)
		{
			base.transform.localPosition = endPos;
			playStart = false;
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}
}
