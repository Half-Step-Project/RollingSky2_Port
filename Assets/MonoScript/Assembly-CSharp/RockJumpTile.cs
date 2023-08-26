using System;
using System.IO;
using Foundation;
using UnityEngine;

public class RockJumpTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public float JumpDistance;

		public float JumpHeight;

		public float MoveSpeed;

		public bool IfShowTrail;

		public Vector3 BeginPos;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			IfShowTrail = bytes.GetBoolean(ref startIndex);
			BeginPos = bytes.GetVector3(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfShowTrail.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
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

	private Transform effectChild;

	private Transform state1;

	private Transform state2;

	private Transform state3;

	public override bool IfRebirthRecord
	{
		get
		{
			return false;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.BeginDistance = (float)objs[0];
			data.ResetDistance = (float)objs[1];
			data.JumpDistance = (float)objs[2];
			data.JumpHeight = (float)objs[3];
			data.MoveSpeed = (float)objs[4];
			data.IfShowTrail = (bool)objs[5];
			data.BeginPos = base.transform.position;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		commonState = CommonState.None;
		if (effectChild == null)
		{
			effectChild = base.transform.Find("effect");
			if ((bool)effectChild && effectChild.gameObject.activeSelf)
			{
				effectChild.gameObject.SetActive(false);
			}
		}
		if (state1 == null || state2 == null || state3 == null)
		{
			state1 = base.transform.Find("model/state1");
			state2 = base.transform.Find("model/state2");
			state3 = base.transform.Find("model/state3");
		}
		if ((bool)state1)
		{
			state1.gameObject.SetActive(true);
		}
		if ((bool)state2)
		{
			state2.gameObject.SetActive(false);
		}
		if ((bool)state3)
		{
			state3.gameObject.SetActive(false);
		}
		beginPos = (endPos = StartLocalPos);
		collidePos = 10000f;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		collidePos = 10000f;
		OnTriggerStop();
	}

	public override void UpdateElement()
	{
		float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - collidePos;
		PlayByPercent(GetPercent(distance));
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (num >= data.BeginDistance)
			{
				OnTriggerPlay();
				commonState = CommonState.Active;
			}
		}
		else if (commonState == CommonState.Active && num >= data.ResetDistance)
		{
			OnTriggerStop();
			commonState = CommonState.End;
		}
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
		if (!ball)
		{
			return;
		}
		BaseRole.JumpType jType = BaseRole.JumpType.Normal;
		if (data.IfShowTrail)
		{
			jType = BaseRole.JumpType.Super;
		}
		ball.OnTileEnter(this);
		if (ball.IfJumpingDown)
		{
			ball.CallEndJump(base.transform.position.y);
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, jType);
			collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
			if ((bool)state1)
			{
				state1.gameObject.SetActive(false);
			}
			if ((bool)state2)
			{
				state2.gameObject.SetActive(false);
			}
			if ((bool)state3)
			{
				state3.gameObject.SetActive(true);
			}
		}
		else if (!ball.IfJumping)
		{
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, jType);
			collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
			if ((bool)state1)
			{
				state1.gameObject.SetActive(false);
			}
			if ((bool)state2)
			{
				state2.gameObject.SetActive(false);
			}
			if ((bool)state3)
			{
				state3.gameObject.SetActive(true);
			}
		}
		else if (ball.IfDropping)
		{
			ball.CallEndDrop(base.transform.position.y);
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, jType);
			collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
			if ((bool)state1)
			{
				state1.gameObject.SetActive(false);
			}
			if ((bool)state2)
			{
				state2.gameObject.SetActive(false);
			}
			if ((bool)state3)
			{
				state3.gameObject.SetActive(true);
			}
		}
	}

	public override void OnTriggerPlay()
	{
		if ((bool)effectChild)
		{
			effectChild.gameObject.SetActive(true);
		}
		if ((bool)state1)
		{
			state1.gameObject.SetActive(false);
		}
		if ((bool)state2)
		{
			state2.gameObject.SetActive(true);
		}
		if ((bool)state3)
		{
			state3.gameObject.SetActive(false);
		}
	}

	public override void OnTriggerStop()
	{
		if ((bool)effectChild)
		{
			effectChild.gameObject.SetActive(false);
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
		base.transform.Find("triggerPoint").position = data.BeginPos;
	}

	public override string Write()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
		base.transform.Find("triggerPoint").position = data.BeginPos;
	}

	public override byte[] WriteBytes()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return StructTranslatorUtility.ToByteArray(data);
	}

	private void OnDrawGizmos()
	{
		Transform transform = base.transform.Find("triggerPoint");
		if ((bool)transform)
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.red;
			Gizmos.DrawCube(transform.position, new Vector3(1f, 0.1f, 0.1f));
			Gizmos.color = color;
		}
	}
}
