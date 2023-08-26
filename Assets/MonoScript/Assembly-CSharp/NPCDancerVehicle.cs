using System;
using Foundation;
using UnityEngine;

public class NPCDancerVehicle : BaseVehicle
{
	public enum Status
	{
		eNone,
		eWait,
		eMove,
		eDepart
	}

	[Serializable]
	public struct TriggerData : IReadWriteBytes
	{
		public float BeginDistance;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			return BeginDistance.GetBytes();
		}
	}

	public static readonly string WaitAnimName = "anim01";

	public static readonly string CollideAnimName = "anim02";

	public TriggerData data;

	private Status currentStatus;

	private Animation anim;

	public override float VehicleRotateSpeed
	{
		get
		{
			return 0f;
		}
	}

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
		anim = base.transform.GetComponentInChildren<Animation>();
		currentStatus = Status.eNone;
	}

	public override void ResetElement()
	{
		base.ResetElement();
		currentStatus = Status.eNone;
		PlayAnim(anim, false);
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (currentStatus == Status.eWait)
		{
			PlayAnim(anim, CollideAnimName, true);
			base.TriggerEnter(ball);
			AddToBall(ball);
			ball.TriggerRolePlayAnim(BaseRole.AnimType.DanceWithNPC);
			currentStatus = Status.eMove;
		}
	}

	public override void UpdateElement()
	{
		if (currentStatus == Status.eNone)
		{
			if (base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z >= data.BeginDistance)
			{
				PlayAnim(anim, WaitAnimName, true);
				currentStatus = Status.eWait;
			}
		}
		else if (currentStatus != Status.eWait && currentStatus == Status.eMove && !anim.isPlaying)
		{
			currentStatus = Status.eDepart;
			BaseRole.theBall.DepartVehicle();
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

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		return "";
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
	}

	public override byte[] RebirthWriteByteData()
	{
		return null;
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
	}
}
