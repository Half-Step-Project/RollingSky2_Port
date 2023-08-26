using System;
using System.IO;
using Foundation;
using UnityEngine;

public class JumpShipTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float MoveSpeed;

		public float JumpHeight;

		public float MoveDistance;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveSpeed = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			MoveDistance = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	private Vector3 beginPos;

	private Vector3 endPos;

	private float collidePos;

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
		beginPos = (endPos = StartLocalPos);
		collidePos = 10000f;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		collidePos = 10000f;
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - collidePos;
		PlayByPercent(GetPercent(distance));
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Min(2f, Mathf.Max(0f, distance));
	}

	public override void PlayByPercent(float percent)
	{
		if (percent > 0f && percent <= 1f)
		{
			float num = data.MoveSpeed - data.MoveSpeed * (percent - 1f) * (percent - 1f);
			base.transform.localPosition = new Vector3(endPos.x, endPos.y + num, endPos.z);
		}
		else if (percent > 1f && percent < 2f)
		{
			float num2 = data.MoveSpeed - data.MoveSpeed * (percent - 1f) * (percent - 1f);
			base.transform.localPosition = new Vector3(endPos.x, endPos.y + num2, endPos.z);
		}
		else if (percent <= 0f || percent >= 2f)
		{
			base.transform.localPosition = endPos;
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if ((bool)ball)
		{
			ball.OnTileEnter(this);
			if (ball.IfJumpingDown)
			{
				ball.CallEndJump(base.transform.position.y);
				JumpBall(ball);
				PlaySoundEffect();
			}
			else if (!ball.IfJumping)
			{
				JumpBall(ball);
				PlaySoundEffect();
			}
			else if (ball.IfDropping)
			{
				ball.CallEndDrop(base.transform.position.y);
				JumpBall(ball);
				PlaySoundEffect();
			}
		}
	}

	private void JumpBall(BaseRole ball)
	{
		InputController.instance.EnableInput(false);
		ball.CallBeginJump(base.transform.position, base.transform.position + data.MoveDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.Ship);
		collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
		if ((bool)state1)
		{
			state1.gameObject.SetActive(false);
		}
		if ((bool)state2)
		{
			state2.gameObject.SetActive(true);
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

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}
}
