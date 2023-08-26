using System;
using System.IO;
using Foundation;
using UnityEngine;

public class JumpDirTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public Railway.RailwayMoveDir MoveDir;

		public float JumpDistance;

		public float JumpHeight;

		public float MoveSpeed;

		public bool IfChangeSpeed;

		public float BeginSpeed;

		public float TargetSpeed;

		public float MoveDistance;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			MoveDir = (Railway.RailwayMoveDir)bytes.GetInt32(ref startIndex);
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			IfChangeSpeed = bytes.GetBoolean(ref startIndex);
			BeginSpeed = bytes.GetSingle(ref startIndex);
			TargetSpeed = bytes.GetSingle(ref startIndex);
			MoveDistance = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(((int)MoveDir).GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(IfChangeSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TargetSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TriggerData data;

	[Range(0f, 2f)]
	public float debugPercent;

	private JumpUtil debugJumpInfo;

	public override void SetDefaultValue(object[] objs)
	{
		if (objs != null)
		{
			data.MoveDir = (Railway.RailwayMoveDir)objs[0];
			data.JumpDistance = (float)objs[1];
			data.JumpHeight = (float)objs[2];
			data.MoveSpeed = (float)objs[3];
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		commonState = CommonState.None;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		commonState = CommonState.None;
	}

	public override void TriggerEnter(BaseRole ball)
	{
		Railway theRailway = Railway.theRailway;
		if (commonState == CommonState.None)
		{
			if (theRailway.GetMoveDir() != data.MoveDir)
			{
				if (data.IfChangeSpeed)
				{
					Vector3 vector = ((data.MoveDir == Railway.RailwayMoveDir.Forward) ? ball.transform.forward : (ball.transform.forward * -1f));
					Vector3 position = theRailway.transform.position;
					MoveData moveData = new MoveData(data.BeginSpeed, data.TargetSpeed, position, position + vector * data.MoveDistance, vector);
					theRailway.ResetMoveData(moveData, data.MoveDir);
				}
				else
				{
					theRailway.ChangeMoveDirTo(data.MoveDir);
				}
				LetRoleJump(ball);
				commonState = CommonState.Active;
			}
		}
		else
		{
			CommonState commonState2 = commonState;
			int num = 2;
		}
	}

	private void LetRoleJump(BaseRole ball)
	{
		if (ball.IfJumpingDown)
		{
			ball.CallEndJump(base.transform.position.y);
		}
		Vector3 vector = ((data.MoveDir == Railway.RailwayMoveDir.Forward) ? ball.transform.forward : (ball.transform.forward * -1f));
		if (!ball.IfJumping)
		{
			ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * vector, vector, data.JumpHeight, BaseRole.JumpType.Back);
		}
		else
		{
			bool ifDropping = ball.IfDropping;
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TriggerData>(info);
	}

	public override string Write()
	{
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TriggerData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		return StructTranslatorUtility.ToByteArray(data);
	}

	public override void OnDrawGizmos()
	{
	}
}
