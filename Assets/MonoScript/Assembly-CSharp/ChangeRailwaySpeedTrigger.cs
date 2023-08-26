using System;
using System.IO;
using Foundation;
using UnityEngine;

public class ChangeRailwaySpeedTrigger : BaseTriggerBox
{
	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float BeginSpeed;

		public float TargetSpeed;

		public float MoveDistance;

		public RailwayMover.MoveDir MoveDir;

		public Railway.RailwayMoveDir ActiveDir;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginSpeed = bytes.GetSingle(ref startIndex);
			TargetSpeed = bytes.GetSingle(ref startIndex);
			MoveDistance = bytes.GetSingle(ref startIndex);
			MoveDir = (RailwayMover.MoveDir)bytes.GetInt32(ref startIndex);
			ActiveDir = (Railway.RailwayMoveDir)bytes.GetInt32(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TargetSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)MoveDir).GetBytes(), ref offset);
				memoryStream.WriteByteArray(((int)ActiveDir).GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TriggerData data;

	public bool IfTestPlay;

	private Vector3 testPos;

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
		if (commonState == CommonState.None)
		{
			Railway theRailway = Railway.theRailway;
			if (theRailway.GetMoveDir() == data.ActiveDir)
			{
				Vector3 position = theRailway.transform.position;
				float num = ((data.MoveDir == RailwayMover.MoveDir.Forward) ? 1f : (-1f));
				Vector3 pTarget = position + num * data.MoveDistance * base.transform.forward;
				MoveData moveData = new MoveData(data.BeginSpeed, data.TargetSpeed, position, pTarget, base.transform.forward * num);
				theRailway.ResetMoveData(moveData, data.MoveDir);
				commonState = CommonState.Active;
			}
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
		base.OnDrawGizmos();
	}
}
